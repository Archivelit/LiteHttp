namespace LiteHttp.Logging;

public sealed class NullLogger : ILogger
{
    public static readonly NullLogger Instance = new NullLogger();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void LogDebug(FormattableString message) { }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void LogError(Exception ex, FormattableString message) { }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void LogInformation(FormattableString message) { }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void LogTrace(FormattableString message) { }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void LogWarning(FormattableString message) { }

    public ILogger<TContext> ForContext<TContext>() =>
        NullLogger<TContext>.Instance;
}

public sealed class NullLogger<TCategoryName> : ILogger<TCategoryName>
{
    public static readonly NullLogger<TCategoryName> Instance = new NullLogger<TCategoryName>();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void LogDebug(FormattableString message) { }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void LogError(Exception ex, FormattableString message) { }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void LogInformation(FormattableString message) { }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void LogTrace(FormattableString message) { }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void LogWarning(FormattableString message) { }

    public ILogger<TContext> ForContext<TContext>() =>
        NullLogger<TContext>.Instance;
}