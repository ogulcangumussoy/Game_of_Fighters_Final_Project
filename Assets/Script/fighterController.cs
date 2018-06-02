using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //Grafik elemanlarını kullanabilmek için dahil ediyoruz.

public class fighterController : MonoBehaviour {

	public Transform enemyTarget; //Düşmanın konumunu tutmak için
	static Animator anim;//Animasyon işlemleri için dahil ettik.
	public static bool mvBack=false; //Geri hareketinin başlangıç değerini false verdik.
	public static bool mvFWD=false;
	public static bool uzaklas=false;
	public static fighterController instance; //örnek oluşturduk.
	public static bool isAttacking = false; //saldırı yapma durumunu kontrol edeceğimiz değişken
	private Vector3 direction; //Arissanın rakibi ile göz teması kurmasını sağlamak için Vektörel değerlerini tutacağımız bir değişken tanımladık.
	public int health=100; //sağlık(can) barı için kullanıcının can değerini tutacak değişken
	public Slider playerHB; //oyuncunun sağlık seviyesinin tutulacağı çizelgeyi tanımladık.
	public BoxCollider[] c; //colliderlar için dizi oluşturduk. Arissanın kol ve bacak box colliderlarını bunun içine attık.
	public AudioClip[] audioClip; //Arissa'nın tüm ses dosyalarını burada barındıracağız.
	AudioSource audio; //AudioSource a erişebilmek için oluşturuldu.
	private Vector3 playerPosition; //yeniden başlatırken bu konum bilgisi kullanılacak.
	public GameObject[] enemyList;
	private int index; //Seçili indis değerini tutacağız.


	void Awake(){
		if (instance == null) { //eğer instance değişkenimize değer atanmamışsa
			instance = this; //Objemizin  değerini verdik.
		}
	}

	// Use this for initialization
	void Start () {
		//PlayerPrefte olan değeri alıyoruz ve index değerine atıyoruz.
		index = PlayerPrefs.GetInt ("EnemyCharacterSelected");
		Transform secili = enemyList[index].GetComponent<Transform> ();
		enemyTarget = secili;

		anim = GetComponent<Animator> (); //Animator menusune erişildi
		setAllBoxColliders (false); //Oyun Başlangıcında bütün boxColliderları false yapmak için metodu parametre ile kullandık.
		audio= GetComponent<AudioSource> ();//AudioSource bölümüne eriştik artık arayüzdeki düzenlemeleri buradan yapabiliriz.
		playerPosition=transform.position; //başlangıç konu                                              munu aldık.
	}                                                                                         

	public void playAudio(int clip){

		audio.clip = audioClip [clip];  //Referans eşitleme dizinin seçilen indisindeki müzik dosyası oynatılacak
		audio.Play (); //Oynatma işlemi
	}



	//BoxCollider ların aktif/pasif olma durumlarını fonksiyona gelen parametre yardımıyla burada belirliyoruz.
	private void setAllBoxColliders(bool state){

		c [0].enabled = state; //ilk eleman(RightHand) in aktiflik durumu
		c [1].enabled=state; //ikinci eleman(RightFoot) in aktiflik durumu

	}


	
	// Update is called once per frame
	void Update () {

		if (anim.GetCurrentAnimatorStateInfo (0).IsName ("fight_idle")) { //bu durumun yalnızca idle durumunda olması gerekiyor
			direction = enemyTarget.position - this.transform.position; //direction değişkenine düşmanının pozisyonu ve bizim karakterimiz arasındaki vektörel farkı attık.
			direction.y = 0; //y ekseninin değerinim değişmemesi için 0 a sabitledik.
			this.transform.rotation = Quaternion.Slerp (this.transform.rotation, Quaternion.LookRotation (direction), 0.3f); 
		}


		//Animasyonların bittiğini yakalayıp resetlememiz gerekiyor bunun için getCurrentAnimatorStateınfo ile durumunu alacağız.
		if (anim.GetCurrentAnimatorStateInfo (0).IsName ("fight_idle")) {
			isAttacking = false;
			setAllBoxColliders (false); // bütün boxColliderları false yapmak için metodu parametre ile kullandık.
		}


		if (gameController.allowMovement == true) { //Dreyer hareket izni varsa
			
			if (isAttacking == false) {
			
				//geriye doğru yönlendirme yapılması kontrolü
				if (mvBack == true) {
					anim.SetTrigger ("wkBACK");
					anim.ResetTrigger ("idle");
					setAllBoxColliders (false); // bütün boxColliderları false yapmak için metodu parametre ile kullandık.

				} else if (mvFWD == false) {
					anim.SetTrigger ("idle");
					anim.ResetTrigger ("wkBACK");
				}

				//ileriye doğru yönlendirme yapılması kontrolü

				if (mvFWD == true) {
					anim.SetTrigger ("wkFWD");
					anim.ResetTrigger ("idle");
					setAllBoxColliders (false); // bütün boxColliderları false yapmak için metodu parametre ile kullandık.
				} else if (mvBack == false) {
					anim.SetTrigger ("idle");
					anim.ResetTrigger ("wkFWD");
				}
				
			} else if (isAttacking == true) { //Saldırı durumu aktifse
				setAllBoxColliders (true); // bütün boxColliderları true yapmak için metodu parametre ile kullandık.
			}
		}
	}

