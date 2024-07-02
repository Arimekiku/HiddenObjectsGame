﻿using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "SO/LevelData", order = 0)]
public class LevelData : ScriptableObject
{
    [field: SerializeField] public uint MaxSpawnRadius { get; private set; }
    [field: SerializeField] public uint MaxSpawnNumber { get; private set; }
    [field: SerializeField] public uint InitialSpawnNumber { get; private set; }
    [field: SerializeField] public uint ObjectProducersNumber { get; private set; }
    [field: SerializeField] public Color CameraColor { get; private set; }
}