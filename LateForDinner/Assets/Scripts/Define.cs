public enum Language
{
    KOREAN
}

public enum CharacterID
{
    DEFAULT = 1
}

public enum ItemCategory
{
    EQUIPMENT = 1,
    CONSUMPTION,
    MISC,
    QUEST,
    EVENT
}

public enum EquipmentPart
{
    WEAPON = 1,
    HAT,
    TOP,
    BOTTOM,
    SHOES
}

public enum WeaponCategory
{
    NULL = 0,
    GREAT_SWORD,
    DAGGER,
    BLUNT,
    BOW,
    THROW
}

public enum ViewEvent
{
    ENTER,
    EXIT,
    LEFT_CLICK,
    RIGHT_CLICK,
    LEFT_DOUBLE_CLICK
}

public class Define
{
    public const string ROOT = "@Root";
    public const string PROFILE = "UserConfig.profile";
    public const string TEMP = ".tmp";

    public class Path
    {
        public const string SYSTEM = "Assets/Systems/";
        public const string SPRITE = "Assets/Sprites/";
        public const string PREFAB = "Assets/Prefabs/";
        public const string TABLE = "Assets/Tables/";
    }

    public class Asset
    {
        public const string FILE_INPUT_SYSTEM = "InputSystem_Actions.inputactions";
        public const string FILE_CHARACTER = "Character.csv";
        public const string PREFAB_CHARACTER = "Character/Base.prefab";
    }

    public class Input
    {
        public const string MAP_USER = "User";
        public const string MAP_UI = "UI";
        public const string ACTION_MOVE = "Move";
        public const string ACTION_JUMP = "Jump";
        public const string ACTION_DASH = "Dash";
        public const string ACTION_ATTACK = "Attack";
    }

    public class Layer
    {
        public const string GROUND = "Ground";
    }
}
