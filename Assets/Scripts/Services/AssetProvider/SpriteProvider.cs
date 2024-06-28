using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class SpriteProvider : ISpriteProvider
{
    public async Task<Sprite> Load(string id)
    {
        AsyncOperationHandle<Sprite> handle = Addressables.LoadAssetAsync<Sprite>(id);
        Sprite localObject = await handle.Task;

        if (localObject == null)
            throw new Exception($"Can't process asset request on {id}");
        
        Addressables.Release(handle);
        return localObject;
    }
}