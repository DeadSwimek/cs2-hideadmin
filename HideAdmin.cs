using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Utils;
using CounterStrikeSharp.API.Modules.Menu;
using CounterStrikeSharp.API.Core.Attributes;
using CounterStrikeSharp.API.Modules.Timers;
using System.ComponentModel;
using CounterStrikeSharp.API.Modules.Memory;
using CounterStrikeSharp.API.Modules.Entities;
using System.Drawing;
using System.Numerics;

namespace HideAdmin;
[MinimumApiVersion(100)]

public partial class HideAdmin : BasePlugin, IPluginConfig<ConfigHide>
{
    public override string ModuleName => "Hide";
    public override string ModuleAuthor => "DeadSwim";
    public override string ModuleDescription => "Admin hide in spectates.";
    public override string ModuleVersion => "V. 1.0.0";


    public ConfigHide Config { get; set; }


    public void OnConfigParsed(ConfigHide config)
    {
        Config = config;
    }

    public override void Load(bool hotReload)
    {
        AddCommandListener("jointeam", OnPlayerChangeTeam);
    }

    public HookResult OnPlayerChangeTeam(CCSPlayerController? player, CommandInfo command)
    {
        if (!Int32.TryParse(command.ArgByIndex(1), out int team_switch))
        {
            Server.PrintToConsole("Cannot get player team switch");
            return HookResult.Continue;
        }

        if (player == null || !player.IsValid)
            return HookResult.Continue;

        var player_team = team_switch;

        player.PlayerPawn.Value.CommitSuicide(true, false);

        if (player_team == 1)
        {
            if (AdminManager.PlayerHasPermissions(player, Config.permission))
            {
                player.ChangeTeam(CsTeam.None);
                player.PrintToChat($" {Config.Prefix} You are automatically hide!");
                return HookResult.Stop;
            }
        }

        return HookResult.Continue;
    }
    // In development
    //[ConsoleCommand("css_hide", "Hide you")]
    public void HideCommand(CCSPlayerController? player, CommandInfo command)
    {
        if (player == null || !player.IsValid)
            return;
        if (AdminManager.PlayerHasPermissions(player, Config.permission))
        {
            player.PlayerPawn.Value.CommitSuicide(true, false);
            player.ChangeTeam(CsTeam.Spectator);
            AddTimer(1.0f, () => { player.ChangeTeam(CsTeam.None); });
            return;
            player.PrintToChat($" {Config.Prefix} You are hided!");
        }
    }
}