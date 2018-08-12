using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class GUIScore : MonoBehaviour {
	public Transform scoreNumber;
	public static int SCORE = 0;

	void Update() {
		if (GUIManager.IsPuased()) return;

		if (SCORE == 0) {
			scoreNumber.GetComponent<Text>().text = "000000";
			return;
		}

		int zerosBefor = 4 - (int) Mathf.Ceil(Mathf.Log10(SCORE));

		string text = "";
		for (int i = 0; i < zerosBefor; i++) {
			text += "0";
		}
		text += SCORE + "00";

		scoreNumber.GetComponent<Text>().text = text;
	}

	public static void UpdateScore(int add) {
		SCORE += add;
	}
}
