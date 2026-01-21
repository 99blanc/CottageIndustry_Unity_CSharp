using Cysharp.Threading.Tasks;
using UnityEngine;

public class Managers : MonoBehaviour
{
    private static Managers Instance;

    public static ResourceManager Resource { get; private set; }
    public static DataManager Data { get; private set; }
    public static ConfigManager Config { get; private set; }
    public static GameManager Game { get; private set; }
    public static UIManager UI { get; private set; }

    private void Awake() => Init();

    private void Init()
    {
        if (Instance)
        {
            Destroy(this);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(this);
        TaskInit().Forget();
    }

    private async UniTaskVoid TaskInit()
    {
        Resource = new();
        Data = new();
        Config = new();
        Game = new();
        UI = new();
        Resource.Init();
        await UniTask.WhenAll(Data.Init(), Config.Init());
        await Game.Init();
        UI.Init();
    }
}