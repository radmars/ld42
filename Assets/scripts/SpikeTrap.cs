using UnityEngine;
using System.Collections;
using System;

public class SpikeTrap : TileEffect
{
    private Vector3 moveDirection = Vector3.up;
    private float moveAmount = 2f;
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
    private float retractTime = 2f;

    private bool paused = false;
    private float pauseCountdown;
    private float lastTime;

    // Use this for initialization
    void Start()
    {
        extended = moveDirection * moveAmount;
        retracted = gameObject.transform.localPosition;
        extended = retracted + extended;
        semiExtended = retracted + Vector3.Scale(extended, new Vector3(0f, .25f, 0f));

        spikes = transform.Find("Spike Holder").gameObject;
        warningSpikes = transform.Find("Spike Warning").gameObject;
        warningSpikes.gameObject.SetActive(false);
        gameObject.SetActive(false);

        Trigger();
    }

    public override void Trigger()
    {
        print("Trigger");
        gameObject.SetActive(true);
        base.Trigger();
        startTime = Time.time;
        warningEnabled = true;

        warningSpikes.gameObject.SetActive(true);
        spikes.gameObject.SetActive(false);
    }

    public void Retract()
    {
        print("Retract");

        retracting = true;
        retractFrom = gameObject.transform.localPosition;
        startTime = Time.time;

        warningSpikes.gameObject.SetActive(true);
        spikes.gameObject.SetActive(false);

    }

    public void Reset()
    {
        moverEnabled = false;
        retracting = false;
        gameObject.SetActive(false);
        gameObject.transform.localPosition = retracted;

        warningSpikes.gameObject.SetActive(true);
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
            warningSpikes.gameObject.SetActive(false);
            spikes.gameObject.SetActive(true);
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

