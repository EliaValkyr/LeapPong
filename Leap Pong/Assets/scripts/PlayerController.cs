using UnityEngine;
using UnityEngine.Networking;
using Leap;
using Leap.Unity;

public class PlayerController : NetworkBehaviour
{
	public float vel;
    public float llarg;
	private GameObject hp, vp;
    private GameObject rotador;
	LeapProvider provider;


	// Use this for initialization
	void Start()
	{
        rotador = GameObject.Find("CameraParent");
		if (isServer ^ isLocalPlayer)
		{
			hp = GameObject.Find("RightPad");
			vp = GameObject.Find("TopPad");
		}
		else
		{
			hp = GameObject.Find("LeftPad");
			vp = GameObject.Find("BottomPad");
		}

		if (isLocalPlayer) {
			Camera cam = GameObject.Find ("Camera").GetComponent<Camera>();
            
			float angle = rotador.GetComponent<CameraParentScript>().getAngle();
            rotador.transform.Rotate(0, -angle, 0);
			if (isServer)
            {
                cam.transform.position = new Vector3(-20, 25, -20);
                cam.transform.rotation = Quaternion.Euler(45, 45, 0);
                //cam.transform.Rotate
            } else
            {
                cam.transform.position = new Vector3(20, 25, 20);
                cam.transform.rotation = Quaternion.Euler(45, 225, 0);
			}
            rotador.transform.Rotate(0, angle, 0);
		}

		//Debug.Log ("player start");
		provider = FindObjectOfType<LeapProvider>() as LeapProvider;
	}

	// Update is called once per frame
	void FixedUpdate()
	{
		//Debug.Log ("player update");
		if (isLocalPlayer)
		{
			Frame frame = provider.CurrentFrame;
            Rigidbody rb = GetComponent<Rigidbody>();
			Vector3 newPadPos;

            if (frame.Hands.Count > 0)
			{
				Hand hand = frame.Hands [0];
				Vector3 padPos = GetComponent<Rigidbody>().position;
				Vector3 handPos = hand.PalmPosition.ToVector3 ();
				float handx = handPos.x;
				float handz = handPos.z;

				float maxz = 0.5f;
				float minz = 0.1f;
				float maxx = -0.1f;
				float minx = -0.5f;

				float padx = handx + handz;
				float padz = handx - handz;

				float x = (padx -0.3f)*50;
				float z = (-padz -0.3f)*50;
				float angle = rotador.transform.rotation.eulerAngles.y / 180 * Mathf.PI;
				padPos.x = x * Mathf.Cos(angle) + z * Mathf.Sin(angle);
				padPos.z = -x * Mathf.Sin(angle) + z * Mathf.Cos(angle);

				//Debug.Log(hand.PalmPosition.ToVector3 ());
				newPadPos = padPos;

				if (isServer) {
					rb.position = newPadPos;
				} else {
					Vector3 auxPos = -newPadPos;
					//Debug.Log ("Old pos = " + rb.position + ", newpadpos = " + newPadPos + ", real new pos = " + auxPos);
					rb.position = auxPos;
				}
			}else{
				float x = Input.GetAxis("Horizontal") * vel;
				float z = Input.GetAxis("Vertical") * vel;
				float angle = rotador.transform.rotation.eulerAngles.y / 180 * Mathf.PI + Mathf.PI/4;
                float x2 = x * Mathf.Cos(angle) + z * Mathf.Sin(angle);
                float z2 = -x * Mathf.Sin(angle) + z * Mathf.Cos(angle);
                newPadPos = rb.position + new Vector3(x2, 0, z2);
				if (isServer) {
					rb.position = rb.position + new Vector3(x2, 0, z2);
				} else {
					rb.position = rb.position - new Vector3(x2, 0, z2);
				}
			}

            float xn, zn;
            xn = Mathf.Min(rb.position.x, 10 - llarg / 2);
            xn = Mathf.Max(xn, -10 + llarg / 2);
            zn = Mathf.Min(rb.position.z, 10 - llarg / 2);
            zn = Mathf.Max(zn, -10 + llarg / 2);
            rb.position = new Vector3(xn, rb.position.y, zn);
        }

		if (hp != null)
		{
			Rigidbody rhp = hp.GetComponent<Rigidbody>();
			Rigidbody rvp = vp.GetComponent<Rigidbody>();
			rhp.position = new Vector3(rhp.position.x, rhp.position.y, GetComponent<Rigidbody>().position.z);
			rvp.position = new Vector3(GetComponent<Rigidbody>().position.x, rvp.position.y, rvp.position.z);
		}

	}

}