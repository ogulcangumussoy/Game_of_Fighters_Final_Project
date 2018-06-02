using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //Grafik elemanlarını kullanabilmek için dahil ediyoruz.


public class enemyController : MonoBehaviour {

	//Değişken tanımlamaları burada yapıldı.
	public Transform playerTransform; //Arissa'nın konum bilgilerine erişebilmek için
	private Vector3 direction;
	static Animator anim2;
	public int enemyHealth=100; //Düşmanın sağlık değerini tutacak bar için gerekli değişken
	public static enemyController instance; //düşmanı yönetebilmek için gerekli kontrollerden örnek oluşturduk.
	public Slider enemyHB;   //düşmanın sağlık seviyesinin tutulacağı çizelgeyi tanımladık.
	public BoxCollider[] c; //colliderlar için dizi oluşturduk. Dreyer'ın kol ve bacak box colliderlarını bunun içine attık.
	public AudioClip[] audioClip; //Dreyer'ın tüm ses dosyalarını burada barındıracağız.
	AudioSource audio; //AudioSource a erişebilmek için oluşturuldu.//
	private Vector3 enemyPosition;  //yeniden başlatırken bu konum bilgisi kullanılacak
	public GameObject[] characterList2;
	private int index; //Seçili indis değerini tutacağız.

	void Awake(){ //Tetikleyici Metot instance yoksa oluşturuyor.

		if (instance == null) { //objemizin değeri null ise referansını this ile veriyoruz.
			instance=this;
		}
	}

	//BoxCollider ların aktif/pasif olma durumlarını burada belirliyoruz.
	private void setAllBoxColliders(bool state){

		c [0].enabled = state; //ilk eleman(RightHand) in aktiflik durumu
		c [1].enabled=state; //ikinci eleman(RightFoot) in aktiflik durumu

	}
		

	// Use this for initialization
	void Start () {
		//PlayerPrefte olan değeri alıyoruz ve index değerine atıyoruz.
		index = PlayerPrefs.GetInt ("CharacterSelected");
		Transform secili = characterList2[index].GetComponent<Transform> ();
		playerTransform = secili;

		anim2 = GetComponent<Animator> (); //Değişkenimizi Animator componentine bağladık.
		setAllBoxColliders (false); //Oyun Başlangıcında bütün boxColliderları false yapmak için metodu parametre ile kullandık.
		audio= GetComponent<AudioSource> ();//AudioSource bölümüne eriştik artık arayüzdeki düzenlemeleri buradan yapabiliriz.
		enemyPosition=transform.position; //başlangıç konumunu aldık.
	}

