using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
	public float speed = 7.5f;
	public float killTimer = 3f;
	public Transform sound;
	[HideInInspector]
	public bool isPlayers;
	private Collider2D worldBox;

	void Awake() {
		GetComponent<LineRenderer>().useWorldSpace = false;
	}

	void Start() {
		Instantiate(sound, transform.position, transform.rotation);

		worldBox = GameObject.Find("World").GetComponent<Collider2D>();
	}

	float timer;
	void Update() {
		if (Mathf.Max(Mathf.Abs(transform.position.x), Mathf.Abs(transform.position.y)) > worldBox.bounds.extents.x) {
			Destroy(gameObject);
		}

		timer += Time.fixedDeltaTime;
		if (timer > killTimer) {
			Destroy(gameObject);
		}
	}

	void FixedUpdate() {
		transform.position += transform.up * speed * Time.fixedDeltaTime;
	}

	public bool IsPlayers() {
		return isPlayers;
	}
}
