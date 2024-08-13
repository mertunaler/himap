using IMAP.Core;
using IMAP.Core.CommandHandler;
using NuGet.Frameworks;

namespace Core.UnitTests;

public class ImapClientTests
{
    [Fact]
    public void ConnectsToImapServer()
    {
        var sut = GetImapClient();

        bool isConnected = sut.ConnectAsync("imap.gmail.com", 993).Result;

        Assert.True(isConnected);
    }
    private ImapClient GetImapClient()
    {
        return new ImapClient();
    }

}