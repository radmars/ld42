using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrushDetector : MonoBehaviour {

    public Chairman chairman;

    private void OnTriggerEnter(Collider other)
    {
        print("CrushDetector collided");
        if (other.tag == "Obstacle")
        {
            chairman.Crush();
        }
    }

}
