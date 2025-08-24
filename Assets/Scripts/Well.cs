using UnityEngine;

public class Well : MonoBehaviour
{
    public Transform Player;
    public Renderer Renderer; // use MPB if many instances

    static readonly int PlayerWS_ID = Shader.PropertyToID("_PlayerWS");
    static readonly int CutoutPos_ID = Shader.PropertyToID("_CutoutPosition");


    void OnValidate()
    {
        if (Renderer == null) Renderer = GetComponent<Renderer>();
    }

    void OnEnable()
    {
        EventHub.Instance.OnPlayerJoined.AddListener(OnPlayerJoined);
    }

    void OnDisable()
    {
        EventHub.Instance.OnPlayerJoined.RemoveListener(OnPlayerJoined);
    }

    void LateUpdate()
    {
        if (Player == null) return;
        // Center of the hole in viewport coords (0..1)
        var vp = Camera.main.WorldToViewportPoint(Player.position);
        var cutout = new Vector2(vp.x, vp.y);

        // Push to material (MaterialPropertyBlock recommended)
        var mpb = new MaterialPropertyBlock();
        Renderer.GetPropertyBlock(mpb);
        mpb.SetVector(PlayerWS_ID, Player.position);
        mpb.SetVector(CutoutPos_ID, cutout);
        Renderer.SetPropertyBlock(mpb);
    }

    private void OnPlayerJoined(PlayerController player)
    {
        if (player.IsOwner)
        {
            Player = player.transform;
        }
    }

}
