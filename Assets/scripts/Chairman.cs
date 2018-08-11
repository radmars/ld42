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
		transform.Rotate(0, 0.1f, 0);
		if (Input.GetKeyDown(KeyCode.Q))
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
		// TODO Rotate into fwd orientation?
		var offset = new Vector3(x, y, 0);
		var q = Quaternion.LookRotation(fwd);
		offset = q * offset;
		var forcePosition = this.transform.position - fwd + offset;
		previous.transform.position = forcePosition;
		body.AddForceAtPosition(forcePosition, transform.forward, ForceMode.Impulse);
	}
}
