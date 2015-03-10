using UnityEngine;
using System.Collections;

public class Demente : MonoBehaviour {

    public Transform Character;//posição do Player
    public Transform iniJump;
    public Transform finJump;
    public Transform iniLimJump;
    public Transform finLimJump;
    public bool jump;
    public bool nearPlayer;
    public bool agarrou;
	public float health;
	//public float respawnDelay = 1f;
    float speed;
	float move;
	bool facingRight = false;
	Animator anim;
	string toString;
	public GUIStyle guiStyle;

	// dependencia do player ter o PlayerControls para mostrar a "vida de agarramento"
	private PlayerControls playerControls;


	// Use this for initialization
    void Start()
    {
		playerControls = Character.GetComponent<PlayerControls>();
        speed = 3; //2
		health = 1;
			anim = GetComponent<Animator>();
    }

	/*
	void FixedUpdate(){

		if (move > 0 && !facingRight) {
			Flip ();
		} else if (move < 0 && facingRight) {
			Flip ();
		}

	}*/

	void Flip()	{
		
		facingRight = !facingRight;//verifica a direção
		//guarda a direção e escala
		Vector3 theScale = transform.localScale;
		
		//inverte a escala em x;
		theScale.x *= -1;
		
		//atualiza a direção invertida
		transform.localScale = theScale;
	}
	//INSERIDO 2

    void SeguePlayer(float speed)
    {
        //se a distancia for maior que 1.8 ele segue, se não, ele para
        if (Vector2.Distance(transform.position, Character.position) > 0.2) // >
        {
            //se a posição do player for menor que a do inimigo, ele adiciona uma força negativa
            //e vira a escala para o inimigo ficar de frente.
            if (Character.transform.position.x < transform.position.x) // <
            {
                transform.Translate(new Vector2(-speed * Time.deltaTime, 0));
                transform.localScale = new Vector3(-1, 1, 1);
				Flip ();
		
            }

            if (Character.transform.position.x > transform.position.x) // >
            {

                transform.Translate(new Vector2(speed * Time.deltaTime, 0));
                transform.localScale = new Vector3(1, 1, 1);
				Flip ();
				//animation.Play("Demente_Run");
            }

        }
    }
    bool NearPlayer()
    {
        //se a distancia for menor que 6 (10?), ele segue
		nearPlayer = (Vector3.Distance (transform.position, Character.position) < 10);
		return nearPlayer;

    }

    // Update is called once per frame
    void Update()
    {
		//Ele persegue
		anim.SetFloat("Speed", Mathf.Abs(move));
		Raycasting();

        //se está perto.
        if (NearPlayer())
        {
			move = 1;
            SeguePlayer(this.speed);
			//move = Input.GetAxis ("Horizontal");
        } else {
			move = 0;
		}



        if (jump)//se for pulo ele pula rs
        {
            //rigidbody2D.velocity = new Vector2(0, 10);
            rigidbody2D.velocity = new Vector2(0, 10);
        }

		if (playerControls.getGrabbedStruggle() < 0 && agarrou)
		{
			agarrou = false;
			anim.SetBool ("agarrou", false); //------------------
			anim.SetBool ("soltou", true);
			Destroy (gameObject);
		}

    }
    void Raycasting()
    {
        //cria uma linha amarela para saber onde está o transformer
        //inijump------------------>finjump
        Debug.DrawLine(iniJump.position, finJump.position, Color.yellow);

        Debug.DrawLine(iniLimJump.position, finLimJump.position, Color.yellow);

        //se a linha encontrar um obstáculo, inicia o pulo.
        if (Physics2D.Linecast(iniJump.position, finJump.position))
        {
            jump = true;
			//anim.SetBool ("Jump", true);
        }
        //se não encontrar ou se encontrar o limite do pulo, finaliza o pulo.
        if (!Physics2D.Linecast(iniJump.position, finJump.position) || Physics2D.Linecast(iniLimJump.position, finLimJump.position))
        {
            jump = false;
			//anim.SetBool ("Jump", false);
        }
    }

    //função para colisão, se o inimigo enconstar no Player, ele o agarra e tira dano;
    void OnCollisionEnter2D(Collision2D coll)
    {
		if (coll.gameObject.tag == "Player") {
			agarrou = true;
			//animation.Play("Demente_Run");
			anim.SetBool ("agarrou", true); //------------------
			anim.SetBool ("soltou", false);
			//Invoke("playerDraw", respawnDelay);
			//playerHealth--;
		}
	}
	void OnCollisionExit2D(Collision2D coll)
	{

    }
	// on collision sair -> agarrou = false

	void OnGUI() {
		//string toString;
		if (agarrou) {
			GUI.skin.textField = guiStyle;
			toString = GUI.TextField (new Rect (Screen.width / 2 - 150, Screen.height / 2 - 40, 300, 80), "Pressione as setas para soltar-se!"); //GUI.Label
			if (playerControls) {
				float barSize = (300 *  playerControls.getGrabbedStruggle() ) / playerControls.grabbedStruggleMax;
				GUI.Box(new Rect(Screen.width / 2 - 150, Screen.height / 2 - 60, barSize, 20), "");
			} else { print ("no player controls found"); }
		} else {

		}
	}

	/*void playerDraw(){
		playerControls.Minus();
	}*/

}
