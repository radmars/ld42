using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFollower : MonoBehaviour {

    public GameObject FollowObject;
    public float Height;

    private Transform FollowTrans;
    private float FollowHeight;

	// Use this for initialization
	void Start () {
        if(Height == 0){
            FollowHeight = this.transform.position.y;
        } else {
            FollowHeight = Height;
        }
        FollowTrans = FollowObject.transform;
	}
	
	// Update is called once per frame
	void Update () {
        UpdatePosition();
	}

    private void UpdatePosition(){
        Vector3 v = FollowTrans.position;
        v.y = FollowHeight;
        this.transform.position = v;
    }
}
