using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockSoundController : MonoBehaviour {

    public AudioSource audioSource;
    private int playingCount;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void BlockMoving()
    {
        playingCount++;
        if (playingCount == 1)
        {
            audioSource.Play();
        }
    }

    public void BlockStopped()
    {
        if (playingCount > 0)
        {
            playingCount--;
        }
        if (playingCount == 0)
        {
            audioSource.Stop();
        }
    }
}
