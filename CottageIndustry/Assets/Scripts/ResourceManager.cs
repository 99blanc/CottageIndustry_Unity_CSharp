using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceManager
{
    public Dictionary<string, Image> Images { get; private set; }
    public Dictionary<string, GameObject> Prefabs { get; private set; }

    public void Init()
    {
        Images = new Dictionary<string, Image>();
        Prefabs = new Dictionary<string, GameObject>();
    }

    private T Load<T>(Dictionary<string, T> dictionary, string path) where T : Object
    {
        if (!dictionary.ContainsKey(path))
        {
            T resource = Resources.Load<T>(path);
            dictionary.Add(path, resource);

            return dictionary[path];
        }

        return dictionary[path];
    }

    public Image LoadImage(string path) => Load(Images, string.Concat(Define.Path.IMAGE, path));
    public GameObject LoadPrefab(string path) => Load(Prefabs, string.Concat(Define.Path.PREFAB, path));

    public GameObject Instantiate(string path, Transform parent = null)
    {
        GameObject prefab = LoadPrefab(path);

        return Instantiate(prefab, parent);
    }

    public GameObject Instantiate(GameObject prefab, Transform parent = null)
    {
        GameObject gameObject = Object.Instantiate(prefab, parent);
        gameObject.name = prefab.name;

        return gameObject;
    }

    public void Destroy(GameObject gameObject)
    {
        if (gameObject)
            Destroy(gameObject);
    }
}
