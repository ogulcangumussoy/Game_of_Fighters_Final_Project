using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerCollision : MonoBehaviour {

	//Burda amaç Arissanın vücudundaki box colliderlar ile çarpışma durumunu algılayabilmek
	//istenmeyen çarpışmalarda etkilenmemesi için Dreyerin bacak ve kollarına "player" tagı verdik
	//Burada ise tag kontrolü yaparak çarpışmaları yakalaydık.
	void OnTriggerEnter(Collider other){

		if (other.tag == "Enemy") {

			fighterController.instance.react ();
			//enemyController.instance.enemyReact ();//Burada Dreyer rakibine temas ettiğinde enemyReact metodunu çalıştıracak.
			Debug.Log ("Saldırı Dreyer"); //Log bu şekilde basılabilir.
		}
	}
}
