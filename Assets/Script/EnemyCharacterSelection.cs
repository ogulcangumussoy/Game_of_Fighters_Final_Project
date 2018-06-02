using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyCharacterSelection : MonoBehaviour {

	private GameObject[] enemycharacterList;
	private int index; //Seçili indis değerini tutacağız.

	private void Start(){

		//PlayerPrefte olan değeri alıyoruz ve index değerine atıyoruz.
		index = PlayerPrefs.GetInt ("EnemyCharacterSelected");

		enemycharacterList = new GameObject[transform.childCount];//Listenin boyutunu çocuk sayısına göre belirledik.
		//Listeyi gameobjectler ile dolduruyoruz.
		for(int i=0; i<transform.childCount;i++){ //for döngüsüsü ile içerisinde geziyoruz.
			enemycharacterList [i] = transform.GetChild(i).gameObject;
		}

		//Başlangıçta tüm karakterleri gizliyoruz.
		foreach (GameObject go in enemycharacterList)
			go.SetActive (false); //Görünmez yaptık.


		//indisteki karakteri aktif yapıyoruz.
		if(enemycharacterList[index]){
			enemycharacterList [index].SetActive (true);
		}

	}


	public void ToggleLeft(){ //Sola kayma işlemi
		//Mevcut seçili modeli pasif yapıyoruz..
		enemycharacterList [index].SetActive (false);
		index--; //index değerini bir düşürdük.
		if(index < 0) //Eğer index değeri 0'dan düşükse yani en baştayken basıldıysa sondaki modeli getiriyoruz.
			index=enemycharacterList.Length -1; //Liste uzunluğunun bir eksiği ile en büyük indisli değere eriştik.

		//Yeni modeli aktif hale getiriyoruz.
		enemycharacterList [index].SetActive (true);
	}



	public void ToggleRight(){ //Sağa kayma işlemi
		//Mevcut seçili modeli pasif yapıyoruz..
		enemycharacterList [index].SetActive (false);

		index++; //index değerini bir düşürdük.
		if(index == enemycharacterList.Length) //Eğer index değeri 0'dan düşükse yani en baştayken basıldıysa sondaki modeli getiriyoruz.
			index=0; //Liste uzunluğunun bir eksiği ile en büyük indisli değere eriştik.

		//Yeni modeli aktif hale getiriyoruz.
		enemycharacterList [index].SetActive (true);

	}

	public void ConfirmButton(){

		PlayerPrefs.SetInt ("EnemyCharacterSelected", index); //Seçili indis değerini tutan bir değişken tanımladık.
		//SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex + 1);//Seçili sahneden bir sonraki sahneye geçiş yapar.
		SceneManager.LoadScene ("single-player");
	}
}
