using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraParentScript : MonoBehaviour {

    public float ang_vel;

    private float angle = 0;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		//Debug.Log ("camera update");
        transform.Rotate(0, ang_vel, 0);
        angle = angle + ang_vel;
	}

    public float getAngle() { return angle; }
}

