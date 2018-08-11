using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chairman : MonoBehaviour
{
	private Rigidbody body;
	public GameObject o;

	private GameObject previous;

	// Use this for initialization
	void Start()
	{
		body = GetComponent<Rigidbody>();
	}

	// Update is called once per frame
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.W))
		{
			force(0, 0);
		}
		else if (Input.GetKeyDown(KeyCode.Q))
		{
			force(.5f, .5f);
		}
		else if (Input.GetKeyDown(KeyCode.E))
		{
			force(-.5f, .5f);
		}
		else if(Input.GetKeyDown(KeyCode.A))
		{
			force(.5f, -.5f);
		}
		else if(Input.GetKeyDown(KeyCode.D))
		{
			force(-.5f, -.5f);
		}
	}

	private void force(float x, float y)
	{
		Destroy(previous);
		previous = Instantiate(o);
		var fwd = transform.forward;
		fwd.y = 0;
		fwd.Normalize();
		if(fwd.magnitude == 0)
		{
			fwd = transform.up;
		}
		var offset = new Vector3(x, y, 0);
		var q = Quaternion.LookRotation(fwd);
		offset = q * offset;
		var forcePosition = this.transform.position - fwd + offset;
		previous.transform.position = forcePosition;
		body.AddForceAtPosition(transform.forward, forcePosition, ForceMode.Impulse);
		Debug.Log(transform.forward);
		Debug.Log(forcePosition);
	}
}
