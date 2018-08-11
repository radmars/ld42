using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chairman : MonoBehaviour {
	private Rigidbody body;

	// Use this for initialization
	void Start () {
		body = GetComponent<Rigidbody>();
	}

	// Update is called once per frame
	void Update () {
		if (Input.anyKeyDown)
		{
			body.AddForceAtPosition(new Vector3(1f, .1f, 0), Vector3.forward, ForceMode.Impulse);
		}
	}
}
