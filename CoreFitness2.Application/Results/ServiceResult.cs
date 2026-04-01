namespace CoreFitness2.Application.Results;

public class ServiceResult
{
    public bool Succeeded { get; private set; }
    public string? ErrorMessage { get; private set; }

    public static ServiceResult Success() => new() { Succeeded = true };

    public static ServiceResult Failure(string errorMessage) => new()
    {
        Succeeded = false,
        ErrorMessage = errorMessage
    };
}