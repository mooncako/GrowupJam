using UnityEngine;

public class Musicplayer : MonoBehaviour
{
    [SerializeField] private AudioSource _audio;

    void OnValidate()
    {
        if (_audio == null) _audio = GetComponent<AudioSource>();
    }

    void Start()
    {
        _audio.Play();
    }
}
