namespace LiteHttp.Models;

/// <summary>
/// Represents the outcome of an operation that can succeed with a value of the specified type or fail with an error.
/// </summary>
/// <remarks>
/// A Result<TResult> instance encapsulates either a successful result, accessible through the Value
/// property, or a failure, described by the Error property. The Success property indicates whether the operation was
/// successful. This type is commonly used to represent operations that may fail without throwing exceptions, enabling
/// more explicit error handling.
/// </remarks>
/// <typeparam name="TResult">
/// The type of the value returned when the operation is successful.
/// </typeparam>
public readonly record struct Result<TResult>
{
    /// <summary>
    /// Gets the error information associated with the current operation, if any.
    /// </summary>
    public Error? Error { get; }
    /// <summary>
    /// Gets the result value if the operation was successful; otherwise, returns null.
    /// </summary>
    public TResult? Value { get; }
    /// <summary>
    /// Gets a value indicating whether the operation completed successfully.
    /// </summary>
    public bool Success { get; }

    /// <summary>
    /// Initializes a new instance of the Result class that represents a failed operation with the specified error.
    /// </summary>
    /// <param name="error">The error information describing the reason for the failure. Cannot be null.</param>
    public Result(Error error)
    {
        Error = error;
        Success = false;
    }

    /// <summary>
    /// Initializes a new instance of the Result class that represents a successful operation with the specified result value.
    /// </summary>
    /// <param name="result">The value to assign to the result. This value will be accessible through the <see cref="Value"/> property.</param>
    public Result(TResult result)
    {
        Value = result;
        Success = true;
    }
}