namespace LiteHttp.RequestProcessors;

public sealed class Responder : IResponder
{
    public async Task SendResponse(Socket connection, string response)
    {
        var encodedResponse = Encoding.UTF8.GetBytes(response);

        await connection.SendAsync(encodedResponse).ConfigureAwait(false);
    }
}