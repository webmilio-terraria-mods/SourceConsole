using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;
using Terraria.ModLoader.IO;
using WebmilioCommons.Extensions;
using WebmilioCommons.Inputs;
using WebmilioCommons.Worlds;

namespace SourceConsole.Binds;

public class BindsManager : ModSystem
{
    private const string GlobalCfg = "binds.cfg";
    private const string WorldCfg = "binds.world-{0}.cfg";
    private const string PlayerCfg = "binds.player-{0}.cfg";

    private readonly Dictionary<string, BindType> _byName = new();
    private readonly Dictionary<BindType, string> _byType = new();

    private readonly Dictionary<Keys, BindGroup> _binds = new();
    private KeyboardManager _keyboard;

    public override bool IsLoadingEnabled(Mod mod)
    {
        return Main.netMode != NetmodeID.Server;
    }

    public override void Load()
    {
        _keyboard = ModContent.GetInstance<KeyboardManager>();
    }

    public override void OnWorldLoad()
    {
        LoadGlobalCfg();
        LoadWorldCfg(ModContent.GetInstance<WCWorldSystem>().UniqueId);
    }

    public override void OnWorldUnload()
    {
        SaveGlobalCfg();
        SaveWorldCfg(ModContent.GetInstance<WCWorldSystem>().UniqueId);
    }

    #region Load Files

    public void LoadGlobalCfg()
    {
        _binds.Clear();
        LoadCfg(GlobalFile);
    }

    public void LoadWorldCfg(Guid worldId) => LoadCfg(GetWorldFile(worldId));
    public void LoadPlayerCfg(Guid playerId) => LoadCfg(GetPlayerFile(playerId));

    private void LoadCfg(FileInfo cfg)
    {
        if (!cfg.Exists)
        {
            cfg.Create().Dispose();
            return;
        }

        using var fs = cfg.OpenRead();
        using StreamReader sr = new(fs);

        LoadCfg(sr);
    }

    private void LoadCfg(StreamReader reader)
    {
        while (!reader.EndOfStream)
        {
            string line = reader.ReadLine();
            var split = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            if (
                !_byName.TryGetValue(split[0], out var bindType) ||
                !Enum.TryParse(split[1], out Keys key))
            {
                continue;
            }

            var bindSentence = line[(split[0].Length + split[1].Length + 2)..];
            Set(key, bindType, bindSentence);
        }
    }

    #endregion

    #region Save Files

    public void SaveGlobalCfg() => SaveCfg(GlobalFile, BindType.Global);
    public void SaveWorldCfg(Guid worldId) => SaveCfg(GetWorldFile(worldId), BindType.World);
    public void SavePlayerCfg(Guid playerId) => SaveCfg(GetPlayerFile(playerId), BindType.Player);

    private void SaveCfg(FileInfo cfg, BindType bindType)
    {
        if (cfg.Exists)
        {
            using var fs = cfg.OpenWrite();
            using StreamWriter sw = new(fs);

            SaveCfg(sw, bindType);
        }
        else
        {
            using var fs = cfg.Create();
            using StreamWriter sw = new(fs);

            SaveCfg(sw, bindType);
        }
    }

    private void SaveCfg(StreamWriter writer, BindType type)
    {
        var commandName = _byType[type];

        foreach ((Keys key, BindGroup bind) in _binds)
        {
            var bindSentence = bind.Get(type);

            if (string.IsNullOrWhiteSpace(bindSentence))
            {
                continue;
            }

            writer.WriteLine($"{commandName} {key} {bindSentence}");
        }
    }

    #endregion

    public FileInfo GetWorldFile(Guid worldId) => 
        new(Path.Combine(CfgRoot.ToString(), string.Format(WorldCfg, worldId.ToString("N"))));
    public FileInfo GetPlayerFile(Guid playerId) => 
        new(Path.Combine(CfgRoot.ToString(), string.Format(PlayerCfg, playerId.ToString("N"))));

    #region Execution

    public override void PostUpdateInput()
    {
        if (Main.blockInput || Main.LocalPlayer.mouseInterface)
        {
            return;
        }

        _keyboard.justPressed.Do(ExecuteBind);
    }

    private void ExecuteBind(Keys key)
    {
        if (!_binds.TryGetValue(key, out var bind))
        {
            return;
        }

        ExecuteBind(bind);
    }

    private void ExecuteBind(BindGroup bind)
    {
        void Execute(string str)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return;
            }

            var split = str.Split(';', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            split.Do(ExecuteCommand);
        }

        Execute(bind.global);
        Execute(bind.world);
        Execute(bind.player);
    }

    private void ExecuteCommand(string command)
    {
        ModContent.GetInstance<CommandHelper>().ExecuteCommand(command);
    }

    #endregion

    #region Adding/Removing

    public void Register(BindType type, string name)
    {
        _byName.Add(name, type);
        _byType.Add(type, name);
    }

    public void Set(Keys key, BindType type, string bindSentence)
    {
        if (!TryGet(key, out var bind))
        {
            _binds.Add(key, bind = new());
        }

        bind.Set(type, bindSentence);
    }

    public bool TryGet(Keys key, out BindGroup bind) => _binds.TryGetValue(key, out bind);

    public bool Remove(Keys key) => _binds.Remove(key);

    public bool Remove(Keys key, BindType type)
    {
        if (!TryGet(key, out var bind))
        {
            return false;
        }

        bind.Set(type, null);

        if (!bind.Any())
        {
            Remove(key);
        }

        return true;
    }

    #endregion

    public DirectoryInfo CfgRoot => new DirectoryInfo(ConfigManager.ModConfigPath).CreateSubdirectory("cfg");
    public FileInfo GlobalFile => new(Path.Combine(CfgRoot.ToString(), GlobalCfg));

    public class BindGroup
    {
        public string global;
        public string world;
        public string player;

        public void Set(BindType type, string value)
        {
            switch (type)
            {
                case BindType.Global:
                    global = value;
                    break;

                case BindType.World:
                    world = value;
                    break;

                case BindType.Player:
                    player = value;
                    break;
            }
        }

        public string Get(BindType type)
        {
            return type switch
            {
                BindType.Global => global,
                BindType.World => world,
                BindType.Player => player,
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
        }

        public bool Any() => !string.IsNullOrWhiteSpace(global) || !string.IsNullOrWhiteSpace(world) || !string.IsNullOrWhiteSpace(player);
    }
}