using Cysharp.Text;
using Cysharp.Threading.Tasks;
using MemoryPack;
using UnityEngine;
using UnityEngine.InputSystem;
using System.IO;

public class ConfigManager
{
    private readonly string SAVE_PATH = Path.Combine(Application.persistentDataPath, ZString.Concat(Define.USER, Define.CONFIG));
    private readonly string TEMP_PATH = Path.Combine(Application.persistentDataPath, ZString.Concat(Define.USER, Define.TEMP));
    public Config curConfig { get; private set; }
    public InputActionAsset actAsset { get; private set; }
    public InputActionMap actMap { get; private set; }

    public async UniTask Init()
    {
        curConfig = await GetConfig() ?? new Config();
        actAsset = await Utils.SetupInputAsset();

        if (!actAsset)
            return;

        if (!string.IsNullOrEmpty(curConfig.control.keybind))
            actAsset.LoadBindingOverridesFromJson(curConfig.control.keybind);

        actMap = actAsset.FindActionMap(Define.Input.MAP_USER);
    }

    public async UniTask<Config> GetConfig()
    {
        if (!File.Exists(SAVE_PATH) && File.Exists(TEMP_PATH))
            File.Move(TEMP_PATH, SAVE_PATH);

        if (!File.Exists(SAVE_PATH))
            return null;

        try
        {
            byte[] data = await File.ReadAllBytesAsync(SAVE_PATH);
            return MemoryPackSerializer.Deserialize<Config>(data);
        }
        catch (System.Exception)
        {
            if (File.Exists(SAVE_PATH))
                File.Delete(SAVE_PATH);

            return null;
        }
    }

    public async UniTask SetConfig(Config config)
    {
        byte[] data = MemoryPackSerializer.Serialize(config);

        try
        {
            await File.WriteAllBytesAsync(TEMP_PATH, data);

            if (File.Exists(SAVE_PATH))
                File.Replace(TEMP_PATH, SAVE_PATH, null);
            else
                File.Move(TEMP_PATH, SAVE_PATH);

            curConfig = config;
        }
        catch (System.Exception)
        {
            if (File.Exists(TEMP_PATH))
                File.Delete(TEMP_PATH);
        }
    }

    public void OnDestroy(InputActionAsset asset = null)
    {
        if (!asset)
            asset = actAsset;

        if (asset)
        {
            asset.Disable();
            Object.Destroy(asset);
        }
    }
}