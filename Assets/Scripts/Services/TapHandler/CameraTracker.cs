using UnityEngine;

public class CameraTracker : MonoBehaviour
{
    public Camera MainCamera { get; private set; }

    private void Awake()
    {
        MainCamera = Camera.main;
    }
}