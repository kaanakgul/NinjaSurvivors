using UnityEngine;

public class MusicManager : MonoBehaviour
{
    private static MusicManager instance; // Singleton instance to prevent duplicates
    private AudioSource audioSource;

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
}
