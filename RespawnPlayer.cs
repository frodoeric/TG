using UnityEngine;
using System.Collections;

public class RespawnPlayer : MonoBehaviour {

	public GameObject player; // objeto a ser respawnado quando colidir comigo
	//public GameObject enemy;
	private Vector3 respawnPosition; // local de respawn
	public float respawnDelay = 1f; // tempo que demorara entre collidir com o enemy e respawnar
	private HealthPoints healthPoints; //HP
	/*
	private MainScript jsScript;

	void Awake(){
		jsScript = this.GetComponent<MainScript>();
	}*/

	void Start() {
		UpdateRespawnPosition ();
	}
		
	void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == player.tag) {
			Invoke("Respawn", respawnDelay);
		}
	}

	/** funcao Update Respawn Position
	 *  atualiza posicao de respawn
	 *  pode ser usada com checkpoint
	 */
	public void UpdateRespawnPosition() {
		if (player) {
			respawnPosition = player.transform.position;
			//MainScript.Health = maxHealth;
		}

		/*if (enemy) {
			respawnPosition = enemy.transform.position;
		}*/
	}
    
	/**
	 * funcao Respawn
	 * transporta o player para a atual posicao de respawn
	 */
	public void Respawn() {
		if (player) {
			player.transform.position = respawnPosition;

			//platform.transform.position = respawnPlatform;
		}

		/*if (enemy) {
			enemy.transform.position = respawnPosition;
		}*/

		print ("LOLOL");
	}
}
