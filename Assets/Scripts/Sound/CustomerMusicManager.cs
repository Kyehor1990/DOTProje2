using UnityEngine;

public class CustomerMusicManager : MonoBehaviour
{
    public static CustomerMusicManager instance;

    private AudioSource audioSource;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // sahneler arası kaybolmasın
            audioSource = GetComponent<AudioSource>();
        }
        else
        {
            Destroy(gameObject); // ikinci bir instance olmasın
        }
    }

    public void PlayCustomerMusic()
    {
        if (audioSource != null && !audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }

    public void StopCustomerMusic()
    {
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }
}
