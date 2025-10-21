using UnityEngine;
using UnityEngine.Audio;


public class AudioMuteToggle : MonoBehaviour
{
[SerializeField] AudioMixer mixer;
[SerializeField] string exposedParam = "Volume";
[SerializeField] float mutedDb = -80f;
float? lastDb;


public void ToggleMute()
{
if (!mixer) return;
float current;
if (!mixer.GetFloat(exposedParam, out current)) current = 0f;
if (current <= mutedDb + 0.1f)
{
mixer.SetFloat(exposedParam, lastDb ?? 0f);
}
else
{
lastDb = current;
mixer.SetFloat(exposedParam, mutedDb);
}
}
}