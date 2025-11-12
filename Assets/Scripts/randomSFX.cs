using System;
using UnityEngine;

public class randomSFX : MonoBehaviour
{
    public AudioSource randomSound;
    [SerializeField] public AudioClip[] sfxList;
    private int lastFrame = -1;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        randomSound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    
    public void PlayRand()
    {
        if (Time.frameCount == lastFrame) return;
        lastFrame = Time.frameCount;
        int rand = UnityEngine.Random.Range(0, sfxList.Length);
        //randomSound.clip = sfxList[rand];
        randomSound.PlayOneShot(sfxList[rand]);
    }
}
