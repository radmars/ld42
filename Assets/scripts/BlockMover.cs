using UnityEngine;
using System.Collections;
using System;

public class BlockMover : TileEffect
{
    public Vector3 moveDirection;
    public float moveAmount;
    public float extendTime = 5.0f;

    private bool moverEnabled = false;
    private bool retracting = false;
    private Vector3 extended; // = Vector3.up * 1.5f;
    private Vector3 retracted = Vector3.zero;
    private Vector3 retractFrom = Vector3.zero;
    private float startTime;
    private float retractTime = 2f;

    private BlockSoundController soundController;

    public override void Trigger()
    {
        print("Trigger");
        gameObject.SetActive(true);
        base.Trigger();
        startTime = Time.time;
        moverEnabled = true;

        soundController.BlockMoving();
    }

    public void Retract()
    {
        print("Retract");

        retracting = true;
        retractFrom = gameObject.transform.localPosition;
        startTime = Time.time;

        soundController.BlockMoving();
    }

    public void Reset()
	{
        moverEnabled = false;
        retracting = false;
        gameObject.SetActive(false);
        gameObject.transform.localPosition = retracted;
	}

    // Use this for initialization
    void Start()
    {
        extended = moveDirection * moveAmount;
        retracted = gameObject.transform.localPosition;
        extended = retracted + extended;
        gameObject.SetActive(false);

        GameObject sc = GameObject.Find("BlockSoundController");
        if (sc)
        {
            soundController = sc.GetComponent<BlockSoundController>();
        }
    }

    // Update is called once per frame
    void Update()
    {
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
        else if (moverEnabled)
        {
            float completeness = (Time.time - startTime) / extendTime;
            gameObject.transform.localPosition = Vector3.Lerp(retracted, extended, completeness);
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

        soundController.BlockStopped();
    }

    private void OnCollisionEnter(Collision collision)
	{
		if(moverEnabled && collision.collider.tag == "Obstacle")
		{
            print("Collide with " + collision.collider.name);
            Stop();
		}
	}
}
