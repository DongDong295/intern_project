using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class DataManager
{
    public Dictionary<string, ScriptableObject> Data;
    
    public void Initiate()
    {
        Debug.Log("Initated");
        Data = new ();
    }

    public async UniTask<T> Load<T>(string assetName) where T : ScriptableObject
    {
        if (Data.TryGetValue(assetName, out ScriptableObject data))
        {
            Debug.Log("Value exist");
            return data as T;
        }
        Debug.Log("Value not exist");   
        return await Preload<T>(assetName);
    }

    public async UniTask<T> Preload<T>(string assetName) where T : ScriptableObject
    {
        T data = await Addressables.LoadAssetAsync<T>(assetName);

        Data.Replace(assetName, data);

        return data;
    }
}

public static class DictionaryExtensions
{
    public static void Replace<TKey, TValue>(this Dictionary<TKey, TValue> original, TKey key, TValue value)
    {
        if (original.ContainsKey(key))
        {
            original[key] = value;
        }
        else
        {
            original.Add(key, value);
        }
    }
}