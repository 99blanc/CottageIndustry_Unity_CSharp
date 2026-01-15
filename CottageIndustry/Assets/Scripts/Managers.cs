using UnityEngine;

public class Managers : MonoBehaviour
{
    private static Managers Instance;

    public static UIManager UI { get; private set; }

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        if (Instance)
        {
            Destroy(this);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(this);

        UI = new UIManager();

        UI.Init();
    }
}
