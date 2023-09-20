namespace bramvandenbussche.readwiser.api.Contract;

public abstract class ApiResponse
{
    public string Reason { get; set; }
    public bool IsSuccess { get; set; }

    /// <summary>
    /// Create a new response object.
    /// If you add a reason, the response will indicate a failure
    /// </summary>
    /// <param name="reason"></param>
    protected ApiResponse(string reason = "")
    {
        IsSuccess = !string.IsNullOrEmpty(reason);
        Reason = reason;
    }
}