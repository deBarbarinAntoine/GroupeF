namespace YellowDirectory.Models;

/// <summary>
/// ErrorViewModel is the model associated to the Error view.
/// </summary>
public class ErrorViewModel
{
    public string? RequestId { get; init; }

    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
}