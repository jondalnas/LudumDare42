using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUILife : MonoBehaviour {
	public static Transform life;
	
	public static void Damage() {
		if (!life) life = GameObject.Find("Life").transform;

		Destroy(life.GetChild(life.childCount-1).gameObject);
	}

	public static void Kill() {
		if (!life) life = GameObject.Find("Life").transform;

		for (int i = 0; i < life.childCount; i++) {
			Destroy(life.GetChild(i).gameObject);
		}
	}
}
