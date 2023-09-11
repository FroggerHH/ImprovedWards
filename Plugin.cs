using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using BepInEx;
using BepInEx.Configuration;
using Extensions;
using HarmonyLib;
using ServerSync;
using static ImprovedWards.Const;

namespace ImprovedWards;

[BepInPlugin(ModGUID, ModName, ModVersion)]
internal class Plugin : BaseUnityPlugin
{
    private void Awake()
    {
        _self = this;
        configSync.AddLockingConfigEntry(config("General", "LockConfig", true, string.Empty));
        useRangeCircleConfig = config("General", "Use range circle", useRangeCircle, string.Empty);
        permanentRangeCircleConfig = config("General", "Permanent range circle", permanentRangeCircle, string.Empty);

        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), ModGUID);
    }

    #region values

    internal const string ModName = "ImprovedWards", ModVersion = "1.0.0", ModGUID = "com.Frogger." + ModName;
    internal static Plugin _self;

    #endregion


    #region tools

    public static void Debug(object msg)
    {
        _self.Logger.LogInfo(msg);
    }

    public static void DebugError(object msg, bool showWriteToDev = true)
    {
        if (showWriteToDev) msg += "Write to the developer and moderator if this happens often.";

        _self.Logger.LogError(msg);
    }

    public static void DebugWarning(object msg, bool showWriteToDev = false)
    {
        if (showWriteToDev) msg += "Write to the developer and moderator if this happens often.";

        _self.Logger.LogWarning(msg);
    }

    #endregion

    #region ConfigSettings

    #region tools

    private static readonly string ConfigFileName = $"com.Frogger.{ModName}.cfg";
    private DateTime LastConfigChange;

    public static readonly ConfigSync configSync = new(ModName)
        { DisplayName = ModName, CurrentVersion = ModVersion, MinimumRequiredVersion = ModVersion };

    public static ConfigEntry<T> config<T>(string group, string name, T value, ConfigDescription description,
        bool synchronizedSetting = true)
    {
        var configEntry = _self.Config.Bind(group, name, value, description);

        var syncedConfigEntry = configSync.AddConfigEntry(configEntry);
        syncedConfigEntry.SynchronizedConfig = synchronizedSetting;

        return configEntry;
    }

    private ConfigEntry<T> config<T>(string group, string name, T value, string description,
        bool synchronizedSetting = true)
    {
        return config(group, name, value, new ConfigDescription(description), synchronizedSetting);
    }

    private void SetCfgValue<T>(Action<T> setter, ConfigEntry<T> config)
    {
        setter(config.Value);
        config.SettingChanged += (_, _) => setter(config.Value);
    }

    public enum Toggle
    {
        On = 1,
        Off = 0
    }

    private void SetupWatcher()
    {
        FileSystemWatcher fileSystemWatcher = new(Paths.ConfigPath, ConfigFileName);
        fileSystemWatcher.Changed += ConfigChanged;
        fileSystemWatcher.IncludeSubdirectories = true;
        fileSystemWatcher.SynchronizingObject = ThreadingHelper.SynchronizingObject;
        fileSystemWatcher.EnableRaisingEvents = true;
    }

    private void ConfigChanged(object sender, FileSystemEventArgs e)
    {
        if ((DateTime.Now - LastConfigChange).TotalSeconds <= 5.0) return;

        LastConfigChange = DateTime.Now;
        try
        {
            Config.Reload();
        }
        catch
        {
            DebugError("Can't reload Config");
        }
    }

    internal void ConfigChanged()
    {
        ConfigChanged(null, null);
    }

    #endregion

    #region configs

    public static ConfigEntry<bool> permanentRangeCircleConfig;
    public static ConfigEntry<bool> useRangeCircleConfig;

    public static bool permanentRangeCircle = false;
    public static bool useRangeCircle = true;

    #endregion

    internal async Task UpdateConfiguration()
    {
        Task task = null;
        task = Task.Run(() =>
        {
            permanentRangeCircle = permanentRangeCircleConfig.Value;
            useRangeCircle = useRangeCircleConfig.Value;


            UpdateWards();
        });

        await task;
        Debug("Configuration Received");
    }

    private void UpdateWards()
    {
    }

    #endregion
}