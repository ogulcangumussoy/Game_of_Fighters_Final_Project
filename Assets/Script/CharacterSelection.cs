using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelection : MonoBehaviour {

	private GameObject[] characterList;
	private int index; //Seçili indis değerini tutacağız.


	private void Start(){

		//PlayerPrefte olan değeri alıyoruz ve index değerine atıyoruz.
		index = PlayerPrefs.GetInt ("CharacterSelected");

		characterList = new GameObject[transform.childCount];//Listenin boyutunu çocuk sayısına göre belirledik.
		//Listeyi gameobjectler ile dolduruyoruz.
		for(int i=0; i<transform.childCount;i++){ //for döngüsüsü ile içerisinde geziyoruz.
			characterList [i] = transform.GetChild(i).gameObject;
		}

		//Başlangıçta tüm karakterleri gizliyoruz.
		foreach (GameObject go in characterList)
			go.SetActive (false); //Görünmez yaptık.


		//indisteki karakteri aktif yapıyoruz.
		if(characterList[index]){
			characterList [index].SetActive (true);
		}

	}




	public void ToggleLeft(){ //Sola kayma işlemi
		//Mevcut seçili modeli pasif yapıyoruz..
		characterList [index].SetActive (false);
		index--; //index değerini bir düşürdük.
		if(index < 0) //Eğer index değeri 0'dan düşükse yani en baştayken basıldıysa sondaki modeli getiriyoruz.
			index=characterList.Length -1; //Liste uzunluğunun bir eksiği ile en büyük indisli değere eriştik.

		//Yeni modeli aktif hale getiriyoruz.
		characterList [index].SetActive (true);
		Debug.Log ("Toogle Left Çalıştı");
	}



	public void ToggleRight(){ //Sağa kayma işlemi
		//Mevcut seçili modeli pasif yapıyoruz..
		characterList [index].SetActive (false);

		index++; //index değerini bir düşürdük.
		if(index == characterList.Length) //Eğer index değeri 0'dan düşükse yani en baştayken basıldıysa sondaki modeli getiriyoruz.
			index=0; //Liste uzunluğunun bir eksiği ile en büyük indisli değere eriştik.

		//Yeni modeli aktif hale getiriyoruz.
		characterList [index].SetActive (true);

	}

	public void ConfirmButton(){

		PlayerPrefs.SetInt ("CharacterSelected", index); //Seçili indis değerini tutan bir değişken tanımladık.
		//SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex + 1);//Seçili sahneden bir sonraki sahneye geçiş yapar.
		//SceneManager.LoadScene ("single-player");
	}
}
