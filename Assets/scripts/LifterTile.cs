using UnityEngine;
using System.Collections;
using System;

public class LifterTile: TileEffect
{
	public GameObject liftee;
	private bool lifterEnabled = false;
	private Vector3 raised = Vector3.up * 1.5f;
	private Vector3 lowered = Vector3.zero;
	public float raiseTime = 1.0f;
	private float startTime;

	public override void Trigger()
	{
		base.Trigger();
		startTime = Time.time;
		lifterEnabled = true;
	}

	IEnumerator WaitAndRun(float t, Action a) {
		yield return new WaitForSeconds(t);
		a();
	}

	// Use this for initialization
	void Start()
	{
		StartCoroutine(WaitAndRun(3, () => Trigger()));
		lowered = liftee.transform.localPosition;
		raised = lowered + raised;
	}

	// Update is called once per frame
	void Update()
	{
		if(lifterEnabled)
		{
			float completeness = (Time.time - startTime) / raiseTime;
			liftee.transform.localPosition = Vector3.Lerp(lowered, raised, completeness);
			if(completeness >= 1)
			{
				lifterEnabled = false;
				MarkFinished();
			}
		}
	}
}
