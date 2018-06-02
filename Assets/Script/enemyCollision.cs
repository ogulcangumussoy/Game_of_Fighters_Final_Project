using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyCollision : MonoBehaviour {

	//Burda amaç düşmanın vücudundaki box colliderlar ile çarpışma durumunu algılayabilmek
	//istenmeyen çarpışmalarda etkilenmemesi için Arissanın bacak ve kollarına "player" tagı verdik
	//Burada ise tag kontrolü yaparak çarpışmaları yakalaydık.
	void OnTriggerEnter(Collider other){

		if (other.tag == "Player") {
			enemyController.instance.enemyReact ();//Burada Arissa rakibine temas ettiğinde enemyReact metodunu çalıştıracak.
			Debug.Log ("Saldırı Arissa"); //Log bu şekilde basılabilir.
		}
	}


}
