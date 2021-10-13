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
    protected override void ModLoad(TagCompound tag)
    {
        var id = Player.GetModPlayer<WCPlayer>().UniqueID;
        ModContent.GetInstance<BindsManager>().LoadCfgs(id);
    }

    protected override void ModSave(TagCompound tag)
    {
        var id = Player.GetModPlayer<WCPlayer>().UniqueID;
        ModContent.GetInstance<BindsManager>().SaveCfgs(id);
    }

    private Guid GetUniqueId() => Player.GetModPlayer<WCPlayer>().UniqueID;

    // Fuckin' useless, can't figure out a way to trigger Load otherwise.
    [Save] public bool Flag { get; set; }
}