using System;
using Sirenix.OdinInspector;
using Unity.Netcode;
using UnityEngine;

[Serializable]
public class Stats
{
    [field: SerializeField, Range(50, 100)] public float MaxEnergy { get; private set; } = 100;
    public float EnergyRecoverRate = 0;
}
