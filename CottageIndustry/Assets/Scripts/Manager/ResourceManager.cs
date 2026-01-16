using Cysharp.Text;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class ResourceManager
{
    private Dictionary<string, AsyncOperationHandle> handles = new();

    public void Init() => handles.Clear();

    private async UniTask<T> Load<T>(string path) where T : Object
    {
        if (handles.TryGetValue(path, out AsyncOperationHandle handle))
            return handle.Convert<T>().Result;

        AsyncOperationHandle<T> asyncHandle = Addressables.LoadAssetAsync<T>(path);
        handles[path] = asyncHandle;

        return await asyncHandle.ToUniTask();
    }

    public async UniTask<Image> LoadImage(string path) => await Load<Image>(ZString.Concat(Define.Path.IMAGE, path));
    public async UniTask<GameObject> LoadPrefab(string path) => await Load<GameObject>(ZString.Concat(Define.Path.PREFAB, path));

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
        if (handles.TryGetValue(path, out AsyncOperationHandle handle))
        {
            Addressables.Release(handle);
            handles.Remove(path);
        }
    }

    public void Destroy(GameObject gameObject)
    {
        if (gameObject)
            Object.Destroy(gameObject);
    }
}
