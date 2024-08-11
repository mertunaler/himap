using System;
using System.Net.Security;

namespace IMAP.Core;

public class ImapClient : IDisposable
{
    TcpClient _tcpClient;
    public ImapClient()
    {
        _tcpClient = new TcpClient();
    }

    public async Task ConnectAsync(string hostName, int port)
    {
        await _tcpClient.ConnectAsync(hostName, port);
    }
    public async Task AuthenticateAsync(string username, string password)
    {
        
    }


    public void Dispose()
    {
        throw new NotImplementedException();
    }
}
