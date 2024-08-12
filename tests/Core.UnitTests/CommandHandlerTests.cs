using IMAP.Core.CommandHandler;

namespace Core.UnitTests;

public class UnitTest1
{
    [Fact]
    public void GeneratesValidCommand()
    {
        string expectedCmd = "LOGIN w3rt w3rt";
        var sut = GetCommandBuilderObject();

        string generatedCmd = sut.WithCommand("LOGIN")
                                 .WithParameters("w3rt", "w3rt")
                                 .Build();

        Assert.Equal(expectedCmd, generatedCmd);
    }
    [Fact]
    public void ReturnsValidCommandWithoutParams()
    {
        string expectedCmd = "LOGIN";
        var sut = GetCommandBuilderObject();

        string generatedCmd = sut.WithCommand("LOGIN")
                                  .Build();

        Assert.Equal(expectedCmd, generatedCmd);
    }
    private IMAPCommandBuilder GetCommandBuilderObject()
    {
        return new IMAPCommandBuilder();
    }
}