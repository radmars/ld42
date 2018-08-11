using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chairman : MonoBehaviour
{
	private Rigidbody body;
	public GameObject o;

	private GameObject previous;
	public bool crushed;

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
			Force(new Vector3(0, -1f));
		}
		else if (Input.GetKeyDown(KeyCode.Q))
		{
			Force(new Vector3(1f, 1f), transform.up);
		}
		else if (Input.GetKeyDown(KeyCode.E))
		{
			Force(new Vector3(1f, 1f), transform.up);
		}
		else if(Input.GetKeyDown(KeyCode.A))
		{
			Force(new Vector3(1f, -1f));
		}
		else if(Input.GetKeyDown(KeyCode.D))
		{
			Force(new Vector3(-1f, -1f));
		}
		else if(Input.GetKeyDown(KeyCode.S))
		{
			Force(new Vector3(0, -1f, 2), transform.forward * -1);
		}
	}

	private void Force(Vector3 offset)
	{
		Force(offset, transform.forward + Vector3.up * .1f);
	}

	private void Force(Vector3 offset, Vector3 direction)
	{
		Destroy(previous);
		previous = Instantiate(o);
		var fwd = transform.forward;
		fwd.Normalize();

		var q = Quaternion.LookRotation(fwd);
		offset = (q * offset);

		var forcePosition = this.transform.position - fwd + offset;
		Debug.Log("outbound offset " + offset);
		previous.transform.position = forcePosition;
		body.AddForceAtPosition(direction, forcePosition, ForceMode.Impulse);
	}

	private void OnCollisionStay(Collision collision)
	{
		if(collision.collider.tag == "Obstacle" && Mathf.Abs(collision.contacts[0].separation) > .1f)
		{
			if(!crushed)
			{
				Debug.Log("You got crushed");
				crushed = true;
			}
		}
	}
}
