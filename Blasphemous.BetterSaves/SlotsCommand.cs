using Blasphemous.CheatConsole;
using Framework.Managers;
using System;
using System.Collections.Generic;

namespace Blasphemous.BetterSaves;

/// <summary>
/// Provides functionality for naming slots
/// </summary>
public class SlotsCommand : ModCommand
{
    protected override string CommandName { get; } = "slots";

    protected override bool AllowUppercase { get; } = false;

    protected override Dictionary<string, Action<string[]>> AddSubCommands()
    {
        return new Dictionary<string, Action<string[]>>()
        {
            { "help", Help },
            { "name", Name },
        };
    }

    private void Help(string[] parameters)
    {
        if (!ValidateParameterList(parameters, 0))
            return;

        Write("Available SLOTS commands:");
        Write("slots name NAME: Sets the name of this save file");
    }

    private void Name(string[] parameters)
    {
        if (!ValidateParameterList(parameters, 1))
            return;

        if (!ValidateStringParameter(parameters[0], 1, 32))
            return;

        if (PersistentManager.GetAutomaticSlot() == -1)
        {
            Write("No save file is currently loaded!");
            return;
        }

        string result = Main.BetterSaves.NameHandler.TrySetSaveName(parameters[0])
            ? $"Named this save file '{parameters[0]}'"
            : "This save file already has a name!";
        Write(result);
    }
}
