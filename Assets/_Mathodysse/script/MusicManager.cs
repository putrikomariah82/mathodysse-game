using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;
    private AudioSource source;
    private bool isOn = false;

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        source = GetComponent<AudioSource>();
        source.loop = true;
        source.playOnAwake = false;
    }

    public void SetMusic(bool on)
    {
        isOn = on;
        if (on) source.Play();
        else source.Stop();
    }

    public bool IsOn() => isOn;
}