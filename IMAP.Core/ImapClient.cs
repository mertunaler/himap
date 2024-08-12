using System;
using System.Net.Security;
using System.Security.Authentication;
using IMAP.Core.CommandHandler;

namespace IMAP.Core;

public class ImapClient : IDisposable
{
    TcpClient _tcpClient;
    IMAPCommandBuilder _builder;
    public ImapClient()
    {
        _tcpClient = new TcpClient();
        _builder = new IMAPCommandBuilder();
    }

    public async Task ConnectAsync(string hostName, int port)
    {
        await _tcpClient.ConnectAsync(hostName, port);
    }
    public async Task AuthenticateAsync(string username, string password)
    {
        string command = _builder.WithCommand("LOGIN")
                                 .WithParameters(username, password)
                                 .Build();

        await _tcpClient.SendAsync(command);
        string response = await _tcpClient.RetrieveAsync();

        if (!IsAuthSuccessful(response))
            throw new AuthenticationException("Authentication failed.");
    }
    public void Dispose()
    {
        throw new NotImplementedException();
    }

    private bool IsAuthSuccessful(string response)
    {
        if (string.IsNullOrEmpty(response))
            return false;

        return response.StartsWith("OK", StringComparison.InvariantCultureIgnoreCase);

    }
}
