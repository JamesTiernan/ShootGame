using System;
using System.Linq;
using UnityEngine;

public class randomSFX : MonoBehaviour
{
    string[] anims = {"playerLand","playerRoll","playerRun","playerWalk","playerCrouchWalk"};
    [SerializeField] public AudioClip[] sfxList;
    [SerializeField] GameObject audioPlayer;
    
    public void PlayRand()
    {
        // Ensures the player is in one of the right animations, to avoid SFX playing during transitions.
        AnimatorClipInfo[] currentClipInfo = GetComponent<Animator>().GetCurrentAnimatorClipInfo(0);
        string clipName = currentClipInfo[0].clip.name;
        Debug.Log(clipName);
        if (anims.Contains(clipName))
        {
            GameObject myInstance = GameObject.Instantiate(audioPlayer, transform.position, Quaternion.identity) as GameObject;
            myInstance.GetComponent<AudioSource>().clip = sfxList[UnityEngine.Random.Range(0, sfxList.Length - 1)];
        }
    }
}
