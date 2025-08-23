using System;
using Sirenix.OdinInspector;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;

public class SplineController : MonoBehaviour
{
    [SerializeField, BoxGroup("References")] private SplineContainer _container;
    [SerializeField, BoxGroup("References")] public Transform VineEnd;
    [SerializeField, BoxGroup("Settings")] private float _distanceBeforeSplit = 2;
    [SerializeField, ReadOnly] private float3 _lastKnotSpawned = float3.zero;
    [SerializeField, ReadOnly] private float3 _vineEndPos => VineEnd != null ? VineEnd.position : Vector3.zero;

    void OnValidate()
    {
        if (_container == null) _container = GetComponent<SplineContainer>();
    }

    void Update()
    {
        BezierKnot knot = _container.Spline[^1];
        knot.Position = new float3(_vineEndPos.x, _vineEndPos.y, _vineEndPos.z);
        if (IsKnotTooFarApart(knot.Position))
        {
            _container.Spline.Add(knot);
            _lastKnotSpawned = knot.Position;
        }
        else
        {
            _container.Spline[^1] = knot;
        }
        
    }

    private bool IsKnotTooFarApart(float3 position)
    {
        return math.distance(position, _lastKnotSpawned) >= _distanceBeforeSplit;
    }
}
