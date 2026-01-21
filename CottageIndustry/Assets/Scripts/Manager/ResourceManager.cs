using Cysharp.Text;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.InputSystem;

public class ResourceManager
{
    public Dictionary<string, AsyncOperationHandle> Handle = new();

    public void Init() => Handle.Clear();

    private async UniTask<T> Load<T>(string path) where T : Object
    {
        if (Handle.TryGetValue(path, out AsyncOperationHandle handle))
        {
            if (handle.IsDone)
                return handle.Convert<T>().Result;

            await handle.ToUniTask();
            return handle.Convert<T>().Result;
        }

        AsyncOperationHandle<T> asyncHandle = Addressables.LoadAssetAsync<T>(path);
        Handle[path] = asyncHandle;
        return await asyncHandle.ToUniTask();
    }

    public async UniTask<Image> LoadSprite(string path) => await Load<Image>(ZString.Concat(Define.Path.SPRITE, path));
    public async UniTask<GameObject> LoadPrefab(string path) => await Load<GameObject>(ZString.Concat(Define.Path.PREFAB, path));
    public async UniTask<InputActionAsset> LoadInputSystem(string path) => await Load<InputActionAsset>(ZString.Concat(Define.Path.SYSTEM, path));
    public async UniTask<TextAsset> LoadTextAsset(string path)  => await Load<TextAsset>(ZString.Concat(Define.Path.TABLE, path));


    public async UniTask<GameObject> Instantiate(string path, Transform parent = null)
    {
        GameObject prefab = await LoadPrefab(path);
        Debug.Assert(prefab);
        return Instantiate(prefab, parent);
    }

    public GameObject Instantiate(GameObject prefab, Transform parent = null)
    {
        GameObject gameObject = Object.Instantiate(prefab, parent);
        gameObject.name = prefab.name;
        return gameObject;
    }

    public void Unload(string path)
    {
        if (Handle.TryGetValue(path, out AsyncOperationHandle handle))
        {
            Addressables.Release(handle);
            Handle.Remove(path);
        }
    }

    public void Destroy(GameObject gameObject)
    {
        if (gameObject)
            Object.Destroy(gameObject);
    }
}
