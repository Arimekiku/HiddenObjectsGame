using System.Threading.Tasks;
using UnityEngine;

public interface ISpriteProvider
{
    public Task LoadSprites();
    public Sprite GetRandomSprite();
    public Sprite GetConcreteSprite(int spriteCode);
    public Sprite GetProducerSprite();
    public Sprite GetCoinSprite();
    public Sprite GetStarSprite();
}