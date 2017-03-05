using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour {


	// Use this for initialization
	void Start () {
		Time.timeScale = 1;
		Show(GameObject.Find("MenuPanel").GetComponent<CanvasGroup>());
		Hide(GameObject.Find("ScorePanel").GetComponent<CanvasGroup>());

		//Debug.Log ("ui manager start");
	}
	
	// Update is called once per frame
	void Update () {
		//Debug.Log ("ui manager update " + Time.timeScale);
		//if (Input.GetKeyDown (KeyCode.Escape)) {
		//	Application.Quit ();
		//}
	}
		

	public void Hide(CanvasGroup cg){
		//Debug.Log ("Amago " + cg.name);
		cg.alpha = 0;
		cg.blocksRaycasts = false;
	}

	public void Show(CanvasGroup cg){
		//Debug.Log ("Ensenyo " + cg.name);
		cg.alpha = 1;
		cg.blocksRaycasts = true;
	}
}
