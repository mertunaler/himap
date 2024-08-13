using IMAP.Core;

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
    [Fact]
    public void AuthenticatesSuccessfully()
    {
        var sut = GetImapClient();
        bool isConnected = sut.ConnectAsync("imap.gmail.com", 993).Result;
        //I may have to create configreader...
        bool isAuthenticated = sut.AuthenticateAsync("CENSORED", "CENSORED").Result;

        Assert.True(isAuthenticated);
    }
    private ImapClient GetImapClient()
    {
        return new ImapClient();
    }
    

}