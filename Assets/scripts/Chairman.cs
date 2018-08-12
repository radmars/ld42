﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Chairman : MonoBehaviour
{
	private Rigidbody body;

	public delegate void DeathHanlder(string why);
	public DeathHanlder OnDie;

	private bool alive;

	// Use this for initialization
	void Start()
	{
		body = GetComponent<Rigidbody>();
		alive = true;
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
		else if (Input.GetKeyDown(KeyCode.A))
		{
			Force(new Vector3(1f, -1f));
		}
		else if (Input.GetKeyDown(KeyCode.D))
		{
			Force(new Vector3(-1f, -1f));
		}
		else if (Input.GetKeyDown(KeyCode.S))
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
		var fwd = transform.forward;
		fwd.Normalize();

		var q = Quaternion.LookRotation(fwd);
		offset = (q * offset);

		var forcePosition = this.transform.position - fwd + offset;
		body.AddForceAtPosition(direction, forcePosition, ForceMode.Impulse);
	}

	private void OnCollisionStay(Collision collision)
	{
		if (collision.collider.tag == "Obstacle" && Mathf.Abs(collision.contacts[0].separation) > .1f)
		{
			Die("You got crushed");
		}
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.collider.tag == "Fatal")
		{
			Die("Stabbed");
		}
	}

	private void Die(string why)
	{
		if (alive)
		{
			alive = false;
			var handler = OnDie;
			if (handler != null)
			{
				handler(why);
			}
		}
	}
}
