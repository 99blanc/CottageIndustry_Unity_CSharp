using Mono.Cecil;
using UnityEngine;

public class Managers : MonoBehaviour
{
    private static Managers Instance;

    public static ResourceManager Resource { get; private set; }
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

        Resource = new ResourceManager();
        UI = new UIManager();
        Resource.Init();
        UI.Init();
    }
}
