namespace LiteHttp.Models;

public readonly struct Error
{
    public readonly string ErrorMessage;
    public readonly int ErrorCode;

    public Error(string errorMessage, int errorCode)
    {
        ErrorMessage = errorMessage;
        ErrorCode = errorCode;
    }

    public Error(int errorCode)
    {
        ErrorCode = errorCode;
        ErrorMessage = string.Empty;
    }
}