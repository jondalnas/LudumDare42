using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class LoadScene : MonoBehaviour {
	public static void Load(string sceneName) {
		SceneManager.LoadScene(sceneName);
	}
}
