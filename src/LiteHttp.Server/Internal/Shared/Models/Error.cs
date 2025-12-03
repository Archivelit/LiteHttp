namespace LiteHttp.Models;

public readonly struct Error
{
    public readonly string ErrorMessage;
    public readonly int ErrorCode;

    public  Error(int errorCode, string errorMessage)
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