using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleHandler : MonoBehaviour {
	public float deadTime = 2f;

	public float speed = 4;
	[HideInInspector]
	public float direction;
	private Vector3 move;

	void Start() {
		direction += Random.Range(-3, 3);
		speed += Random.Range(-3, 3);

		move = new Vector2(Mathf.Cos(direction * Mathf.Deg2Rad), Mathf.Sin(direction * Mathf.Deg2Rad));
	}

	float timer;
	void Update() {
		timer += Time.deltaTime;

		if (deadTime < timer) {
			Destroy(gameObject);
		}

		speed *= 0.85f;
	}

	void FixedUpdate() {
		transform.position += move * speed * Time.fixedDeltaTime;
	}
}
