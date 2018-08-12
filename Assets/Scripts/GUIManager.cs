using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIManager : MonoBehaviour {

	public float blink = 0.5f;
	public Transform pause;
	public Transform gameOver;

	public enum GUI {
		GAME,
		PAUSE,
		GAME_OVER,
	};

	private static GUI state;
	
	float blinkTimer;
	void Update () {
		blinkTimer += Time.deltaTime / Time.timeScale;

		if (state == GUI.GAME_OVER) {
			//make the menu blink, with "blink" sencond interval
			if (blinkTimer > blink) {
				blinkTimer = 0;

				gameOver.Find("Game Over Text").gameObject.SetActive(!gameOver.Find("Game Over Text").gameObject.activeSelf);
			}

			if (Input.GetButtonDown("Insert Coin") && (!GUIHiscore.hasNewHiscore || GUIHiscore.enteredName)) {
				state = GUI.GAME;
				GUIScore.SCORE = 0;
				LoadScene.Load("Game");
			}
		} else if (state == GUI.PAUSE) {
			//make the menu blink, with "blink" sencond interval
			if (blinkTimer > blink) {
				blinkTimer = 0;

				pause.gameObject.SetActive(!pause.gameObject.activeSelf);
			}

			if (Input.GetButtonDown("Pause")) {
				ChangeGameState(GUI.GAME);
				Time.timeScale = 1f;
			}
		} else if (state == GUI.GAME) {
			pause.gameObject.SetActive(false);

			if (Input.GetButtonDown("Pause")) {
				ChangeGameState(GUI.PAUSE);
				Time.timeScale = 0.00001f;
				pause.gameObject.SetActive(true);
				blinkTimer = 0;
			}
		}
	}

	public static void ChangeGameState(GUI state) {
		if (state == GUI.GAME_OVER && GUIManager.state != GUI.GAME_OVER) {
			GameObject.Find("Canvas").transform.Find("Game Over").gameObject.SetActive(true);
			GUIHiscore.AddScore(GUIScore.SCORE);
			GameObject.Find("Canvas").transform.Find("Game Over").Find("Hiscore").Find("New Hiscore").gameObject.SetActive(GUIHiscore.hasNewHiscore);
		}

		GUIManager.state = state;
	}

	public static bool IsPuased() {
		return state == GUI.PAUSE;
	}
}
