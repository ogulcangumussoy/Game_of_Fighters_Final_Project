using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Vuforia; //tracking event handler yani olayların izlenmesi ve yönetimi için tanımladık.
using UnityEngine.SceneManagement;

public class gameController : MonoBehaviour {

	public static gameController instance; //Diğer dosyalardan bu sınfıfa erişim sağlayabilmek için bir obje oluşturduk.
	public static bool allowMovement = false; //Hareket izin verip vermeme durumunu için bir değişken oluşturduk ve başlangıç değerine false verdik.

	//Arayüzde bulunan nesnelere erişebilmek için burada tanımladık. GameObject Hiyerarşide bunlarla aynı seviyede bulunduğu için erişebiliyoruz.
	public GameObject flashButton;
	public GameObject cameraButton;
	public GameObject playerScoreOnScreen;
	public GameObject enemyScoreOnScreen;
	public GameObject backButton;
	public GameObject fwdButton;
	public GameObject punchButton;
	public GameObject kickButton;
	public GameObject BaslangicMetni;
	public Text oyunAsamalari;
	private bool played321=false; //Geriye doğru sayıp oyunu başlatmayı kontrol etmek için bool bir değişken tanımladık ve false ile başlattık.
	public AudioClip[] audioClip;
	AudioSource audio;//
	public static int playerScore=0;//Arissanın puan değeri
	public static int enemyScore=0; //Dreyer'in puan değeri
	public GameObject[] points; //Arayüzde bulunan puan değerlerini tutan elementleri içine alacağız.
	public static int round =0; //round sayısını tutacak değişken 0'dan başlatıldı.


	void Awake(){ //Bu metot ile oluşturduğumuz objenin boş olup olmadığını kontrol ediyoruz eğer boş ise this yapısı ile eşitliyoruz.
		if (instance == null) {
			instance = this;
		}
	}


	// Use this for initialization
	void Start () {
		audio = GetComponent<AudioSource> (); //AudioSource kompanentine erişim sağladık.

	}

	//Ses dosyalarını oynatmak için gerekli
	private void playAudioTrack(int clip){
		audio.clip = audioClip [clip]; //Dizi içerisinden gelen indise göre değişkene aktarılır.
		audio.Play ();//Oynatma işlemi
	}

	public void scorePlayer(){ //Arissanın puanını hesaplayacak metot //Bu metodu dışarıdan çağıracağız her çağırmada değeri 1 artacak.
		playerScore++;

	}

	public void scoreEnemy(){ //Dreyerin puanını hesaplayacak metot //Bu metodu dışarıdan çağıracağız her çağırmada değeri 1 artacak.
		enemyScore++;

	}

	public void refreshGame(){
			DefaultTrackableEventHandler.trueFalse = false;
			allowMovement = false;
			playerScore = 0;
			enemyScore = 0;
			//SceneManager.LoadScene (SceneManager.GetActiveScene().buildIndex -2);//Aktif sahnenin değerinin bir eksiğini verdik. Build Settings
			SceneManager.LoadScene ("menu");
	}


	//Resetleme işlemi Nakavt olma durumu ve iki oyuncudan birinin 2 puana ulaşma durumu
	public void doReset(){
		/*
		//Oyun sonucunun kazanma va kaybetme durumuna göre ses efektlerinin eklenmesi
		if (playerScore == 2) { //Kazanma durumu
			playAudioTrack (6); 	//You Won Ses Efektini oynatır.
		} else { //Kaybetme durumu
			playAudioTrack (5);		//You Lose Ses Efektini oynatır.
		}
		fighterController.instance.playerHB.value = 100; //Oyuncumuzun sağlık çubuğunu 100% yaptık
		fighterController.instance.health=100; //Oyuncumuzun sağlık seviyesini 100 yaptık

		enemyController.instance.enemyHB.value = 100; //Düşmanın sağlık çubuğunu 100% yaptık
		enemyController.instance.enemyHealth=100; //Düşmanın sağlık seviyesini 100 yaptık

		playerScore = 0;
		enemyScore = 0;*/
		StartCoroutine (oyunAsamalariDegistir (4));
		refreshGame ();
		//StartCoroutine (restartGame ()); //restartGame metodu sıralı metot yardımıyla çağırılı (ıEnumerator)
	}

