using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

	public void PlayGame(){
	
		SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex + 1);//Seçili sahneden bir sonraki sahneye geçiş yapar.
		//SceneManager.LoadScene ("karakter-secim");
	}

	public void QuitGame(){
		Application.Quit ();
	}

}
