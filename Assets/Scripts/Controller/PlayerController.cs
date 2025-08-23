using Sirenix.OdinInspector;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(PlayerInput))]
public class PlayerController : NetworkBehaviour
{
    [SerializeField, BoxGroup("References")] private Rigidbody _rb;
    [SerializeField, BoxGroup("References")] private PlayerInput _input;
    [SerializeField, BoxGroup("References")] private SplineController _vinePrefab;
    [SerializeField, BoxGroup("Settings")] private float _speed = .2f;
    [SerializeField, BoxGroup("Debug"), ReadOnly] private Vector2 _moveInput;
    [SerializeField, BoxGroup("Debug"), ReadOnly] private Vector3 _moveDir;

    private Vector3 _camForward;
    private Vector3 _camRight;

    private void OnValidate()
    {
        if (_rb == null) _rb = GetComponent<Rigidbody>();
        if (_input == null) _input = GetComponent<PlayerInput>();
    }

    private void Start()
    {
        SetupCamVector();
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        if (!IsOwner)
        {
            _input.enabled = false;
        }

        SplineController vine = Instantiate(_vinePrefab);
        vine.VineEnd = transform;
    }

    void Update()
    {
        transform.position += _moveDir * _speed * Time.deltaTime;
        _rb.position = transform.position;
    }

    public void OnMove(InputValue value)
    {
        if (!IsOwner) return;

        _moveInput = value.Get<Vector2>();

        _moveDir = _camForward * _moveInput.y + _camRight * _moveInput.x;
    }

    private void SetupCamVector()
    {
        _camForward = Camera.main.transform.up;
        _camRight = Camera.main.transform.right;
        _camRight.y = 0;
        _camForward.Normalize();
        _camRight.Normalize(); 
    }
}
