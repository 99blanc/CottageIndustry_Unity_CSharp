using CsvHelper;
using ZLinq;
using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;

public class DataManager
{
    public Dictionary<CharacterID, CharacterData> characters { get; private set; }
    
    public async UniTask Init()
    {
        TextAsset cTable = await Managers.Resource.LoadTextAsset(Define.Asset.FILE_CHARACTER);
        await UniTask.Yield(PlayerLoopTiming.Update);
        characters = ParseToDictionary<CharacterID, CharacterData>(cTable.text, data => data.id);
    }

    private List<T> ParseToList<T>(string text)
    {
        using StringReader reader = new StringReader(text);
        using CsvReader csv = new CsvReader(reader, CultureInfo.InvariantCulture);
        return csv.GetRecords<T>().AsValueEnumerable().ToList();
    }

    private Dictionary<TKey, TItem> ParseToDictionary<TKey, TItem>(string text, Func<TItem, TKey> key)
    {
        using StringReader reader = new StringReader(text);
        using CsvReader csv = new CsvReader(reader, CultureInfo.InvariantCulture);
        return csv.GetRecords<TItem>().AsValueEnumerable().ToDictionary(key);
    }
}
