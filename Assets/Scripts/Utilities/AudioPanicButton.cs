using UnityEngine;
using UnityEngine.Audio;

public class AudioPanicButton : MonoBehaviour
{
    [Header("Optional: assign if you use a mixer")]
    public AudioMixer mixer;
    public string masterParam = "MasterVolume";

    AudioSource src;

    void Awake()
    {
        // Ensure a listener exists and isn't paused
        var cam = Camera.main ? Camera.main.gameObject : null;
        if (cam && !cam.GetComponent<AudioListener>()) cam.AddComponent<AudioListener>();
        AudioListener.pause = false;
        AudioListener.volume = 1f;

        // Force mixer to 0 dB if present
        if (mixer) mixer.SetFloat(masterParam, 0f);

        // Add a 2D source
        src = gameObject.AddComponent<AudioSource>();
        src.playOnAwake = false;
        src.loop = false;
        src.volume = 0.9f;
        src.spatialBlend = 0f; // 2D
        src.mute = false;

        Debug.Log("[AudioPanicButton] Ready. Click the on-screen button to play a test beep.");
    }

    void OnGUI()
    {
        if (GUI.Button(new Rect(10, 10, 280, 44), "▶ Play Beep (forces user gesture)"))
        {
            PlayBeep440();
        }
    }

    void PlayBeep440()
    {
        int sampleRate = 44100, samples = sampleRate;
        var clip = AudioClip.Create("Beep440", samples, 1, sampleRate, false);
        float freq = 440f;
        var data = new float[samples];
        for (int i = 0; i < samples; i++)
            data[i] = Mathf.Sin(2f * Mathf.PI * freq * i / sampleRate) * 0.2f;
        clip.SetData(data, 0);

        src.Stop();
        src.clip = clip;
        src.Play();
        Debug.Log("[AudioPanicButton] Beep attempted.");
    }
}
