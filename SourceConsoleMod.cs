using Microsoft.Xna.Framework.Input;
using Terraria.ModLoader;
using WebmilioCommons.Inputs;

namespace SourceConsole
{
	public class SourceConsoleMod : Mod
	{
        public SourceConsoleMod()
        {
            Instance = this;
        }

        [Keybind("Show Console", Keys.F11)] public ModKeybind ShowConsole { get; set; }

		public static SourceConsoleMod Instance { get; private set; }
	}
}