	IEnumerator restartGame(){
		yield return new WaitForSeconds (4.5f); //4.5 saniye bekliyor.
		points[0].SetActive (false);
		points[1].SetActive (false);
		points[2].SetActive (false);
		points[3].SetActive (false);
		allowMovement=true; //Dreyer hareket izni veriyoruz.
		StartCoroutine (restartRoundAudio ()); //round 1 ses efekti için metoda gidiyor
	}
	IEnumerator restartRoundAudio(){ //round 1 ses efekti 
		yield return new WaitForSeconds (2); //2 saniye bekletir.
		playAudioTrack (0); //Round one ses efekti çalar
	}
		

	
	// Update is called once per frame
	void Update () {


		if (played321 == false) { //oyununu henüz başlamamış olması durumu

			if (DefaultTrackableEventHandler.trueFalse == true) { //Kamera ile bir oyun alanı tanıtıldığında true oluyor. Bu metodu kullanarak oyunu başlatabiliriz. 

				//Oyun Başladığında ekranda bulunan araçların aktif/pasif olmasını kod ile gerçekleştirdik
				flashButton.SetActive (false);
				//BaslangicMetni.SetActive (false);
				oyunAsamalari.text = "OYUN BAŞLIYOR";
				cameraButton.SetActive (false);
				played321 = true;//oyun Başladı anlamına geliyor.

				/*
					-------- Olayların işleyişi şu şekilde olacak: --------------
					*Ugulama açılır 
					*Kullanıcı uygun bir oyun alan seçer ve Kamera tuşuna basar.
					*Ardından IEnumerator yapısı ile tanımladığımız şekilde sıralı olarak işlemler gerçekleşir.
				*/

				StartCoroutine (round1 ()); //Sıralı işlemler için IEnumerator kullanırız StartCoroutine() ile de IEnumerator elemanlarını sıralı şekilde çalıştırabilriz.
			}

		}
		
	}

	IEnumerator round1(){ //IEnumerator yapısı ile sıralı fonksiyonlar yazıyoruz round1,prepareYourSelf şeklinde ilerleyeceğiz.

		yield return new WaitForSeconds (0);
		playAudioTrack (0);
		StartCoroutine(prepareYourself());
	}
	IEnumerator prepareYourself(){
		yield return new WaitForSeconds(1.2f); //Geçişler için süre belirliyoruz.
		playAudioTrack(1);
		StartCoroutine (start321 ());
	}

	IEnumerator start321(){
		yield return new WaitForSeconds (2f);
		oyunAsamalari.text = "1. RAUNT";
		playAudioTrack (2);
		StartCoroutine (allowPlayerMovement ());
	}

	IEnumerator allowPlayerMovement(){
		yield return new WaitForSeconds (5f);
		playerScoreOnScreen.SetActive (true);
		enemyScoreOnScreen.SetActive (true);
		backButton.SetActive (true);
		BaslangicMetni.SetActive (false);
		fwdButton.SetActive (true);
		punchButton.SetActive (true);
		kickButton.SetActive (true);
		allowMovement = true;//Hareket iznini bu değişken tutuyordu true yapmamız ile birlikte dreyer hareket edebilecek.

	}

	public void onScreenPoints(){ //Ekranda skorların gösterilmesini sağlayan metot

		if (playerScore == 1) { //puan 1 ise
			points[0].SetActive (true);
		} else if(playerScore==2){ //puan 2 ise
			points [1].SetActive (true);
		}

		if (enemyScore == 1) { //puan 1 ise
			points[2].SetActive (true);
		} else if(enemyScore==2){ //puan 2 ise
			points [3].SetActive (true);
		}

	}

	public void rounds(){ //Round ile ilgili işlemler		
		round = playerScore + enemyScore; //toplam skora eşitledik.

		if (round == 1) {//2.round
			playAudioTrack (3);//Round 2 ses efektini oynattık.
			StartCoroutine (oyunAsamalariDegistir (2));
		}
		if (round == 2 && playerScore!=2 && enemyScore!=2) {
			playAudioTrack (4);//Final round ses efektini oynattık.
			StartCoroutine (oyunAsamalariDegistir (3));
		}
	}

	IEnumerator oyunAsamalariDegistir(int asama){

		if (asama == 3) {
			oyunAsamalari.text = "FINAL RAUNT";
		} else if (asama == 4) {
			if (playerScore == 2) { //Kazanma durumu
				oyunAsamalari.text = "SEN KAZANDIN";
				playAudioTrack (6); 	//You Won Ses Efektini oynatır.
			} else { //Kaybetme durumu
				oyunAsamalari.text = "RAKiP KAZANDI";
				playAudioTrack (5);		//You Lose Ses Efektini oynatır.
			}

		}else {
			oyunAsamalari.text = asama+ ". RAUNT";
		}



		BaslangicMetni.SetActive (true);
		if (asama == 4) {
			yield return new WaitForSeconds(5); 
		} else {
			yield return new WaitForSeconds(1.2f); 
		}
		BaslangicMetni.SetActive (false);
	}



}
