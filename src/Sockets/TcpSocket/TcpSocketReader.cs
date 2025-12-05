namespace LiteHttp.Sockets.TcpSocket;

internal sealed class TcpSocketReader
{
    public async Task ReceiveAsync(Socket connection, Pipe pipe)
    {
        while (true)
        {
            try
            {
                Memory<byte> buffer = pipe.Writer.GetMemory(512);
                var bytesRead = await connection.ReceiveAsync(buffer);

                if (bytesRead == 0)
                    break;
                pipe.Writer.Advance(bytesRead);
            }
            catch (Exception ex)
            {
                break;
            }
            
            var flushResult = await pipe.Writer.FlushAsync();

            if (flushResult.IsCompleted)
                break;
        }

        await pipe.Writer.CompleteAsync();
    }
}