﻿using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "SO/LevelData", order = 0)]
public class LevelSpawnData : ScriptableObject
{
    [field: SerializeField] public uint InitialSpawnNumber { get; private set; }
    [field: SerializeField] public uint MaxSpawnNumber { get; private set; }
    [field: SerializeField] public uint MaxInstanceScale { get; private set; }
    public uint MinRangeBetweenObjects => MaxInstanceScale * 2;
}