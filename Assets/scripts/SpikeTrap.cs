﻿using UnityEngine;
using System.Collections;
using System;

public class SpikeTrap : TileEffect
{
    private Vector3 moveDirection = Vector3.up;
    private float moveAmount = 3f;
    private float extendTime = .2f;

    private GameObject spikes;
    private GameObject warningSpikes;

    private bool moverEnabled = false;
    private bool retracting = false;
    private bool warningEnabled = false;
    private Vector3 extended;
    private Vector3 semiExtended;
    private Vector3 retracted = Vector3.zero;
    private Vector3 retractFrom = Vector3.zero;
    private float startTime;
    private float retractTime = 1.5f;

    private bool paused = false;
    private float pauseCountdown;
    private float lastTime;

    public AudioSource audioSource;
    public AudioClip warningAudio;
    public AudioClip spikeAudio;

    private GameObject chairman;

    // Use this for initialization
    void Start()
    {
        extended = moveDirection * moveAmount;
        retracted = gameObject.transform.localPosition;
        extended = retracted + extended;
        semiExtended = retracted + new Vector3(0f, .6f, 0f);

        spikes = transform.Find("Spike Holder").gameObject;
        warningSpikes = transform.Find("Spike Warning").gameObject;
        //warningSpikes.gameObject.SetActive(false);
        gameObject.SetActive(false);

        //Trigger();

        chairman = GameObject.Find("chairman");
    }

    public override void Trigger()
    {
        print("Trigger");
        gameObject.SetActive(true);
        base.Trigger();
        startTime = Time.time;
        warningEnabled = true;

        //warningSpikes.gameObject.SetActive(true);
        spikes.gameObject.SetActive(false);

        audioSource.Stop();
        audioSource.clip = warningAudio;
        audioSource.Play();
    }

    public override void Retract()
    {
        print("Retract");

        retracting = true;
        retractFrom = gameObject.transform.localPosition;
        startTime = Time.time;

        //warningSpikes.gameObject.SetActive(true);
        spikes.gameObject.SetActive(false);

    }

    public void Reset()
    {
        moverEnabled = false;
        retracting = false;
        gameObject.SetActive(false);
        gameObject.transform.localPosition = retracted;

        //warningSpikes.gameObject.SetActive(true);
        spikes.gameObject.SetActive(false);
    }



    // Update is called once per frame
    void Update()
    {
        if (pauseCountdown > 0) {
            float timeDiff = Time.time - lastTime;
            pauseCountdown -= timeDiff;
            lastTime = Time.time;
            return;  
        }
        else if (paused && pauseCountdown <= 0)
        {
            paused = false;
            startTime = Time.time;
            //warningSpikes.gameObject.SetActive(false);
            spikes.gameObject.SetActive(true);

            audioSource.Stop();
            audioSource.clip = spikeAudio;
            audioSource.Play();

            Vector3 heading = chairman.transform.position - transform.position;
            float dist = heading.magnitude;

            if (dist < 5.0f)
            {
                Chairman ch = chairman.GetComponent<Chairman>();
                ch.Panic();
            } 
        }


        if (retracting)
        {
            float completeness = (Time.time - startTime) / retractTime;
            gameObject.transform.localPosition = Vector3.Lerp(retractFrom, retracted, completeness);
            if (completeness >= 1)
            {
                Stop();
                retracting = false;
                gameObject.SetActive(false);
            }
        }
        else if (warningEnabled)
        {
            float completeness = (Time.time - startTime) / extendTime;
            gameObject.transform.localPosition = Vector3.Lerp(retracted, semiExtended, completeness);
            if (completeness >= 1)
            {
                paused = true;
                lastTime = Time.time;
                warningEnabled = false;
                moverEnabled = true;
                pauseCountdown = 2.5f;
            }

        }
        else if (moverEnabled)
        {
            float completeness = (Time.time - startTime) / extendTime;
            gameObject.transform.localPosition = Vector3.Lerp(semiExtended, extended, completeness);
            if (completeness >= 1)
            {
                Stop();
            }
        }
    }

    private void Stop()
    {
        print("Stop");
        moverEnabled = false;
        MarkFinished();
    }


}

