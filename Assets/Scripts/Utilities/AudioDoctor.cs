using UnityEngine;
using UnityEngine.Audio;

public class AudioDoctor : MonoBehaviour
{
    [Header("Optional: Assign your mixer to verify it")]
    public AudioMixer mixer;                   // leave null if not using
    public string masterParam = "MasterVolume";

    [Header("Optional: Assign a one-shot clip to test SFX")]
    public AudioClip testClip;

    AudioSource src;
    static AudioListener listener;

    void Awake()
    {
        Debug.Log("=== AudioDoctor ===");

        // Ensure a single AudioListener on main camera
        var cam = Camera.main ? Camera.main.gameObject : null;
        if (cam)
        {
            var ls = cam.GetComponent<AudioListener>() ?? cam.AddComponent<AudioListener>();
            if (listener && listener != ls) DestroyImmediate(ls);
            else listener = ls;
        }
        else Debug.LogWarning("AudioDoctor: No Main Camera found.");

        // Make a 2D AudioSource
        src = gameObject.AddComponent<AudioSource>();
        src.playOnAwake = false;
        src.loop = false;
        src.spatialBlend = 0f;
        src.volume = 0.9f;

        // Log mixer value
        if (mixer)
        {
            float v;
            if (mixer.GetFloat(masterParam, out v))
                Debug.Log($"Mixer {masterParam} = {v} dB");
            else
                Debug.LogWarning($"Mixer param '{masterParam}' not found.");
        }

        // Log project-wide settings
        Debug.Log($"AudioSettings: dspTime={AudioSettings.dspTime}, outputSampleRate={AudioSettings.outputSampleRate}, speakerMode={AudioSettings.speakerMode}");
    }

    void OnGUI()
    {
        const int w = 260, h = 42;
        if (GUI.Button(new Rect(10, 10, w, h), "▶ Play Test Beep (user gesture)"))
        {
            PlayBeep440();
        }
        if (GUI.Button(new Rect(10, 60, w, h), "▶ Play Test Clip (oneshot)"))
        {
            if (testClip) src.PlayOneShot(testClip);
            else Debug.LogWarning("Assign a testClip on AudioDoctor to try this.");
        }
        if (GUI.Button(new Rect(10, 110, w, h), "Unmute Mixer (0 dB)"))
        {
            if (mixer) mixer.SetFloat(masterParam, 0f);
        }
    }

    // Procedural 440 Hz beep (1 second) — bypasses asset issues
    void PlayBeep440()
    {
        int sampleRate = 44100;
        int samples = sampleRate;
        var clip = AudioClip.Create("Beep440", samples, 1, sampleRate, false);
        float freq = 440f;

        var data = new float[samples];
        for (int i = 0; i < samples; i++)
            data[i] = Mathf.Sin(2f * Mathf.PI * freq * i / sampleRate) * 0.2f;
        clip.SetData(data, 0);

        src.spatialBlend = 0f;
        src.Stop();
        src.clip = clip;
        src.loop = false;
        src.Play();
        Debug.Log("Played procedural 440 Hz beep via user click.");
    }
}
