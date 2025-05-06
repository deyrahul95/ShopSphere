using Microsoft.Extensions.Logging;
using PaymentService.Application.DTOs;
using PaymentService.Application.Extensions;
using PaymentService.Application.Handlers;
using PaymentService.Application.Models;
using PaymentService.Application.Results;
using PaymentService.Application.Services.Interfaces;
using PaymentService.Domain.Entities;
using PaymentService.Domain.Enums;
using PaymentService.Domain.Exceptions;
using PaymentService.Domain.Providers;
using PaymentService.Domain.Repositories;

namespace PaymentService.Application.Services;

public class PaymentServiceImpl(
    IPaymentRepository paymentRepository,
    IPaymentProviderFactory paymentProviderFactory,
    RetryHandler retryHandler,
    ILogger<PaymentServiceImpl> logger) : IPaymentService
{
    public async Task<ServiceResult<PaymentDto>> ProcessPayment(
    Guid userId,
    ProcessPaymentRequest request,
    CancellationToken cancellationToken = default)
    {
        try
        {
            //TODO: Fetch order data 

            logger.LogInformation(
                "Start processing payment. Order Id: {OrderId}",
                request.OrderId);

            var payment = Payment.Create(
                orderId: request.OrderId,
                userId: userId,
                amount: request.Amount,
                paymentMode: request.PaymentMode);

            (PaymentStatus paymentStatus, string message) = await MakePayment(
                paymentMode: request.PaymentMode,
                amount: payment.Amount,
                cancellationToken: cancellationToken);

            payment.UpdateStatus(paymentStatus);
            var updatedPayment = await paymentRepository.Create(payment, cancellationToken);

            if (paymentStatus == PaymentStatus.Failed)
            {
                logger.LogInformation(
                    "Payment transaction failed. Order Id: {OrderId}, Error: {Error}",
                    request.OrderId,
                    message);
                return PaymentResults<PaymentDto>.PaymentFailed(message);
            }

            logger.LogInformation(
                "Payment processed successfully. Payment: {@Payment}",
                updatedPayment);
            return PaymentResults<PaymentDto>.PaymentProcessed(updatedPayment.ToDto());
        }
        catch (PaymentValidationException ex)
        {
            logger.LogError(
                ex,
                "Domain validation failed. User Id: {UserId}, Order Id: {OrderId}, Error: {Error}",
                userId,
                request.OrderId,
                ex.Message);

            return PaymentResults<PaymentDto>.ValidationFailed([ex.ToError()]);
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex,
                "Failed to create payment. User Id: {UserId}, Order Id: {OrderId}, Error: {Error}",
                userId,
                request.OrderId,
                ex.Message);

            return PaymentResults<PaymentDto>.InternalServerError;
        }
    }

    public async Task<ServiceResult<PaymentDto>> GetPaymentDetails(
        Guid orderId,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var paymentDto = await GetPaymentByOrderId(orderId, cancellationToken);

            if (paymentDto is null)
            {
                return PaymentResults<PaymentDto>.PaymentNotFound(orderId);
            }

            return PaymentResults<PaymentDto>.PaymentFetched(paymentDto);
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex,
                "Failed to fetched payment details. Order Id: {OrderId}, Error: {Error}",
                orderId,
                ex.Message);

            return PaymentResults<PaymentDto>.InternalServerError;
        }
    }

    private async Task<(PaymentStatus paymentStatus, string message)> MakePayment(
        PaymentMode paymentMode,
        decimal amount,
        CancellationToken cancellationToken)
    {
        try
        {
            var paymentProvider = paymentProviderFactory.GetPaymentProvider(paymentMode);

            var paymentStatus = await retryHandler.ExecuteWithRetryAsync(
                operationName: $"MakePayment-{paymentMode}",
                action: async token => await paymentProvider.ProcessTransaction(amount, token),
                cancellationToken: cancellationToken);

            return (paymentStatus, string.Empty);
        }
        catch (PaymentException ex)
        {
            logger.LogError(
                ex,
                "Payment failed. Error: {Error}",
                ex.Message);

            return (PaymentStatus.Failed, ex.Message);
        }
    }

    private async Task<PaymentDto?> GetPaymentByOrderId(
        Guid orderId,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Fetching payment details. Order Id: {OrderId}", orderId);

        var payment = await paymentRepository.GetByOrderId(orderId: orderId, cancellationToken: cancellationToken);

        if (payment is null)
        {
            logger.LogWarning("Payment details not found. Order Id: {OrderId}", orderId);
            return null;
        }

        logger.LogInformation("Payment fetch successfully. Payment: {@Payment}", payment);
        return payment.ToDto();
    }
}
