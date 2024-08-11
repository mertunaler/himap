using System;

namespace IMAP.Core.CommandHandler;

public interface ICommandBuilder
{
    ICommandBuilder WithCommand(string command);
    ICommandBuilder WithParameters(params string[] parameters);
     string Build();

}
