using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour {
	
	public Transform player;
	//public Transform background;
	//public float startSpeed = f3;
	//public float augmentSpeed = f0.001;
	
	
	// Update is called once per frame
	void Update () {
		
		//startSpeed += augmentSpeed;
		if (player) {
			transform.position = new Vector3 ((float)player.position.x + 0, (float)player.position.y + 3, -10); //6 = startSpeed
			//PARA BACKGROUND //transform.position = new Vector3 ((float)background.position.x, (float)background.position.y);
			//transform.Translate (Vector3.forward * 100 / 100 * Time.deltaTime);
		}
		transform.Translate (Vector3.forward * 100 / 100 * Time.deltaTime);

	}
}

