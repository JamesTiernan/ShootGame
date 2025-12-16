using UnityEngine;

public class oneShotSFX : MonoBehaviour
{
    public AudioSource audioPlayer;
    bool playing;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playing = false;
        audioPlayer = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!playing)
        {
            if (audioPlayer != null)
            {
                if (audioPlayer.clip != null)
                {
                    StartPlay();
                }
            }
        }
    }

    void DestroySelf()
    {
        Destroy(gameObject);
    }

    void StartPlay()
    {
        playing = true;
        Debug.Log($"Clip: {audioPlayer.clip} | Length: {audioPlayer.clip.length}.");
        audioPlayer.Play();
        Invoke(nameof(DestroySelf),audioPlayer.clip.length);
    }
}
