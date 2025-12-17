namespace LiteHttp.ErrorHandling;

/// <summary>
/// Represents an error with an associated error code and message.
/// </summary>
/// <remarks>
/// Use this structure to convey error information, including a numeric code and an optional descriptive
/// message, within internal components. The struct is immutable and intended for internal error reporting
/// scenarios.
/// </remarks>
public readonly struct Error
{
    /// <summary>
    /// Provides a descriptive message detailing the error condition.
    /// </summary>
    public readonly string ErrorMessage;
    /// <summary>
    /// Represents the error code associated with the current operation or result.
    /// </summary>
    public readonly int ErrorCode;

    /// <summary>
    /// Initializes a new instance of the <see cref="Error"/> class with the specified error code and message.
    /// </summary>
    /// <param name="errorCode">The numeric code that identifies the type or category of the error.</param>
    /// <param name="errorMessage">A descriptive message that provides details about the error.</param>
    public Error(int errorCode, string errorMessage)
    {
        ErrorMessage = errorMessage;
        ErrorCode = errorCode;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Error"/> class with the specified error code.
    /// </summary>
    /// <remarks>
    /// Recomended to use this constructor when no descriptive message is necessary or available.
    /// </remarks>
    /// <param name="errorCode">
    /// The numeric code that identifies the error. The meaning of the code depends on the context in which the error occurs.
    /// </param>
    public Error(int errorCode)
    {
        ErrorCode = errorCode;
        ErrorMessage = string.Empty;
    }
}