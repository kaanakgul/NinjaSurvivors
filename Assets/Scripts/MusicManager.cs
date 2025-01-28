using UnityEngine;

public class MusicManager : MonoBehaviour
{
    private static MusicManager instance; // Singleton instance
    private AudioSource audioSource;

    public static MusicManager Instance
    {
        get { return instance; }
    }

    void Awake()
    {
        if (instance == null)
        {
            // Set this as the singleton instance and don't destroy it between scenes
            instance = this;
            DontDestroyOnLoad(gameObject);

            // Get the AudioSource component
            audioSource = GetComponent<AudioSource>();

            // Play the music if it's not already playing
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
        else
        {
            // If another instance exists, destroy this one
            Destroy(gameObject);
        }
    }

    public void SetVolume(float volume)
    {
        if (audioSource != null)
        {
            audioSource.volume = volume;
            Debug.Log("SetVolume: " + volume);
        }
    }

    public float GetVolume()
    {
        return audioSource != null ? audioSource.volume : 1f; // Default volume is 1
    }
}
