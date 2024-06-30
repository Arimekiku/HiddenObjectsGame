using UnityEngine;
using Zenject;

public class CameraTracker : IInitializable
{
    public Camera MainCamera { get; private set; }

    public void Initialize()
    {
        MainCamera = Camera.main;
    }
}