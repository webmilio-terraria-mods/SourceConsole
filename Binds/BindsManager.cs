using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;
using WebmilioCommons.Extensions;
using WebmilioCommons.Inputs;

namespace SourceConsole.Binds;

public class BindsManager : ModSystem
{
    private const string GlobalCfg = "binds.cfg";
    private const string PlayerCfg = "binds.{0}.cfg";

    private Dictionary<string, BindType> _byName = new();
    private Dictionary<BindType, string> _byType = new();

    private Dictionary<Keys, BindGroup> _binds = new();
    private KeyboardManager _keyboard;

    public override bool IsLoadingEnabled(Mod mod)
    {
        return Main.netMode != NetmodeID.Server;
    }

    public override void Load()
    {
        _keyboard = ModContent.GetInstance<KeyboardManager>();
        _binds = new();
    }

    #region Load Files

    public void LoadCfgs(Guid playerId)
    {
        _binds.Clear();

        DirectoryInfo root = GetCfgRoot();

        LoadGlobalCfg(root);
        LoadPlayerCfg(root, playerId);
    }

    private void LoadGlobalCfg(DirectoryInfo root)
    {
        FileInfo globalCfg = GetGlobalFile(root);

        if (globalCfg.Exists)
        {
            LoadCfg(globalCfg);
        }
        else
        {
            globalCfg.Create().Dispose();
        }
    }

    private void LoadPlayerCfg(DirectoryInfo root, Guid playerId)
    {
        FileInfo playerCfg = GetPlayerFile(root, playerId);

        if (playerCfg.Exists)
        {
            LoadCfg(playerCfg);
        }
        else
        {
            playerCfg.Create().Dispose();
        }
    }

    private void LoadCfg(FileInfo cfg)
    {
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

    public void SaveCfgs(Guid playerId)
    {
        DirectoryInfo root = GetCfgRoot();

        SaveGlobalCfg(root);
        SavePlayerCfg(root, playerId);
    }

    private void SaveGlobalCfg(DirectoryInfo root) => SaveCfg(GetGlobalFile(root), BindType.Global);
    private void SavePlayerCfg(DirectoryInfo root, Guid playerId) => SaveCfg(GetPlayerFile(root, playerId), BindType.Player);

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

    private static DirectoryInfo GetCfgRoot() => new DirectoryInfo(ConfigManager.ModConfigPath).CreateSubdirectory("cfg");
    private static FileInfo GetGlobalFile(DirectoryInfo root) => new(Path.Combine(root.ToString(), GlobalCfg));
    private static FileInfo GetPlayerFile(DirectoryInfo root, Guid playerId) => 
        new(Path.Combine(root.ToString(), string.Format(PlayerCfg, playerId.ToString("N"))));

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