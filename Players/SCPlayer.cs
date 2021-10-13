using System;
using SourceConsole.Binds;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using WebmilioCommons.Players;
using WebmilioCommons.Saving;

namespace SourceConsole.Players;

public class SCPlayer : BetterModPlayer
{
    private readonly BindsManager _binds;

    public SCPlayer()
    {
        _binds = ModContent.GetInstance<BindsManager>();
    }

    public override void OnEnterWorld(Player player)
    {
        _binds.LoadCfgs(GetUniqueId());
    }
    
    protected override void ModSave(TagCompound tag) => _binds.SaveCfgs(GetUniqueId());

    private Guid GetUniqueId() => Player.GetModPlayer<WCPlayer>().UniqueID;
}