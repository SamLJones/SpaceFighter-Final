using UnityEngine;

public class Songs : MonoBehaviour
{
    [SerializeField] private AudioClip[] songs = new AudioClip[3];
    private AudioSource audioSource;
    private int currentSong = 0;

    void Start()
    {
        int currentSong = Random.Range(1, 4);
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

// The script manages background music
// This is 3 songs, that will cycle after the last finishes playing
// Audio resets each time scene resets, WIP to make the song consistent and continue playing.
