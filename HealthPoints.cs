using UnityEngine;
using System.Collections;

/// <summary>
/// Health points class. object that has health MUST call this script, on the object it is.
/// ALL textures from each HP must be referenced.
/// remembering that this ONLY DRAWS the health, doesn't controls it.
/// </summary>
public class HealthPoints : MonoBehaviour {

	public int health = 0; //0
	public int maxHealth = 11;
	public int quantity;
	public Texture2D[] hpTextures;

	public void Start() {
		SetHealth (11);
		//Debug.Log (health);
	}

	public void AddHealth(int quantity) {
		health += quantity;
		if (health < 1) health = 1;
		//if (health == 11) health = 11;
		DrawHealth ();
	}

	public void SetHealth(int quantity) {
		health = quantity;
		if (health < 1) health = 1;
		//if (health == 11) health = 11;
		DrawHealth ();
	}

	private void DrawHealth() {
		transform.guiTexture.texture = hpTextures[health-1];
	}

	/*
	public void Update() {
		MediumHealth (0);
	}

	public void MediumHealth(int hp){
		if (health > maxHealth)
			health = maxHealth;
		//else
			//curStamina += (moreStamina / (isDefending ? defendingDebuff : 1) ) * Time.deltaTime;
		
		if (maxHealth < 1)
			maxHealth = 1;
	}


	 //NAOVAIUSAR
	if(MainScript.Health == 10){
		image.texture = HP01;
	}
	*/
}