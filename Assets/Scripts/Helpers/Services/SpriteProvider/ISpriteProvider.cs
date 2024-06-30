using System.Threading.Tasks;
using UnityEngine;

public interface ISpriteProvider
{
    public Task<Sprite> Load(string id);
}