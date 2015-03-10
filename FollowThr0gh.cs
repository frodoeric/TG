using UnityEngine;
using System.Collections;

public class FollowThr0gh : MonoBehaviour {

	/// <summary>
	/// Objeto a seguir
	/// </summary>
	public GameObject target;
	/// <summary>
	/// Distancia do target a manter
	/// </summary>
	public Vector3 distance;
	/// <summary>
	/// Se verdadeiro, utiliza a distancia real entre target e si mesmo.
	/// </summary>
	public bool autoDistance = false;
	/// <summary>
	/// Porcentagem de distancia relativa ao target
	/// </summary>
	public Vector3 ratio = new Vector3(1,1,1);
	// definem se vai seguir no eixo
	public bool followX = true;
	public bool followY = true;
	public bool followZ = true;

	// Use this for initialization
	void Start () {
		if (autoDistance && target) {
			distance = transform.position - target.transform.position;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (target) {
			transform.position = new Vector3( // se segue o eixo, posicao = posicao do alvo + (distancia * ratio). senao, nao. =D
					followX ? target.transform.position.x + distance.x * ratio.x : transform.position.x,
					followY ? target.transform.position.y + distance.y * ratio.y : transform.position.y,
					followZ ? target.transform.position.z + distance.z * ratio.z : transform.position.z
				);
		}
	}
}
