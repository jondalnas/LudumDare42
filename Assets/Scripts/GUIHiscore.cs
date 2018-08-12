using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class GUIHiscore : MonoBehaviour {
	private static int[] scores;
	private static string[] names;
	private static string[] fakeNames = new string[] { "JON", "CAD", "SAN", "EGD", "EEE", "FEP", "NDA", "KAN", "SAF", "BIN" };
	public static bool hasNewHiscore;
	public static Transform hiscore;
	public static string NAME = "";
	private static int pos;
	public static bool enteredName;

	void Start () {
		scores = new int[10];
		names = new string[10];

		for (int i = 0; i < 10; i++) {
			if (!PlayerPrefs.HasKey("score" + i)) PlayerPrefs.SetInt("score" + i, ((9-i)+1)*100);
			if (!PlayerPrefs.HasKey("name" + i)) PlayerPrefs.SetString("name" + i, fakeNames[i]);
		}

		PlayerPrefs.Save();

		for (int i = 0; i < 10; i++) {
			scores[i] = PlayerPrefs.GetInt("score" + i);
			names[i] = PlayerPrefs.GetString("name" + i);
		}

		hiscore = GameObject.Find("Canvas").transform.Find("Game Over").Find("Hiscore");

		hiscore.GetComponent<Text>().text = "1.  " + names[0] + "  " + scores[0] + "00";
		hiscore.GetComponent<Text>().text += "\n";
		hiscore.GetComponent<Text>().text += "2.  " + names[1] + "  " + scores[1] + "00";
		hiscore.GetComponent<Text>().text += "\n";
		hiscore.GetComponent<Text>().text += "3.  " + names[2] + "  " + scores[2] + "00";
		hiscore.GetComponent<Text>().text += "\n";
		hiscore.GetComponent<Text>().text += "4.  " + names[3] + "  " + scores[3] + "00";
		hiscore.GetComponent<Text>().text += "\n";
		hiscore.GetComponent<Text>().text += "5.  " + names[4] + "  " + scores[4] + "00";
		hiscore.GetComponent<Text>().text += "\n";
		hiscore.GetComponent<Text>().text += "6.  " + names[5] + "  " + scores[5] + "00";
		hiscore.GetComponent<Text>().text += "\n";
		hiscore.GetComponent<Text>().text += "7.  " + names[6] + "  " + scores[6] + "00";
		hiscore.GetComponent<Text>().text += "\n";
		hiscore.GetComponent<Text>().text += "8.  " + names[7] + "  " + scores[7] + "00";
		hiscore.GetComponent<Text>().text += "\n";
		hiscore.GetComponent<Text>().text += "9.  " + names[8] + "  " + scores[8] + "00";
		hiscore.GetComponent<Text>().text += "\n";
		hiscore.GetComponent<Text>().text += "10. " + names[9] + "  " + scores[9] + "00";
	}

	string valid = "abcdefghijklmnopqrstuvwxyz";
	void Update() {
		if (!hasNewHiscore || enteredName) return;

		foreach (KeyCode vKey in System.Enum.GetValues(typeof(KeyCode))) {
			if (Input.GetKeyDown(vKey)) {
				if (valid.Contains(vKey.ToString().ToLower())) {
					NAME += vKey.ToString();
				} else if (vKey == KeyCode.Backspace) {
					NAME = NAME.Substring(0, NAME.Length - 1);
				}
			}
		}

		hiscore.GetComponent<Text>().text = "";

		for (int i = 0; i < 10; i++) {
			if (i == pos) {
				hiscore.GetComponent<Text>().text += (i + 1) + (i == 9 ? ". " : ".  ") + NAME + (NAME.Length==0?"   ":(NAME.Length==1?"  ": (NAME.Length == 0 ? " " : ""))) + "  " + scores[i] + "00";
				hiscore.GetComponent<Text>().text += "\n";
				continue;
			}

			hiscore.GetComponent<Text>().text += (i + 1) + (i == 9 ? ". " : ".  ") + names[i] + "  " + scores[i] + "00";
			hiscore.GetComponent<Text>().text += "\n";
		}

		if (NAME.Length >= 3) {
			enteredName = true;
			names[pos] = NAME;
			UpdateName();
		}
	}

	public static void AddScore(int score) {
		hasNewHiscore = false;
		if (scores[9] < score) {
			hasNewHiscore = true;
			for (int i = 8; i > 0; i--) {
				if (scores[i] < score) {
					scores[i + 1] = scores[i];
					names[i + 1] = names[i];
				} else {
					scores[i + 1] = score;
					names[i + 1] = "   ";
					pos = i + 1;
					break;
				}
			}

			NAME = "";
			enteredName = false;
		}

		hiscore.GetComponent<Text>().text = "1.  " + names[0] + "  " + scores[0] + "00";
		hiscore.GetComponent<Text>().text += "\n";
		hiscore.GetComponent<Text>().text += "2.  " + names[1] + "  " + scores[1] + "00";
		hiscore.GetComponent<Text>().text += "\n";
		hiscore.GetComponent<Text>().text += "3.  " + names[2] + "  " + scores[2] + "00";
		hiscore.GetComponent<Text>().text += "\n";
		hiscore.GetComponent<Text>().text += "4.  " + names[3] + "  " + scores[3] + "00";
		hiscore.GetComponent<Text>().text += "\n";
		hiscore.GetComponent<Text>().text += "5.  " + names[4] + "  " + scores[4] + "00";
		hiscore.GetComponent<Text>().text += "\n";
		hiscore.GetComponent<Text>().text += "6.  " + names[5] + "  " + scores[5] + "00";
		hiscore.GetComponent<Text>().text += "\n";
		hiscore.GetComponent<Text>().text += "7.  " + names[6] + "  " + scores[6] + "00";
		hiscore.GetComponent<Text>().text += "\n";
		hiscore.GetComponent<Text>().text += "8.  " + names[7] + "  " + scores[7] + "00";
		hiscore.GetComponent<Text>().text += "\n";
		hiscore.GetComponent<Text>().text += "9.  " + names[8] + "  " + scores[8] + "00";
		hiscore.GetComponent<Text>().text += "\n";
		hiscore.GetComponent<Text>().text += "10. " + names[9] + "  " + scores[9] + "00";
	}

	public static void UpdateName() {
		for (int i = 0; i < 10; i++) {
			PlayerPrefs.SetInt("score" + i, scores[i]);
			PlayerPrefs.SetString("name" + i, names[i]);
		}

		PlayerPrefs.Save();
	}
}
