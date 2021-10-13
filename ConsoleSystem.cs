using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using SourceConsole.Points.CVars;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;
using WebmilioCommons.UI;
using Console = SourceConsole.UI.Console;

namespace SourceConsole;

public class ConsoleSystem : ModSystem
{
    private UIBundle<Console, StandardUILayer> _consoleBundle;

    public override void Load()
    {
        if (Main.netMode != NetmodeID.Server)
            _consoleBundle = new(InterfaceScaleType.UI, ConsoleLayerIndexProvider);
    }

    public override void PostUpdateInput()
    {
        if (SourceConsoleMod.Instance.ShowConsole.JustPressed)
        {
            _consoleBundle.UIState.BuildInterface();
            _consoleBundle.UIState.Visible = !_consoleBundle.UIState.Visible;
        }
    }

    public override void UpdateUI(GameTime gameTime)
    {
        _consoleBundle.Update(gameTime);
    }

    public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
    {
        _consoleBundle.ModifyInterfaceLayers(layers);
    }

    private int ConsoleLayerIndexProvider(List<GameInterfaceLayer> layers)
    {
        return layers.FindIndex(l => l.Name.Equals("Vanilla: Cursor", StringComparison.OrdinalIgnoreCase));
    }
}