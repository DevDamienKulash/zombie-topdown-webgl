using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(AudioSource))]
public class MusicLoop : MonoBehaviour
{
    public static MusicLoop Instance { get; private set; }

    [SerializeField] AudioClip track;
    [SerializeField] AudioMixer mixer;             // optional
    [SerializeField] string masterParam = "MasterVolume";

    AudioSource src;

    void Awake()
    {
        if (Instance && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        src = GetComponent<AudioSource>();
        src.loop = true;
        src.playOnAwake = false;        // <-- important for WebGL autoplay
        src.spatialBlend = 0f;          // 2D
        if (track) src.clip = track;

        // Safety: unmute mixer on boot (you can remove if not needed)
        if (mixer) mixer.SetFloat(masterParam, 0f);
    }

    public void PlayIfNeeded()
    {
        if (!src || src.isPlaying) return;
        if (track && src.clip != track) src.clip = track;
        src.Play();                     // will succeed after a user gesture
    }

    public void StopMusic()
    {
        if (src && src.isPlaying) src.Stop();
    }
}
