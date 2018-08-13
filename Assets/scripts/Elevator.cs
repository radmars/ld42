using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour {
	public GameObject leftDoor;
	public GameObject rightDoor;

	public delegate void ElevatorHandler();
	public ElevatorHandler OnInElevator;
	public float openDuration = 4f;

    public AudioSource audioSource;

	void Start () {
		StartCoroutine(Open());
	}

	private void OnTriggerEnter(Collider other)
	{
		if(other.tag == "Player")
		{
			Debug.Log("In the damn thing");
			if (OnInElevator != null)
			{
				OnInElevator();
			}
		}
	}

	private IEnumerator Open()
	{
        audioSource.Play();

		float offset = 1.65f;
		float startTime = Time.time;
		float openTime = startTime + openDuration;

		Vector3 leftStart = leftDoor.transform.position;
		Vector3 leftTarget = leftStart + new Vector3(0, 0, -offset);

		Vector3 rightStart = rightDoor.transform.position;
		Vector3 rightTarget = rightStart + new Vector3(0, 0, offset);

		while(Time.time < openTime)
		{
			var progress = (Time.time - startTime) / openDuration;
			leftDoor.transform.position = Vector3.Lerp(leftStart, leftTarget, progress);
			rightDoor.transform.position = Vector3.Lerp(rightStart, rightTarget, progress);
			yield return new WaitForFixedUpdate();
		}
	}

}
