namespace CartService.Application.Constants;

public class HttpClientConstants
{
    public const string BaseAddress = "BaseAddress";
    public const string ProductPipelineName = "ProductPipeline";

    public const int RequestTimeoutInSeconds = 5;
    public const int MaxRetryCount = 3;
    public const int RetryDelayInMilliseconds = 500;
    public const int SamplingDurationInSeconds = 30;
    public const double FailureRatio = 0.1;
    public const int MinimumThroughput = 100;
    public const int BreakDurationInSeconds = 5;
}
