using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GruntController : MonoBehaviour {
	private void OnTriggerEnter(Collider other)
	{
        Chairman chairman = transform.parent.gameObject.GetComponent<Chairman>();
        chairman.Fall();
	}
}
