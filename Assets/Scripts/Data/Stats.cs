using System;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable]
public class Stats
{
    [field: SerializeField, Range(50, 100)] public float MaxEnergy { get; private set; } = 100;
    public float Energy;
    public float EnergyRecoverRate = 0;
}
