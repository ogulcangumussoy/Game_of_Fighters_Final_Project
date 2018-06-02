using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//Burada butona basma ve bırakma olaylarımız olacak. 
//Basıldığında mvBack değişkenimizi true yapıp geri hareketi yapacağız aksi durumda varsayılan konuna dönmesi için 
//değişkeni false yapıp fighterController'da durumuna göre işlem yaptıracağız.
//Bunun için interrupt(kesme) mantığını kullanacağız.
public class walkBackController : MonoBehaviour,IPointerDownHandler,IPointerUpHandler {

	public void OnPointerDown(PointerEventData data){ //Butonun basılma ve basılı tutulma durumu

		fighterController.mvBack = true;

	}
	public void OnPointerUp(PointerEventData data){ //Butonun bırakılma durumu
		fighterController.mvBack = false;

	}

}
