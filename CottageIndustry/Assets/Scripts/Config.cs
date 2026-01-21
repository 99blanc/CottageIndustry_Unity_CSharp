using MemoryPack;

[MemoryPackable]
public partial class Config
{
    public GameplayConfig Gameplay { get; set; } = new();
    public AudioConfig Audio { get; set; } = new();
    public ControlConfig Controls { get; set; } = new();
    public Language Language { get; set; } = Language.KOREAN;
}

[MemoryPackable]
public partial struct GameplayConfig
{
    public bool IsDashComboOnly { get; set; }
    public float ScreenShakeIntensity { get; set; }

    [MemoryPackIgnore]
    public static GameplayConfig Default => new()
    {
        IsDashComboOnly = false,
        ScreenShakeIntensity = 1.0f
    };
}

[MemoryPackable]
public partial struct AudioConfig
{
    public float MasterVolume { get; set; }
    public float BGMVolume { get; set; }
    public float SFXVolume { get; set; }

    [MemoryPackIgnore]
    public static AudioConfig Default => new()
    {
        MasterVolume = 1.0f,
        BGMVolume = 1.0f,
        SFXVolume = 1.0f
    };
}

[MemoryPackable]
public partial struct ControlConfig
{
    public string RawKeybindData { get; set; }
}
