using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;


public class myNetworkManager : MonoBehaviour{
	UIManager manager;

	public void StartHost(){
		NetworkManager nm = GameObject.Find ("NetworkManager").GetComponent<NetworkManager> ();
		nm.StartHost();
		manager = GameObject.Find ("UIManager").GetComponent<UIManager> ();
		manager.Show (GameObject.Find ("ScorePanel").GetComponent<CanvasGroup> ());
		manager.Hide(GameObject.Find("MenuPanel").GetComponent<CanvasGroup>());
	}

	public void StartClient(){
		string IP = GameObject.Find ("IPText").GetComponent<Text> ().text;
		NetworkManager nm = GameObject.Find ("NetworkManager").GetComponent<NetworkManager> ();
		NetworkClient nc = nm.StartClient ();
		nc.Connect (IP, 7777);
		manager = GameObject.Find ("UIManager").GetComponent<UIManager> ();
		manager.Show (GameObject.Find ("ScorePanel").GetComponent<CanvasGroup> ());
		manager.Hide(GameObject.Find("MenuPanel").GetComponent<CanvasGroup>());
	}
}
