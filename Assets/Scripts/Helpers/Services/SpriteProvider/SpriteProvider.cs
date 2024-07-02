using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Random = UnityEngine.Random;

public class SpriteProvider : MonoBehaviour, ISpriteProvider
{
    [SerializeField] private List<AssetReferenceSprite> _hiddenObjectReferences;
    [SerializeField] private AssetReferenceSprite _producerReference;
    [SerializeField] private AssetReferenceSprite _coinReference;
    [SerializeField] private AssetReferenceSprite _starReference;
    
    private readonly List<Sprite> _loadSprites;
    private Sprite _starSprite;
    private Sprite _coinSprite;
    private Sprite _producerSprite;

    public SpriteProvider()
    {
        _loadSprites = new List<Sprite>();
    }
    
    public async Task LoadSprites()
    {
        foreach (AssetReferenceSprite reference in _hiddenObjectReferences)
        {
            Sprite sprite = await LoadSprite(reference);
            
            _loadSprites.Add(sprite);
        }

        _coinSprite = await LoadSprite(_coinReference);
        _starSprite = await LoadSprite(_starReference);
        _producerSprite = await LoadSprite(_producerReference);
    }

    public Sprite GetRandomSprite()
    {
        int index = Random.Range(0, _loadSprites.Count);

        return _loadSprites[index];
    }

    public Sprite GetConcreteSprite(int spriteCode)
    {
        Sprite loadSprite = _loadSprites.First(s => s.GetHashCode() == spriteCode);
        
        return loadSprite;
    }

    public Sprite GetProducerSprite()
    {
        return _producerSprite;
    }

    public Sprite GetCoinSprite()
    {
        return _coinSprite;
    }

    public Sprite GetStarSprite()
    {
        return _starSprite;
    }

    private async Task<Sprite> LoadSprite(AssetReferenceSprite spriteToLoad)
    {
        AsyncOperationHandle<Sprite> handle = Addressables.LoadAssetAsync<Sprite>(spriteToLoad);
        Sprite localObject = await handle.Task;

        if (localObject == null)
            throw new Exception($"Can't process asset request on {spriteToLoad}");
        
        Addressables.Release(handle);
        return localObject;
    }
}