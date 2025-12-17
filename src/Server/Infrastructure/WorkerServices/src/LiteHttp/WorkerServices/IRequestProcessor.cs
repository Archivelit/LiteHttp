namespace LiteHttp.Abstractions;

internal interface IRequestProcessor
{
    public Result<ReadOnlyMemory<byte>> Process(Memory<byte> request);
}