using UnityEngine;
using System.Collections;
using System;

public class BlockMover : TileEffect
{
    public Vector3 moveDirection;
    public float moveAmount;
    public float extendTime = 5.0f;

    private bool moverEnabled = false;
    private Vector3 extended; // = Vector3.up * 1.5f;
    private Vector3 retracted = Vector3.zero;
    private float startTime;

    public override void Trigger()
    {
        base.Trigger();
        startTime = Time.time;
        moverEnabled = true;
    }

    IEnumerator WaitAndRun(float t, Action a)
    {
        yield return new WaitForSeconds(t);
        a();
    }

    // Use this for initialization
    void Start()
    {
        extended = moveDirection * moveAmount;
        StartCoroutine(WaitAndRun(3, () => Trigger()));
        retracted = gameObject.transform.localPosition;
        extended = retracted + extended;
    }

    // Update is called once per frame
    void Update()
    {
        if (moverEnabled)
        {
            float completeness = (Time.time - startTime) / extendTime;
            gameObject.transform.localPosition = Vector3.Lerp(retracted, extended, completeness);
            if (completeness >= 1)
            {
                moverEnabled = false;
                MarkFinished();
            }
        }
    }
}
