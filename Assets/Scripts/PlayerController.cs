using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private SplineContainer _container;
    [SerializeField] private int _currentIndex = 5;

    void OnValidate()
    {
        if(_container == null) _container = GetComponent<SplineContainer>();
    }


    // Test
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            
            _container.Spline[_container.Spline.Count - 1] += new float3(0, 0, 1) * Time.deltaTime;
        }

        // if (Input.GetKey(KeyCode.A))
        // {
        //     float3 knotPos = _container.Spline[_container.Spline.Count - 1].Position;
        //     _container.Spline.Add(new float3(knotPos.x, knotPos.y + 1 * Time.deltaTime, knotPos.z));
        // }
    }
}
