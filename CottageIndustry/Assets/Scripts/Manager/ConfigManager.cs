using Cysharp.Text;
using Cysharp.Threading.Tasks;
using MemoryPack;
using UnityEngine;
using UnityEngine.InputSystem;

public class ConfigManager
{
    private readonly string SAVE_PATH = System.IO.Path.Combine(Application.persistentDataPath, ZString.Concat(Define.PROFILE));
    public Config Current { get; private set; }
    public InputActionAsset ActAsset { get; private set; }
    public InputActionMap ActMap { get; private set; }

    public async UniTask Init()
    {
        Current = await GetProfile() ?? new Config();
        ActAsset = await Utils.SetupInputAsset();

        if (!ActAsset)
            return;

        if (!string.IsNullOrEmpty(Current.Controls.RawKeybindData))
            ActAsset.LoadBindingOverridesFromJson(Current.Controls.RawKeybindData);

        ActMap = ActAsset.FindActionMap(Define.Input.MAP_USER);
    }

    public async UniTask<Config> GetProfile()
    {
        if (!System.IO.File.Exists(SAVE_PATH))
            return null;

        try
        {
            byte[] data = await System.IO.File.ReadAllBytesAsync(SAVE_PATH);
            return MemoryPackSerializer.Deserialize<Config>(data);
        }
        catch (System.Exception)
        {
            if (System.IO.File.Exists(SAVE_PATH))
                System.IO.File.Delete(SAVE_PATH);

            return null;
        }
    }

    public async UniTask SaveProfile(Config config)
    {
        byte[] data = MemoryPackSerializer.Serialize(config);
        string path = ZString.Concat(SAVE_PATH, Define.TEMP);

        try
        {
            await System.IO.File.WriteAllBytesAsync(path, data);

            if (System.IO.File.Exists(SAVE_PATH))
                System.IO.File.Replace(path, SAVE_PATH, null);
            else
                System.IO.File.Move(path, SAVE_PATH);

            Current = config;
        }
        catch (System.Exception)
        {
            if (System.IO.File.Exists(path))
                System.IO.File.Delete(path);
        }
    }

    public void OnDestroy(InputActionAsset asset = null)
    {
        if (!asset)
            asset = ActAsset;

        if (asset)
        {
            asset.Disable();
            Object.Destroy(asset);
        }
    }
}