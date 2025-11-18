namespace LiteHttp.Abstractions;

public interface IParser
{
    /// <summary>
    /// Parses the entire request bytes into <see cref="HttpContext"/> model.
    /// </summary>
    /// <param name="request">Entire request bytes.</param>
    /// <returns></returns>
    Result<HttpContext> Parse(Memory<byte> request);
}