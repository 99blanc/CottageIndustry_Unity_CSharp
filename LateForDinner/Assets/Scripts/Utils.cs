using Cysharp.Text;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

public static class Utils
{
    public static async UniTask<InputActionAsset> SetupInputAsset()
    {
        InputActionAsset original = await Managers.Resource.LoadInputSystem(Define.Asset.FILE_INPUT_SYSTEM);
        Debug.Assert(original);
        InputActionAsset copy = Object.Instantiate(original);
        Debug.Assert(copy);
        Config config = Managers.Config.curConfig;
        bool hasSavedData = !string.IsNullOrEmpty(config.control.keybind);
        string bindJson = hasSavedData ? config.control.keybind : copy.SaveBindingOverridesAsJson();
        copy.LoadBindingOverridesFromJson(bindJson);

        if (!hasSavedData)
        {
            config.control = new ControlConfig { keybind = bindJson };
            await Managers.Config.SaveProfile(config);
        }

        SyncMoveToDashBindings(copy);
        copy.Enable();
        return copy;
    }

    private static void SyncMoveToDashBindings(InputActionAsset asset)
    {
        InputActionMap map = asset.FindActionMap(Define.Input.MAP_USER);
        InputAction moveAction = map.FindAction(Define.Input.ACTION_MOVE);
        InputAction dashAction = map.FindAction(Define.Input.ACTION_DASH);

        if (moveAction is null || dashAction is null) 
            return;

        foreach (InputBinding moveBinding in moveAction.bindings)
        {
            if (moveBinding.isComposite) 
                continue;

            dashAction.ApplyBindingOverride(new InputBinding
            {
                name = moveBinding.name,
                overridePath = moveBinding.effectivePath
            });
        }
    }
}
