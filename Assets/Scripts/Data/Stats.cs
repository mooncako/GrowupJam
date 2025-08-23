using System;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable]
public class Stats
{
    [field: SerializeField, Range(100, 500)] public float MaxEnergy { get; private set; } = 100;
    public float Energy;
    public float EnergyRecoverRate = 0;
}
