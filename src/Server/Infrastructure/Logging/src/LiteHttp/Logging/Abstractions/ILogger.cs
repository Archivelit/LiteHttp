namespace LiteHttp.Logging.Abstractions;

/// <summary>
/// Represents the base logging interface used across the application.
/// </summary>
/// <remarks>
/// Implement this interface to provide a custom logging implementation or 
/// to integrate with an external logging provider.
/// </remarks>
public interface ILogger
{
    /// <summary>
    /// Writes a log message with the <c>Trace</c> severity level.
    /// </summary>
    /// <param name="message">
    /// A <see cref="FormattableString"/> whose text and arguments should be logged.
    /// </param>
    public void LogTrace(FormattableString message);

    /// <summary>
    /// Writes a log message with the <c>Debug</c> severity level.
    /// </summary>
    /// <param name="message">
    /// A <see cref="FormattableString"/> whose text and arguments should be logged.
    /// </param>
    public void LogDebug(FormattableString message);

    /// <summary>
    /// Writes a log message with the <c>Information</c> severity level.
    /// </summary>
    /// <param name="message">
    /// A <see cref="FormattableString"/> whose text and arguments should be logged.
    /// </param>
    public void LogInformation(FormattableString message);

    /// <summary>
    /// Writes a log message with the <c>Warning</c> severity level.
    /// </summary>
    /// <param name="message">
    /// A <see cref="FormattableString"/> whose text and arguments should be logged.
    /// </param>
    public void LogWarning(FormattableString message);

    /// <summary>
    /// Writes a log message with the <c>Error</c> severity level, including an exception.
    /// </summary>
    /// <param name="ex">The exception associated with this log entry.</param>
    /// <param name="message">
    /// A <see cref="FormattableString"/> whose text and arguments should be logged.
    /// </param>
    public void LogError(Exception ex, FormattableString message);

    /// <summary>
    /// Writes a log message with the <c>Error</c> severity level.
    /// </summary>
    /// <param name="message">
    /// A <see cref="FormattableString"/> whose text and arguments should be logged.
    /// </param>
    public void LogError(FormattableString message);

    /// <summary>
    /// Creates a category-specific logger for the specified <typeparamref name="TContext"/>.
    /// </summary>
    /// <typeparam name="TContext">
    /// The type that defines the logging category. Typically, the class where logging occurs.
    /// </typeparam>
    /// <returns>
    /// A new <see cref="ILogger{TContext}"/> instance associated with <typeparamref name="TContext"/>.
    /// </returns>
    public ILogger<TContext> ForContext<TContext>();
}

/// <summary>
/// Represents a category-specific logger, typically bound to the type
/// where the logging occurs.
/// </summary>
/// <typeparam name="TCategoryName">
/// The type defining the logging category.
/// </typeparam>
public interface ILogger<TCategoryName> : ILogger;