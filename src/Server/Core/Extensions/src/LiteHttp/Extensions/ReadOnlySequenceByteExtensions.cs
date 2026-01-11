using System.Buffers;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace LiteHttp.Extensions;

public static class ReadOnlySequenceByteExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [SkipLocalsInit]
    public static ReadOnlyMemory<byte> GetReadOnlyMemoryFromSequence(in this ReadOnlySequence<byte> methodSequence)
    {
        if (SequenceMarshal.TryGetReadOnlyMemory(methodSequence, out var memory))
        {
            return memory;
        }

        using var methodMemoryOwner = MemoryPool<byte>.Shared.Rent((int)methodSequence.Length);
        methodSequence.CopyTo(methodMemoryOwner.Memory.Span);

        return methodMemoryOwner.Memory;
    }
}
