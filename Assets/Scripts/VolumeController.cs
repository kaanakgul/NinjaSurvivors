using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    [SerializeField] private Slider volumeSlider;

    private void Start()
    {
        // Ensure the volume slider is assigned
        if (volumeSlider == null)
        {
            Debug.LogError("VolumeSlider: No Slider assigned!");
            return;
        }

        // Ensure MusicManager exists before using it
        if (MusicManager.Instance == null)
        {
            Debug.LogError("VolumeSlider: MusicManager instance not found!");
            return;
        }

        // Get saved volume or default to current volume
        float savedVolume = PlayerPrefs.GetFloat("MusicVolume", MusicManager.Instance.GetVolume());
        volumeSlider.value = savedVolume;

        // Add listener to update MusicManager volume dynamically
        volumeSlider.onValueChanged.AddListener(OnVolumeChanged);
    }

    private void OnVolumeChanged(float volume)
    {
        if (MusicManager.Instance != null)
        {
            // Update the volume in MusicManager
            MusicManager.Instance.SetVolume(volume);

            // Save the volume setting
            PlayerPrefs.SetFloat("MusicVolume", volume);
            PlayerPrefs.Save(); // Ensure the volume persists across sessions
        }
    }
}
