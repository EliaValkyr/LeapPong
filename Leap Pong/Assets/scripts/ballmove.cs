using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ballmove : NetworkBehaviour {
	private Rigidbody rb;

	[SyncVar]
	private int hostScore=0; //blau
	[SyncVar]
	private int clientScore=0; //vermell
	private int roundNumber = 0;
	private int bouncesCurrentRound = 0;


	// Use this for initialization
	void Start ()
	{
		//Debug.Log ("ballmove start");
        if (!isServer) return;
        rb = GetComponent<Rigidbody> ();
		rb.velocity = startingDirection ();
	}
	
	// Update is called once per frame
	void FixedUpdate () {	
		//Debug.Log ("ballmove update");
		if (isServer) {

			if (Mathf.Abs (rb.position.x) > 10 || Mathf.Abs (rb.position.z) > 10) {
				if (Mathf.Min (rb.position.x, rb.position.z) < -10) {
					//perd el host
					clientScore++;//vermell
				} else {
					hostScore++;//blau
				}
				roundNumber++;
				bouncesCurrentRound = 0;
				rb.position = new Vector3 (0, 0.5f, 0);
				Vector2 newVel = Random.insideUnitCircle;
				newVel = newVel / newVel.magnitude;
				newVel = newVel * Speed ();
				rb.velocity = startingDirection ();
			}
		}
		GameObject.Find ("RedScoreText").GetComponent<Text> ().text = "Red: " + clientScore;
		GameObject.Find ("BlueScoreText").GetComponent<Text> ().text = "Blue: " + hostScore;

	}

	void OnCollisionEnter(Collision c)
    {
        if (!isServer) return;
        GameObject objecte = c.collider.gameObject;
		if (objecte.name.Contains ("Pad")) {
			Vector3 oldVelocity = rb.velocity;
			Vector3 normal = new Vector3(1,0,0);
			if (objecte.name == "TopPad")
				normal = new Vector3 (0, 0, -1);
			if (objecte.name == "RightPad")
				normal = new Vector3 (-1, 0, 0);
			if (objecte.name == "BottomPad")
				normal = new Vector3 (0, 0, 1);
			if (objecte.name == "LeftPad")
				normal = new Vector3 (1, 0, 0);

			Vector3 reflectVelocity = Vector3.Reflect (oldVelocity, normal);
			Vector3 randomVelocity = transform.position - objecte.transform.position;
			reflectVelocity.y = 0;
			randomVelocity.y = 0;
			reflectVelocity.Normalize ();
			randomVelocity.Normalize ();

			Vector3 finalVelocity = reflectVelocity + randomVelocity;
			finalVelocity.Normalize ();

			rb.velocity = Speed() * finalVelocity;
			bouncesCurrentRound++;
		}
	}

	float Speed(){
		float baseSpeed = 5;
		float roundMultiplier = Mathf.Min (2, 1 + (float) roundNumber / 10);
		float bounceMultiplier = 1 + (float) bouncesCurrentRound / 8;
		return baseSpeed * roundMultiplier * bounceMultiplier;
	}

	Vector3 startingDirection(){
		Vector2 newVel = Random.insideUnitCircle;
		newVel = newVel / newVel.magnitude;
		newVel = newVel * Speed();
		return new Vector3 (newVel.x, 0, newVel.y);
	}
}