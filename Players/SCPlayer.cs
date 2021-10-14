using SourceConsole.Binds;
using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using WebmilioCommons.Players;

namespace SourceConsole.Players;

public class SCPlayer : BetterModPlayer
{
    private readonly BindsManager _binds;

    public SCPlayer()
    {
        _binds = ModContent.GetInstance<BindsManager>();
    }

    public override void OnEnterWorld(Player player) => _binds.LoadPlayerCfg(UniqueId);
    protected override void ModSave(TagCompound tag) => _binds.SavePlayerCfg(UniqueId);

    private Guid UniqueId => Player.GetModPlayer<WCPlayer>().UniqueId;
}