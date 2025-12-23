using HttpContext = LiteHttp.Models.PipeContextModels.HttpContext;

namespace LiteHttp.RequestProcessors;

internal sealed class HttpContextBuilder
{
    private ReadOnlyMemory<byte> _method;
    private ReadOnlyMemory<byte> _route;
    private ReadOnlyMemory<byte> _protocolVersion;
    private ReadOnlySequence<byte>? _body;
    private Dictionary<ReadOnlyMemory<byte>, ReadOnlyMemory<byte>> _headers = [];
    
    [SkipLocalsInit]
    public void Reset()
    {
        _method = ReadOnlyMemory<byte>.Empty;
        _route = ReadOnlyMemory<byte>.Empty;
        _protocolVersion = ReadOnlyMemory<byte>.Empty;
        _body = null;
        _headers = [];
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void WithMethod(ReadOnlyMemory<byte> method) => _method = method;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void WithProtocolVersion(ReadOnlyMemory<byte> protocolVersion) => 
        _protocolVersion = protocolVersion;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void WithRoute(ReadOnlyMemory<byte> route) => _route = route;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void WithBody(ReadOnlySequence<byte>? body) => _body = body;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void AddHeader(ReadOnlyMemory<byte> key, ReadOnlyMemory<byte> value) => _headers.Add(key, value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void WithHeaders(HeaderCollection headerCollection) => _headers = headerCollection.Headers;

    public HttpContext Build() => new HttpContext(_method, _route, _headers, _body);
}