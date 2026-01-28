public class Define
{
    public const string ROOT = "@Root";
    public const string USER = "user";
    public const string CONFIG = ".config";
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
        public const string FILE_PLAYER = "Player.csv";
        public const string PREFAB_PLAYER = "Character/Player.prefab";
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

public enum Language
{
    NULL,
    KOREAN = 1
}

public enum PlayerID
{
    NULL,
    DEFAULT
}

public enum ItemCategory
{
    NULL,
    EQUIPMENT,
    CONSUMPTION,
    MISC,
    QUEST,
    EVENT
}

public enum EquipmentPart
{
    NULL,
    WEAPON,
    HAT,
    TOP,
    BOTTOM,
    SHOES
}

public enum WeaponCategory
{
    NULL,
    GREAT_SWORD,
    DAGGER,
    BLUNT,
    BOW,
    THROW
}

public enum ViewEvent
{
    NULL,
    ENTER,
    EXIT,
    LEFT_CLICK,
    RIGHT_CLICK,
    LEFT_DOUBLE_CLICK
}

public enum StatType
{
    NULL,
    CURRENT_HEALTH,
    MAX_HEALTH,
    TEMP_HEALTH,
    MOVE_SPEED,
    DAMAGE,
    ATK_SPEED,
    ATK_RANGE,
    ATK_INTERVAL,
    CHARGE_MULTIPLE,
    PIERCE_COUNT,
    DASH_COUNT,
    DASH_COOLTIME,
    DASH_DISTANCE,
    JUMP_COUNT,
    JUMP_FORCE,
    GV_REDUCTION,
    INVUL_DURATION,
    WEAPON_CATEGORY,
}
