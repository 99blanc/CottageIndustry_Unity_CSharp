using Cysharp.Threading.Tasks;
using UnityEngine;

public class GameManager
{
    public Character<Component> character;

    public async UniTask Init()
    {
        GameObject gameObject = await Managers.Resource.Instantiate(Define.Asset.PREFAB_CHARACTER);
        character = gameObject.GetComponentAssert<Character<Component>>();
        character.Init();
    }
}
