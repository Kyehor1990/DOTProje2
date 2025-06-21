using UnityEngine;

public class DungeonMusicManager : MonoBehaviour
{
    public static DungeonMusicManager instance;

    private AudioSource audioSource;

    public AudioClip dungeonMusic;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.clip = dungeonMusic;
            audioSource.loop = true;
            audioSource.playOnAwake = false;
            audioSource.volume = 0.05f;
        }
    }

    public void PlayDungeonMusic()
    {
        if (!audioSource.isPlaying)
            audioSource.Play();
    }

    public void StopDungeonMusic()
    {
        if (audioSource.isPlaying)
            audioSource.Stop();
    }
}