	public void playAudio(int clip){

		audio.clip = audioClip [clip];  //Referans eşitleme dizinin seçilen indisindeki müzik dosyası oynatılacak
		audio.Play (); //Oynatma işlemi
	}

	
	// Update is called once per frame
	void Update () {

		if (anim2.GetCurrentAnimatorStateInfo (0).IsName ("fight_idleCopy")) {
			//Göz teması kurmaları için yapılan işlemler
			direction = playerTransform.position - this.transform.position; //aralarındaki pozisyon farkını bulduk.
			direction.y = 0; //Kaymaları engellemek için y ekseni değerini 0' sabitledik.
			this.transform.rotation = Quaternion.Slerp (this.transform.rotation, Quaternion.LookRotation (direction), 0.3f); //verilen değer doğrultusunda Quaternion yardımıyla döndürdük.
			setAllBoxColliders (false); // bütün boxColliderları false yapmak için metodu parametre ile kullandık.
		}

		Debug.Log (direction.magnitude); //Karakterler arasındaki farkı veriyor.

		//Aralarındaki uzaklığı inceleyerek şart veriyoruz.
		//GameController den gelen harekete izin verme değişkeninin değeri true ise walkFWD animasyonuna izin veriyoruz.



		if (direction.magnitude > 0.5f && gameController.allowMovement == true) {
			anim2.SetTrigger ("walkFWD");
			audio.Stop (); //13f den büyük olma durumunda ses efektini kapat
			setAllBoxColliders (false); // bütün boxColliderları false yapmak için metodu parametre ile kullandık.
		} else {
			anim2.ResetTrigger ("walkFWD");
		}


		//Yapay Zeka Tekme Atma Olayı
		if (direction.magnitude <= 0.5f && direction.magnitude > 0.42f && gameController.allowMovement == true) {

			setAllBoxColliders (true); // bütün boxColliderları true yapmak için metodu parametre ile kullandık.

			//Eğer hiçbir ses klibi oynatılmıyorsa ve anim2 roundhouse_kik 2 durumunda değilse ses klibi oynat ve kick durumuna geç
			if (!audio.isPlaying && !anim2.GetCurrentAnimatorStateInfo (0).IsName ("roundhouse_kick 2")) {
				playAudio (1);
				anim2.SetTrigger ("kick"); //Eğer aralarındaki mesafe 13f den küçük ve 7'den büyükse tekme atma animasyonu aktif oluyor.
			}

		}else{
			anim2.ResetTrigger ("kick");

			/*float yonelim = Random.Range (1, 3);

			if (yonelim == 1) { //1 gelirse geri gidip aralıktan çıksın
				anim2.SetTrigger ("walkBack");
				setAllBoxColliders (false); // bütün boxColliderları false yapmak için metodu parametre ile kullandık.
				audio.Stop(); //0 ve 2 arasındaki durumlarda ses efektlerini kapat
			}else if(yonelim==2){ //2 gelirse ileri gidip aralıktan çıksın
				anim2.SetTrigger ("walkFWD");
				setAllBoxColliders (false); // bütün boxColliderları false yapmak için metodu parametre ile kullandık.
				audio.Stop(); //0 ve 2 arasındaki durumlarda ses efektlerini kapat
			
			}*/

		}

		//Yapay Zeka Yumruk Atma Olayı
		if (direction.magnitude <= 0.32f && direction.magnitude > 0.24f && gameController.allowMovement == true) {

			setAllBoxColliders (true); // bütün boxColliderları true yapmak için metodu parametre ile kullandık.
			//Eğer hiçbir ses klibi oynatılmıyorsa ve anim2 cross_punch durumunda değilse ses klibi oynat ve kick durumuna geç
			if (!audio.isPlaying && !anim2.GetCurrentAnimatorStateInfo (0).IsName ("cross_punch")) {
				playAudio (0);
				anim2.SetTrigger ("punch"); //Eğer aralarındaki mesafe 6f den küçükse yumruk atma animasyonu aktif oluyor.
			}

		}else{
			anim2.ResetTrigger ("punch");
		}

		if (direction.magnitude > 0f && direction.magnitude <= 0.24f && gameController.allowMovement == true) { //Yakınlık 0 ve 2 arasında uzaklaşmasını sağlıyoruz.
			anim2.SetTrigger ("walkBack");
			setAllBoxColliders (false); // bütün boxColliderları false yapmak için metodu parametre ile kullandık.
			audio.Stop(); //0 ve 2 arasındaki durumlarda ses efektlerini kapat
		} else {
			anim2.ResetTrigger ("walkBack");
		}


	}


	public void enemyReact(){

		enemyHealth = enemyHealth -10; //Her vuruşta bu metot çalışıyor enemyCollisiondan çağırılmıştı.

		enemyHB.value=enemyHealth; //Arayüzdeki slidera değerini aktardık.
		if (enemyHealth < 10) { //eğer sağlık seviyesi 10'un aşağısına inmişse nakavt metodunu çağırıyoruz.
			enemyKnockout ();
		} else {
			anim2.ResetTrigger ("idle");
			anim2.SetTrigger ("react");
			//playAudio (2);
		}

	}

	public void enemyKnockout(){ //Nakavt olma durumu
		gameController.allowMovement=false; //hareket iznini kaldırdık.
		enemyHealth=100;
		anim2.SetTrigger ("knockout");
		gameController.instance.scorePlayer (); //Arissanın puanını arttırması için metot çağırıldı.
		gameController.instance.onScreenPoints(); //Puan değişiminin ekrana yansıması sağlandı.
		gameController.instance.rounds (); //round hesaplaması metodu


		if (gameController.playerScore == 2) {
			gameController.instance.doReset (); //Eğer Arissa 2 puan olduysa resetleme işlemi yapıyoruz.
		}else{ //round geçisi olduysa
			StartCoroutine (resetCharacters ()); //Karakterleri sıfırlama metodu
		}
	}

	IEnumerator resetCharacters(){
		yield return new WaitForSeconds (4);
		enemyHB.value = 100; //Arayüzdeki sağlık çubuğunu 100% yapar.
		//Burada amaç karakter yeniden oluşturulurken doğru konumda oluşturmak bunun içinde Unity'nin
		//oyun başladığı anda oluştrduğu kopya modelin konumunu almamız gerekiyor.

		GameObject[] theclone = GameObject.FindGameObjectsWithTag ("Enemy");
		Transform t = theclone [1].GetComponent<Transform> ();
		t.position = enemyPosition;
		t.position = new Vector3 (t.position.x, 0, t.position.z); 
		gameController.allowMovement = true;
	}


}
