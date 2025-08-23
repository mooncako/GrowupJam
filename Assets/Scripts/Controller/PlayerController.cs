using System.Collections;
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
    [SerializeField, BoxGroup("Settings")] public Stats Stats;
    [SerializeField, BoxGroup("Settings")] private int _sunlightSpotAmount = 0;
    [SerializeField, BoxGroup("Settings")] private AnimationCurve _recoveryRateIncrementCurve;
    [SerializeField, BoxGroup("Debug"), ReadOnly] private bool _canMove = false;
    [SerializeField, BoxGroup("Debug"), ReadOnly] private Vector2 _moveInput;
    [SerializeField, BoxGroup("Debug"), ReadOnly] private Vector3 _moveDir;

    private WaitForSeconds _waitForSeconds = new WaitForSeconds(1);

    private Vector3 _camForward;
    private Vector3 _camRight;

    private void OnValidate()
    {
        if (_rb == null) _rb = GetComponent<Rigidbody>();
        if (_input == null) _input = GetComponent<PlayerInput>();
    }

    private void Start()
    {
        SetupEnergy();
    }

    private void OnEnable()
    {
        EventHub.Instance.OnGameStart.AddListener(OnGameStart);
    }

    private void OnDisable()
    {
        EventHub.Instance.OnGameStart.RemoveListener(OnGameStart);
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        if (!IsOwner)
        {
            _input.enabled = false;
        }

        EventHub.Instance.OnPlayerJoined.Invoke(this);

        SplineController vine = Instantiate(_vinePrefab);
        vine.VinePlayer = transform;
    }

    void Update()
    {
        if (!_canMove) return;

        SetupCamVector();

        if (Stats.Energy > 0)
        {
            transform.position += _moveDir * _speed * Time.deltaTime;
            _rb.position = transform.position;
        }

        if (_moveInput != Vector2.zero)
        {
            Stats.Energy -= Time.deltaTime;
        }

        Mathf.Clamp(Stats.Energy, 0, Stats.MaxEnergy);
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

    private void SetupEnergy()
    {
        Stats.Energy = Stats.MaxEnergy;
        Stats.EnergyRecoverRate = 0;
    }

    public void OnReceiveSunlight()
    {
        _sunlightSpotAmount++;
        Stats.EnergyRecoverRate += _recoveryRateIncrementCurve.Evaluate(_sunlightSpotAmount);
    }

    [Button]
    private void OnGameStart()
    {
        StartCoroutine(EnergyGainCO());
        SetupEnergy();
        _canMove = true;
    }

    private IEnumerator EnergyGainCO()
    {
        while (true)
        {
            yield return _waitForSeconds;
            if (Stats.Energy < Stats.MaxEnergy)
                Stats.Energy += Stats.EnergyRecoverRate;
        }
    }

    public int GetInputId()
    {
        return _input.user.index;
    }
}
