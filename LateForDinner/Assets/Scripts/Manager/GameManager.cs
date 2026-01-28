using Cysharp.Threading.Tasks;
using UnityEngine;

public class GameManager
{
    public Character<Component> character;

    public async UniTask Init()
    {
        var gameObject = await Managers.Resource.Instantiate(Define.Asset.PREFAB_PLAYER);
        Player player = gameObject.GetComponentAssert<Player>();

        if (Managers.Data.players.TryGetValue(PlayerID.DEFAULT, out var data))
            player.Init(data);

    }
}
