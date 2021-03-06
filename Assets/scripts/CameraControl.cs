﻿using UnityEngine;

/// <summary>
/// https://wiki.unity3d.com/index.php/MouseOrbitImproved
/// </summary>
public class CameraControl : MonoBehaviour
{
	public Transform target;
	public float distance = 5.0f;
	public float xSpeed = 120.0f;
	public float ySpeed = 120.0f;

	public float yMinLimit = -20f;
	public float yMaxLimit = 80f;

	public float distanceMin = .5f;
	public float distanceMax = 15f;
	public Vector3 offset = Vector3.zero;

	private Rigidbody body;

	float x = 0.0f;
	float y = 0.0f;

	// Use this for initialization
	void Start()
	{
		Cursor.lockState = CursorLockMode.Locked;
		Vector3 angles = transform.eulerAngles;
		x = angles.y;
		y = angles.x;

		body = GetComponent<Rigidbody>();

		// Make the rigid body not change rotation
		if (body != null)
		{
			body.freezeRotation = true;
		}
	}

	private bool breakCamera = false;
	void LateUpdate()
	{
		if (breakCamera)
		{
			return;
		}
		if(Input.GetKeyDown(KeyCode.Space))
		{
			breakCamera = true;
		}
		if (target)
		{
			x += Input.GetAxis("Mouse X") * xSpeed * distance * 0.02f;
			y -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;

			y = ClampAngle(y, yMinLimit, yMaxLimit);

			Quaternion rotation = Quaternion.Euler(y, x, 0);

			distance = Mathf.Clamp(distance - Input.GetAxis("Mouse ScrollWheel") * 5, distanceMin, distanceMax);

			/*
			// This is supposed to put the camera in front of things between target and distance, except is buggy
			RaycastHit hit;
			if (Physics.Linecast(target.position, transform.position, out hit))
			{
				distance -= hit.distance;
			}
			*/
			Vector3 negDistance = new Vector3(0.0f, 0.0f, -distance);
			Vector3 position = rotation * negDistance + target.position;

			transform.rotation = rotation;
			transform.position = position + offset;
		}
	}

	public static float ClampAngle(float angle, float min, float max)
	{
		if (angle < -360F)
			angle += 360F;
		if (angle > 360F)
			angle -= 360F;
		return Mathf.Clamp(angle, min, max);
	}
}