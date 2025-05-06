using System.Net;
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
    IOrderHttpClient orderHttpClient,
    ILogger<PaymentServiceImpl> logger) : IPaymentService
{
    public async Task<ServiceResult<PaymentDto>> ProcessPayment(
        Guid userId,
        ProcessPaymentRequest request,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var (isSuccess, result) = await FetchOrder(orderId: request.OrderId, cancellationToken: cancellationToken);

            if (isSuccess is false)
            {
                return PaymentResults<PaymentDto>.HttpRequestFailed(result);
            }

            var order = result?.Data;

            if (order is null)
            {
                logger.LogInformation("Order not found. Order Id: {OrderId}", request.OrderId);
                return PaymentResults<PaymentDto>.OrderNotFound(request.OrderId);
            }

            logger.LogInformation(
                "Start processing payment. Order Id: {OrderId}",
                request.OrderId);

            if (order.PaymentState.Equals("Paid", StringComparison.OrdinalIgnoreCase))
            {
                logger.LogInformation(
                    "Order amount already paid. Order Id: {OrderId}, Payment Status: {PaymentStatus}",
                    order.Id,
                    order.PaymentState);
                return PaymentResults<PaymentDto>.AmountAlreadyPaid;
            }

            if (order.TotalPrice > request.Amount)
            {
                logger.LogInformation(
                    "Order total price is getter than request amount. Order Id: {OrderId}",
                    request.OrderId);
                return PaymentResults<PaymentDto>.InsufficientAmount;
            }

            (PaymentStatus paymentStatus, string message) = await PaidOrderAmount(
                paymentMode: request.PaymentMode,
                amount: request.Amount,
                cancellationToken: cancellationToken);

            var payment = await SavePayment(
                userId: userId,
                request: request,
                paymentStatus: paymentStatus,
                cancellationToken: cancellationToken);

            if (paymentStatus == PaymentStatus.Failed)
            {
                logger.LogInformation(
                    "Payment transaction failed. Order Id: {OrderId}, Error: {Error}",
                    request.OrderId,
                    message);
                return PaymentResults<PaymentDto>.PaymentFailed(message, payment.ToDto());
            }

            logger.LogInformation(
                "Payment processed successfully. Payment: {@Payment}",
                payment);
            return PaymentResults<PaymentDto>.PaymentProcessed(payment.ToDto());
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

    private async Task<(bool isSuccess, ServiceResult<OrderDto>? result)> FetchOrder(Guid orderId, CancellationToken cancellationToken)
    {
        logger.LogInformation("Fetching cart details.");
        var result = await orderHttpClient.GetOrder(orderId: orderId, cancellationToken: cancellationToken);

        if (result == null || result.StatusCode != HttpStatusCode.OK)
        {
            logger.LogWarning(
                "Failed to fetched order details. Result: {@Result}",
                result);
            return (false, result);
        }

        logger.LogInformation(
            "Order details fetched successfully. Result: {@Result}",
            result);
        return (true, result);
    }

    private async Task<(PaymentStatus paymentStatus, string message)> PaidOrderAmount(
        PaymentMode paymentMode,
        decimal amount,
        CancellationToken cancellationToken)
    {
        try
        {
            logger.LogInformation(
                "Payment transaction started. Payment Mode: {PaymentMode}, Amount: {Amount}",
                paymentMode,
                amount);
            var paymentProvider = paymentProviderFactory.GetPaymentProvider(paymentMode);

            var paymentStatus = await retryHandler.ExecuteWithRetryAsync(
                operationName: $"MakePayment-{paymentMode}",
                action: async token => await paymentProvider.ProcessTransaction(amount, token),
                cancellationToken: cancellationToken);

            logger.LogInformation(
                "Payment transaction completed. Payment Mode: {PaymentMode}, Amount: {Amount},Payment Status: {PaymentStatus}",
                paymentMode,
                amount,
                paymentStatus);

            return (paymentStatus, string.Empty);
        }
        catch (PaymentException ex)
        {
            logger.LogError(
                ex,
                "Payment transaction failed.Payment Mode: {PaymentMode}, Amount: {Amount} Error: {Error}",
                paymentMode,
                amount,
                ex.Message);

            return (PaymentStatus.Failed, ex.Message);
        }
    }

    private async Task<Payment> SavePayment(
        Guid userId,
        ProcessPaymentRequest request,
        PaymentStatus paymentStatus,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Start creating payment. Order Id: {OrderId}", request.OrderId);
        var payment = Payment.Create(
            orderId: request.OrderId,
            userId: userId,
            amount: request.Amount,
            paymentMode: request.PaymentMode);

        payment.UpdateStatus(paymentStatus);
        var updatedPayment = await paymentRepository.Create(payment: payment, cancellationToken: cancellationToken);

        logger.LogInformation(
            "Payment created successfully. Payment: {Payment}",
            payment);
        return updatedPayment;
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
