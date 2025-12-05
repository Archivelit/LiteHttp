namespace LiteHttp.Sockets.TcpSocket;

internal class TcpSocketWriter
{
    public async Task SendAsync(Socket socket, Pipe pipe)
    {
        while (true)
        {
            var result = await pipe.Reader.ReadAsync();
            if (result.IsCompleted)
                break;
            
            try
            {
                foreach (var segment in result.Buffer)
                    await socket.SendAsync(segment);
            }
            catch
            {
                break;
            }

            pipe.Reader.AdvanceTo(result.Buffer.Start, result.Buffer.End);
        }

        await pipe.Reader.CompleteAsync();
    }
}