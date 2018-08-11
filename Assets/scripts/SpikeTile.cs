using UnityEngine;
using System.Collections;
using System;

public class SpikeTile : FloorTile
{
	public GameObject spikeHolder;
	private bool spikeEnabled = false;
	private Vector3 raised = Vector3.up * 1.5f;
	private Vector3 lowered = Vector3.zero;
	public float raiseTime = 1.0f;
	private float startTime;

	public void Trigger()
	{
		startTime = Time.time;
		spikeEnabled = true;
	}

	IEnumerator WaitAndRun(float t, Action a) {
		yield return new WaitForSeconds(t);
		a();
	}

	// Use this for initialization
	void Start()
	{
		StartCoroutine(WaitAndRun(3, () => Trigger()));
		lowered = spikeHolder.transform.localPosition;
		raised = lowered + raised;
	}

	// Update is called once per frame
	void Update()
	{
		if(spikeEnabled)
		{
			float completeness = (Time.time - startTime) / raiseTime;
			spikeHolder.transform.localPosition = Vector3.Lerp(lowered, raised, completeness);
			if(completeness >= 1)
			{
				spikeEnabled = false;
			}
		}
	}
}
