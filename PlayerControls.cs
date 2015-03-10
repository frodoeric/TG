using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class PlayerControls : MonoBehaviour {

	public float jumpForce = 700f;
	public float moveForce = 13f;
	public float maxSpeed = 11f;
	public float respawnDelay = 1f;
	bool facingRight = true; //direçao
	public AudioClip playJump;

	//Umbrella
	private bool isUmbrellaOpened = false;
	public float moveFriction = 15f;
	public float umbrellaFriction = 5.5f;
	public float delayToOpenUmbrella = 1f;
	private float currentUmbrellaOpenDelay = 0f;
	public float dashDelay = 0.15f;
	private float dashDelayCounter = 0f;
	public float dashForce = 1500f;
	public bool trueFriction = false;
	public GameObject Cure;

	//DoubleJump
	public bool doubleJump = true;
	bool jump = false;

	// attack
	private float attackTime = 0.5f;
	private float attackTimeCounter = 0f;
	private bool isAttacking = false;

	private HealthPoints healthPoints;//Objeto do script de HP
	public int playerHealth; //O proprio HP
	public GameObject Enemy; //Demente
	private Demente demente; //Objeto de script do Demente

	//Empreendedorismo
	//private GUITexture restartButton;

	public bool getIsUmbrellaOpened() {
			return isUmbrellaOpened;
	}

	//referencia o animator
	public Animator anim; //**********************************

	//flag que indica se o personagem esta no chao
	bool grounded = false;

	//verifica posicionamento em relacao ao chao
	public Transform groundCheck;
	float groundRadius = 0.2f;
	public LayerMask whatIsGround;

	// controle de agarrado - demente
	private float grabbedPosition; // posiçao em que foi agarrado
	private bool isGrabbed; // controle se esta agarrado
	public float grabbedRestraint = 0.05f; // quanto o player pode se mexer da posiçao em que foi agarrado
	private float grabbedStruggle = 0f; // vida de agarramento (controle) - quanto o demente aguenta ate te soltar
	public float grabbedStruggleMax = 100f; // maximo de vida de agarramento
	public float grabbedStruggleRegen = 5f; // quanto regenera da vida de agarramento por segundo
	public float grabbedStruggleDecay = 17f; // quanto abaixa da vida de agarramento por tentativa de sair
	public float grabbedDamageDelay = 1f; // tempo em seconds
	private float grabbedDamageDelayCounter = 0f; // contador para dar o dano
	public float getGrabbedStruggle() { return grabbedStruggle; }
	
	void FixedUpdate() {
		//gera uma circunferencia com funcionalidade igual semelhante
		//parametro: (posicao, raio e layer) e retorna bool
		grounded = Physics2D.OverlapCircle (groundCheck.position, groundRadius, whatIsGround);

		//seta parametro a ser criado ground, true or false
		anim.SetBool ("Ground", grounded);

		//guarda a direçao
		float move = Input.GetAxis ("Horizontal");

		//Movimenta o rigidbody 2d - se nao houver input, adiciona forca contraria ao movimento * fricçao.
		if (move != 0 && Mathf.Abs (rigidbody2D.velocity.x) < maxSpeed) {
				rigidbody2D.AddForce(new Vector2(move * (moveForce - Mathf.Abs(rigidbody2D.velocity.x)), 0));
		} else {
			rigidbody2D.AddForce(new Vector2(-(rigidbody2D.velocity.x) * moveFriction, 0)); 
			//Quando o persona estiver sobre uma poça d'agua, alterar o moveFriction para 3!
		}

		//Procura o parametro float e seta um valor
		//abs valor absoluto de uma variavel //**********************************
		anim.SetFloat ("Speed", Mathf.Abs(move)); //**********************************

		anim.SetFloat ("vSpeed", rigidbody2D.velocity.y);

		if (move > 0 && !facingRight) {
			Flip ();
		} else if (move < 0 && facingRight) {
			Flip ();
		}

		isUmbrellaOpened = (rigidbody2D.velocity.y < -0.3 && currentUmbrellaOpenDelay <= 0);

		//Forca aplicada ao guarda-chuva aberto
		if (isUmbrellaOpened) {
			rigidbody2D.AddForce (new Vector2(0, rigidbody2D.velocity.y * -umbrellaFriction ));
		}
//		print (rigidbody2D.velocity.x);

		if (grounded) { //DoubleJump
			doubleJump = false;
			jump = false; //retira isso e ele pula para sempre
		}
	}

	void Flip()	{

		facingRight = !facingRight;//verifica a direção
		//guarda a direção e escala
		Vector3 theScale = transform.localScale;

		//inverte a escala em x;
		theScale.x *= -1;

		//atualiza a direção invertida
		transform.localScale = theScale;
	}



	// Use this for initialization
	void Start () {

		//pegando o componente Animator para atribur em anim; //**********************************
		anim = GetComponent<Animator>(); //**********************************

		//Empreendedorismo
		//restartButton = GameObject.FindWithTag("RestartButton").guiTexture;
		//restartButton.enabled = false;

		//Instanciando o HP para o salto
		healthPoints = GameObject.Find ("HealthPoints").GetComponent<HealthPoints>();
		//playerHealth = healthPoints.health;
		playerHealth = 11;

	}

	void OnTriggerEnter2D(Collider2D hit) {
		if (hit.gameObject.tag == "Cure") {
			playerHealth++;
			Destroy (hit.gameObject);
			//Destroy(gameObject); // ele morre
		}

		if (hit.gameObject.tag == "Destroy") {
			Invoke("Death", respawnDelay);
		}

		//fricçao na poça dqagua
		if (hit.gameObject.tag == "Water") {
			trueFriction = true;
		}

		//if(hit.gameObject.tag)
	}

	void OnTriggerExit2D() {
		trueFriction = false;
	}

	/*void OnCollisionEnter2D(Collider2D lit){

		//fricçao na poça dqagua
		if (lit.gameObject.tag == "Water") {
			trueFriction = true;
		} else {
			trueFriction = false;
		}

	}*/
	
	// Update is called once per frame
	void Update () {

		//Para controle
		//print ("horizontal axis: " + Input.GetAxis ("Horizontal") + ", getBtnDn Hor: " + Input.GetButtonDown ("xboxB"));
		
		if(trueFriction)
			moveFriction = 3f;
		else
			moveFriction = 10f;

		//Salto pressionado
		if ((grounded || !doubleJump) && Input.GetButtonDown ("Jump")) {  //Input.GetKeyDown (KeyCode.Space)) {
			audio.PlayOneShot(playJump, 1.0F);
			audio.loop = false;
			currentUmbrellaOpenDelay = delayToOpenUmbrella;
			anim.SetBool("Ground", false);
			anim.SetBool("Umbrella", true);
			rigidbody2D.AddForce (new Vector2(0, jumpForce));
			//jump = true;
			//Debug.Log(playerHealth);

			if(!grounded){
				doubleJump = true;
				//playerHealth--; //--------------------------------DIMINUI O HP!
				audio.PlayOneShot(playJump, 1.0F);
				audio.loop = false;
			}
		}

		bool Umbrella = false;

		//Quando estiver fora do chao, pressionando o espaço
		if (!grounded && Input.GetButton ("Jump")) { 		//Input.GetKey (KeyCode.Space) || Input.GetKey(KeyCode.W)) {
			currentUmbrellaOpenDelay -= Time.deltaTime;
			currentUmbrellaOpenDelay = (currentUmbrellaOpenDelay < 0 ? 0 : currentUmbrellaOpenDelay);
		} else if (!grounded && !Input.GetButtonDown ("Jump")) {		//!Input.GetKeyDown (KeyCode.Space) || !Input.GetKeyDown(KeyCode.W)) {
			currentUmbrellaOpenDelay = delayToOpenUmbrella;
			anim.SetBool("Umbrella", false);
		}

		/*
		//POWER: DOUBLE JUMP
		if ((grounded || !doubleJump) && jump) { 
			// Add a vertical force to the player.
			anim.SetBool("Ground", false);
			rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, 0);
			rigidbody2D.AddForce(new Vector2(0f, jumpForce));
			
			if(!grounded)
				doubleJump = true;
		}*/

		//POWER: DASH
		bool isSidePressed = Input.GetButtonDown("Horizontal");//(Mathf.Abs(Input.GetAxis("Horizontal")) > 0.7f); //Input.GetButtonDown ("Left") || Input.GetButtonDown ("Right")); Input.GetButtonDown("xboxB")
		if (isSidePressed && !isGrabbed) {
			if (dashDelayCounter <= 0) {
				dashDelayCounter = dashDelay;
			} else {
				if (isSidePressed) {
					rigidbody2D.AddForce (new Vector2(dashForce * Input.GetAxis ("Horizontal"), 0));
				}
			}
		}

		if (dashDelayCounter > 0) {
			dashDelayCounter -= Time.deltaTime;
		}
		
		healthPoints.SetHealth(playerHealth);
		if (playerHealth <= 1) {
			RespawnPlayer respawnPlayer  = GameObject.Find ("Destroyer").GetComponent<RespawnPlayer>();
			if (!respawnPlayer) throw new UnityException("Cade o respawn Ç.Ç");
			respawnPlayer.Respawn();
			Invoke("Death", respawnDelay);
		}

		/*bool isAttackPressed = (Input.GetButtonDown("Attack");
		if (Input.GetKey (KeyCode.UpArrow)){
			jump = true;
			print(rigidbody2D.velocity.y);
		}*/

		//  Movement restrain
		if (isGrabbed) {

			if (transform.position.x > grabbedPosition + grabbedRestraint) {
				transform.position = new Vector3(
					grabbedPosition + grabbedRestraint,
					transform.position.y,
					transform.position.z
					);
			}
			if (transform.position.x < grabbedPosition - grabbedRestraint) {
				transform.position = new Vector3(
					grabbedPosition - grabbedRestraint,
					transform.position.y,
					transform.position.z
					);
			}
			if (grabbedStruggle < grabbedStruggleMax && grabbedStruggle > 5) {
				grabbedStruggle += grabbedStruggleRegen;
			}
			if (Input.GetButtonDown("Horizontal")){ //(Input.GetButtonDown("Left") || Input.GetButtonDown("Right")) {
				grabbedStruggle -= grabbedStruggleDecay;
			}
			if (grabbedStruggle < 0) {
				isGrabbed = false;
				grabbedStruggle = -1;
				//Invoke("DeathEnemy", respawnDelay);
			}
			//print(grabbedStruggle);

			if ( (grabbedDamageDelayCounter-=Time.deltaTime) <= 0) {
				Minus();
				grabbedDamageDelayCounter = grabbedDamageDelay;
			}
		}

		//Defesa
		bool isDefense = (Input.GetButton ("Fire2"));

		anim.SetBool ("Defense", isDefense && !isSidePressed);
		anim.SetBool("DefWalk", isSidePressed && isDefense);

		//Ataque
		bool Attack = (Input.GetButton("Fire1"));
		if (Attack && !isAttacking) {
			attackTimeCounter = attackTime;
		}
		isAttacking = ((attackTimeCounter-=Time.deltaTime) >= 0);
		anim.SetBool ("Attack", isAttacking);
	}

	/*
	public void Attack(){
		anim.SetBool("Attack", false);
	}
	*/
	
	public void Death(){
		//(Application.loadedLevel);
		playerHealth = 11;

	}

	public void Minus(){
		playerHealth--;
	}

	void OnCollisionEnter2D(Collision2D coll)
	{

		if (coll.gameObject.tag == "Enemy" && !isGrabbed){
			grabbedPosition = transform.position.x;
			isGrabbed = true;
			grabbedStruggle = grabbedStruggleMax;
			grabbedDamageDelayCounter = grabbedDamageDelay;
		}		
		if (coll.collider.gameObject.tag == "Enemy" && isAttacking) {
			Destroy (coll.gameObject);
			isGrabbed = false;
			grabbedStruggle = -1;
		}
	}

	void OnCollisionStay2D(Collision2D coll)
	{
		if (coll.gameObject.tag == "Enemy" && isGrabbed)
		{
			if (grabbedStruggle < 10) {
				//coll.gameObject.transform.rigidbody2D.AddForce(new Vector2(0, 100));
				coll.gameObject.transform.position = new Vector3(0, 13, 0) + coll.gameObject.transform.position;
			}
		}
	}

	/*public void DeathEnemy(){
		void onCollisionEnter2D(Collision2D collider)
		{
			if(collider.gameObject.tag == "Enemy"){
				Destroy(enemyDemente); //mata o inimigo
			}
		}
	}*/
	
}