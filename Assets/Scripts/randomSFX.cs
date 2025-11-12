using System;
using UnityEngine;

public class randomSFX : MonoBehaviour
{
    public AudioSource randomSound;
    [SerializeField] public AudioClip[] sfxList;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        randomSound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    
    public void playRand()
    {
        randomSound.clip = sfxList[UnityEngine.Random.Range(0, sfxList.Length)];
        randomSound.Play();
    }
}
