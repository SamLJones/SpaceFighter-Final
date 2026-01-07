using UnityEngine;

public class Songs : MonoBehaviour
{
    [SerializeField] private AudioClip[] songs = new AudioClip[3];
    private AudioSource audioSource;
    private int currentSong = 0;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        PlayCurrentSong();
        audioSource.volume = 0.2f;
    }

    void Update()
    {
        if (!audioSource.isPlaying)
        {
            PlayNextSong();
        }
    }

    void PlayCurrentSong()
    {
        if (songs.Length > 0 && currentSong < songs.Length)
        {
            audioSource.clip = songs[currentSong];
            audioSource.Play();
        }
    }

    void PlayNextSong()
    {
        currentSong++;
    
        if (currentSong >= songs.Length)
        {
            currentSong = 0;
        }

        PlayCurrentSong();
    }
}
