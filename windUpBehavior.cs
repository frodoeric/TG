using UnityEngine;
using System.Collections;

public class windUpBehavior : MonoBehaviour {

	public Rigidbody2D[] pushupers;
	public Rigidbody2D player;
	public float upForce = 50f;
	public float distance = 20f;

	// Use this for initialization
	void Start () {
		if (!player) {
			GameObject playerAux = (GameObject) GameObject.FindGameObjectWithTag("Player");
			if (playerAux && playerAux.rigidbody2D) {
				player = playerAux.rigidbody2D;
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		foreach (Rigidbody2D pushuper in pushupers) {
			if (pushuper) {
				if (pushuper.gameObject.transform.position.x > transform.position.x - transform.localScale.x &&
				    pushuper.gameObject.transform.position.x < transform.position.x + transform.localScale.x &&
				    pushuper.gameObject.transform.position.y >= transform.position.y) {
					float dist = 1 - ((pushuper.gameObject.transform.position.y - transform.position.y) / distance);
					dist  = (dist < 0 ? 0 : dist);
					pushuper.AddForce(new Vector2(0, upForce * (dist) ));
				}
			}
		}
		if (player) {
			if (player.gameObject.transform.position.x > transform.position.x - transform.localScale.x &&
			    player.gameObject.transform.position.x < transform.position.x + transform.localScale.x &&
			    player.gameObject.transform.position.y >= transform.position.y) {
					float dist = 1 - ((player.gameObject.transform.position.y - transform.position.y) / distance);
					dist  = (dist < 0 ? 0 : dist);
					player.AddForce(new Vector2(0, upForce* (dist)));

					PlayerControls playerControls = (PlayerControls) player.GetComponent("PlayerControls");
					//PlayerControls doubleJump = (PlayerControls) doubleJump.GetComponent("PlayerControls");
				if (playerControls.getIsUmbrellaOpened()) {
					player.AddForce(new Vector2(0, upForce* (dist)));
				}
			}
		}
	}
}
