using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Steamworks;
using Terraria;
using Terraria.ModLoader;

namespace SourceConsole.Points.Commands;

public class InviteCommand : Command
{
    private const string InviteSuccess = "Invited `{0}` to the game!";

    public InviteCommand() : base("invite", CommandLocation.Client)
    {
    }

    protected override void ActionLocal(CommandCaller caller, Player player, string input, string[] args)
    {
        if (args.Length == 0)
        {
            PrintNoArguments();
            return;
        }

        if (ulong.TryParse(args[0], out var steamId))
            InviteFromId(steamId);
        else
            InviteFromName(string.Join(" ", args));
    }

    private void InviteFromId(ulong steamId)
    {
        CSteamID id = new(steamId);
        

        if (!SteamMatchmaking.InviteUserToLobby(new CSteamID(Main.LobbyId), id))
        {
            Error($"Failed to invite Steam ID {steamId}.");
        }
        else
        {
            Success(string.Format(InviteSuccess, SteamFriends.GetFriendPersonaName(id)));
        }
    }

    private void InviteFromName(string name)
    {
        var friendFlags = EFriendFlags.k_EFriendFlagAll;
        var friendCount = SteamFriends.GetFriendCount(friendFlags);
        List<CachedFriend> cached = new(friendCount);

        CachedFriend toInvite = default;

        for (int i = 0; i < friendCount && toInvite == null; i++)
        {
            var friendId = SteamFriends.GetFriendByIndex(i, friendFlags);
            var friendName = SteamFriends.GetFriendPersonaName(friendId);

            if (friendName.Equals(name, StringComparison.CurrentCultureIgnoreCase))
            {
                toInvite = new(friendId, friendName);
            }
            else
            {
                cached.Add(new(friendId, friendName));
            }
        }

        CachedFriend Find(Func<CachedFriend, Func<string, StringComparison, bool>> comparer)
        {
            for (int i = 0; i < cached.Count; i++)
            {
                if (comparer(cached[i])(name, StringComparison.OrdinalIgnoreCase))
                    return cached[i];
            }

            return null;
        }

        toInvite ??= Find(friend => friend.PersonaName.StartsWith);
        toInvite ??= Find(friend => friend.PersonaName.Contains);

        if (toInvite == null)
        {
            Main.NewText($"Could not find anyone by the name (or containing) `{name}`.", Color.DarkRed);
            return;
        }

        Success($"Invited `{toInvite.PersonaName}` to the game!");
        SteamMatchmaking.InviteUserToLobby(new CSteamID(Main.LobbyId), toInvite.SteamId);
    }

    private record CachedFriend(CSteamID SteamId, string PersonaName);

    public override string Usage { get; } = "invite <name>";
    public override string Description { get; } = "Sends an invite to the specified person.";
}