	//Yumruk atma metodu
	public void punch(){
		isAttacking = true;	
		anim.ResetTrigger ("idle");
		anim.SetTrigger ("punch");
		playAudio (0); //Yumruk ses efekti oynatılır.

	}

	//Tekme atma metodu
	public void kick(){ //vuruş durumu
		isAttacking = true;	
		anim.ResetTrigger ("idle");
		anim.SetTrigger ("kick");
		playAudio (1); //Tekme ses efekti oynatılır.

	}

	public void react(){
		isAttacking = true;	
		health = health - 10; //sağlık seviyesi 10 düşürülüyor darbe alma durumunda
		if(health <10){ //Eğer sağlık seviyesi 10'un altına inerse nakavt metodu çağırılır.
			knockout ();
			playAudio (3); //Nakavt olunca  ses efekti oynatılır.
		}else{ //Sağlık seviyesi 10 ve üstünde ise react trigger'ına bağladığımız animasyon çalışır.
			anim.ResetTrigger ("idle");
			anim.SetTrigger ("react");
			playAudio (2); //Reaksiyon gösterme durumu
		}
		playerHB.value = health; //Arayüzden erişerek değerini değiştiriyoruz.
	
	}



	//Nakavt Olma durumu için gerekli metot
	public void knockout(){
		gameController.allowMovement=false; //hareket iznini kaldırdık.
		health = 100;
		anim.SetTrigger ("knockout");

		gameController.instance.scoreEnemy ();//Dreyer'ın skorunu arttırması için metot çağırıldı.
		gameController.instance.onScreenPoints(); //Puan değişiminin ekrana yansıması sağlandı.
		gameController.instance.rounds (); //round hesaplaması metodu

		if (gameController.enemyScore == 2) {
			gameController.instance.doReset (); //Eğer Dreyer 2 puan olduysa resetleme işlemi yapıyoruz.
			StartCoroutine (resetCharacters ()); //Karakterleri sıfırlama metodu
		}else{ //round geçisi olduysa
			StartCoroutine (resetCharacters ()); //Karakterleri sıfırlama metodu
		}
	}


	IEnumerator resetCharacters(){
		//theClone içerisinde 5 adet player tagı olan obje oluşuyor collisionlar da dahil beşinci sırada Arissa modeli var bu yüzden 5i seçiyoruz.
		yield return new WaitForSeconds (4);

		playerHB.value = 100; //sağlık çubuğunu 100% yapar.
		//Burada amaç karakter yeniden oluşturulurken doğru konumda oluşturmak bunun içinde Unity'nin
		//oyun başladığı anda oluştrduğu kopya modelin konumunu almamız gerekiyor.

		GameObject[] theclone = GameObject.FindGameObjectsWithTag ("Player"); //Player tagı olanları bir diziye attık.
		Transform t = theclone [5].GetComponent<Transform> ();
		Debug.Log(theclone[0]);
		Debug.Log(theclone[1]);
		Debug.Log(theclone[2]);
		Debug.Log(theclone[3]);
		Debug.Log(theclone[4]);
		Debug.Log(theclone[5]);
		anim.SetTrigger ("idle");
		anim.ResetTrigger ("knockout");
		t.position = playerPosition;
		t.position = new Vector3 (t.position.x, 0, t.position.z); 
		uzaklas = true;
		gameController.allowMovement = true;
	}



}
