using UnityEngine;
using System.Collections;

public class PlayerStamina: MonoBehaviour {

	public int maxStamina = 100;
	public float curStamina; // setado no Start();
	private float staminaBarLenght;
	private bool isDefending = false;
	private bool isAttacking = false;
	private PlayerControls playerControls; // obj referenciando o CharacterMotor para alterar a MaxSpeed quando defende
	private float characterSpeed;
	//private EnemyAttack enemyAttack;

	public int moreStamina = 5;

	public float defendingDebuff = 4;
	public float atkStamina = 12.5F;
	public float defStamina = 5.0F;
	public float moveStamina = 0.1F;

	public Collision2D coll;
	//Animator anim;
	//public bool defending = false;


	// Use this for initialization
	void Start () {
		//parei aqui------------------------------------------
		staminaBarLenght = Screen.width / 2;
		curStamina = maxStamina;
		playerControls = GetComponent<PlayerControls>();
		characterSpeed = playerControls.maxSpeed;
		//anim = playerControls.anim;
	}
	
	// Update is called once per frame
	void Update () {

		if (curStamina > atkStamina + 1) {
			if (Input.GetButtonDown("Fire1")) { // quando atacar
				curStamina -= atkStamina;
				// fazer coisas
			}
		}

		// perde e limita stamina quando ataca
		if (curStamina > atkStamina + 1) { // se tem stamina
			if (Input.GetButtonDown("Fire2")) { // quando defender
				curStamina -= atkStamina;
				// fazer coisas
			}
		}

		//Defesa instanciada para diminuir a velocidade da barra
		isAttacking = Input.GetButton ("Fire1"); 
		isDefending = Input.GetButton ("Fire2");

		if (isDefending) {
			playerControls.moveForce = characterSpeed / defendingDebuff;
			playerControls.maxSpeed = characterSpeed / defendingDebuff;
			//playerControls.movement.maxForwardSpeed = characterSpeed / defendingDebuff;
			//playerControls.movement.maxSidewaysSpeed = characterSpeed / defendingDebuff;
			//playerControls.movement.maxBackwardsSpeed = characterSpeed / defendingDebuff;
			//defending = true;
		}
		else {
			playerControls.moveForce = characterSpeed;
			playerControls.maxSpeed = characterSpeed;
			//playerControls.movement.maxForwardSpeed = characterSpeed;
			//playerControls.movement.maxSidewaysSpeed = characterSpeed;
			//playerControls.movement.maxBackwardsSpeed = characterSpeed;
			//defending = false;
		}



		AddjustCurrentStamina (0);
	}

	void OnGUI() {

		GUI.Label (new Rect(20, 517, 100, 30), "Defesa");

		GUI.color = Color.cyan;
		//GUI.Box(new Rect(10, 10, Screen.width / 2 / (maxHealth / curHealth), 20), curHealth + "/" + maxHealth);
		GUI.Box(new Rect(18, 550, staminaBarLenght, 20), (int) curStamina + "/" + maxStamina);
	}

	

	public void AddjustCurrentStamina(int adj){

		if (curStamina > maxStamina)
			curStamina = maxStamina;
		else
			curStamina += (moreStamina / (isDefending ? defendingDebuff : 1) ) * Time.deltaTime;



		if (maxStamina < 1)
			maxStamina = 1;

		staminaBarLenght = (Screen.width / 4) * (curStamina /(float)maxStamina);
	}

	/*
	void OnCollisionEnter2D(Collision2D coll)
	{
		if (coll.gameObject.tag == "Enemy") {
			if (isAttacking) {
				//yield return new WaitForSeconds (1);
				Destroy (coll.gameObject);
			}
		}
	}*/
		
}
