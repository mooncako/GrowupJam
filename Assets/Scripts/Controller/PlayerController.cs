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
    [SerializeField, BoxGroup("Debug"), ReadOnly]
    private NetworkVariable<bool> _canMove = new(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    [SerializeField, BoxGroup("Debug"), ReadOnly]
    public NetworkVariable<float> Energy = new(0f, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    [SerializeField, BoxGroup("Debug"), ReadOnly] private Vector2 _moveInput;
    [SerializeField, BoxGroup("Debug"), ReadOnly] private Vector3 _moveDir;

    private WaitForSeconds _waitForSeconds = new WaitForSeconds(1);

    private Vector3 _camForward;
    private Vector3 _camRight;
    private bool _isReady;

    private Coroutine _energyCo;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        // Only the owner should read local devices
        _input.enabled = IsOwner;
        if(IsOwner)
            EventHub.Instance.OnPlayerJoined.Invoke(this);

        var vine = Instantiate(_vinePrefab);
        vine.VinePlayer = transform;
    }

    private void OnEnable()
    {
        EventHub.Instance.OnGameStart.AddListener(OnGameStart_ServerOnly);
        EventHub.Instance.OnPlayerVictory.AddListener(OnGameEnded);
    }

    private void OnDisable()
    {
        EventHub.Instance.OnGameStart.RemoveListener(OnGameStart_ServerOnly);
        EventHub.Instance.OnPlayerVictory.RemoveListener(OnGameEnded); // <-- fix
    }

    private void Start()
    {
        if(IsOwner)
            SetupEnergy();
        _energyCo = StartCoroutine(EnergyGainCO());
    }

    void Update()
    {
        if (!_canMove.Value) return;

        SetupCamVector();

        if (Energy.Value > 0)
        {
            // only compute dir here; move in FixedUpdate
            // (_moveDir is set in OnMove)
        }

        if (_moveInput != Vector2.zero && IsOwner)
        {
            Energy.Value = Mathf.Clamp(Energy.Value - Time.deltaTime, 0f, Stats.MaxEnergy);
        }


        // <-- actually clamp it
        
    }

    void FixedUpdate()
    {
        if (!_canMove.Value) return;
        if (Energy.Value <= 0) return;

        // Physics-friendly move
        var targetPos = _rb.position + _moveDir * _speed * Time.fixedDeltaTime;
        _rb.MovePosition(targetPos);
    }

    public void OnMove(InputValue value)
    {
        if (!IsOwner) return;

        _moveInput = value.Get<Vector2>();
        _moveDir = _camForward * _moveInput.y + _camRight * _moveInput.x;
    }

    public void OnReady(InputValue value)
    {
        if (_isReady) return;

        _isReady = true;
        if (IsServer) // host or dedicated server
            EventHub.Instance.OnPlayerReady.Invoke();
        else
            NotifyReadyServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    private void NotifyReadyServerRpc()
    {
        EventHub.Instance.OnPlayerReady.Invoke();
    }

    private void SetupCamVector()
    {
        var cam = Camera.main;
        if (cam == null) return;

        _camForward = cam.transform.up;
        _camRight   = cam.transform.right;
        _camRight.y = 0;
        _camForward.Normalize();
        _camRight.Normalize();
    }

    private void SetupEnergy()
    {
        Energy.Value = Stats.MaxEnergy; 
        Stats.EnergyRecoverRate = 0;
    }


    [Button]
    public void OnReceiveSunlight()
    {
        _sunlightSpotAmount++;
        Stats.EnergyRecoverRate = _recoveryRateIncrementCurve.Evaluate(_sunlightSpotAmount);
        Energy.Value += 5;
    }

    // --- Gate movement from the server only ---
    private void OnGameStart_ServerOnly()
    {
        if (IsServer) _canMove.Value = true; // server is the only writer
    }

    private void OnGameEnded(PlayerController player)
    {
        if (IsServer) _canMove.Value = false; // server writes
        if (_energyCo != null) { StopCoroutine(_energyCo); _energyCo = null; } // <-- fix
    }

    private IEnumerator EnergyGainCO()
    {
        while (true)
        {
            yield return _waitForSeconds;
            if (!IsOwner) continue;
            if (Energy.Value < Stats.MaxEnergy)
                Energy.Value += Stats.EnergyRecoverRate;
        }
    }

    public int GetInputId() => _input.user.index;
}
