using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity {
	private Rigidbody2D rb;
	private Collider2D worldBox;

	public float speed = 2f;
	public float rotationSpeed = 2f;
	public float transferSpeedAngel = 20f;

	public Transform bullet;
	public Transform head;
	public float cooldown = 0.5f;

	private int hp = 3;
	public float imunity = 1.5f;
	private float imunityTimer;
	public float blink = 0.5f;
	public Transform particle;
	public Transform sound;
	public AudioClip explosion;

	void Start() {
		rb = GetComponent<Rigidbody2D>();
		rb.centerOfMass = Vector3.zero;

		worldBox = GameObject.Find("World").GetComponent<Collider2D>();
	}

	float blinkTimer;
	float cooldownTimer;
	void Update() {
		if (GUIManager.IsPuased()) return;

		if (Mathf.Max(Mathf.Abs(transform.position.x), Mathf.Abs(transform.position.y)) + Mathf.Max(transform.GetComponent<Collider2D>().bounds.extents.x, transform.GetComponent<Collider2D>().bounds.extents.y) > worldBox.bounds.extents.x) {
			Kill();
		}

		//If the player is imune, then blink to indicate it
		if (imunityTimer > 0) {
			imunityTimer -= Time.deltaTime;

			blinkTimer += Time.deltaTime;
			//If two thirds of the time has passed, then blink twice as fast
			if (imunityTimer < imunity / 3) {
				if (blinkTimer > blink / 2) {
					blinkTimer = 0;
					graphicsComponent.SetActive(!graphicsComponent.activeSelf);
				}
			} else {
				if (blinkTimer > blink) {
					blinkTimer = 0;
					graphicsComponent.SetActive(!graphicsComponent.activeSelf);
				}
			}
		} else graphicsComponent.SetActive(true);

		if (Input.GetButtonDown("Fire")) {
			//if (cooldownTimer > cooldown) {
			cooldownTimer = 0;
			Instantiate(bullet, head.transform.position, transform.rotation).GetComponent<Bullet>().isPlayers = true;
			//}
		}
	}

	Vector2 lastVel;
	void FixedUpdate() {
		//Set rotation based on input
		rb.MoveRotation(rb.rotation - Input.GetAxis("Rotate") * rotationSpeed);

		//Check if the player is pressing the forward or backward keys
		if (Input.GetAxis("Gas") != 0) {
			float keySpeed = Input.GetAxis("Gas");
			
			//Check if the last time the forward vector was updated, is within "transferSpeedAngel" degrees of the current forward vector
			if (Mathf.Acos(Vector3.Dot(lastVel.normalized, (transform.up * keySpeed * speed).normalized)) * Mathf.Rad2Deg < transferSpeedAngel) {
				//If the speed up the last time we updated the forward vector is grater than the new forward vecotr, then 

				if (lastVel.sqrMagnitude > (transform.up * keySpeed * speed).sqrMagnitude) {
					keySpeed = lastVel.magnitude / speed * (keySpeed < 0 ? -1 : 1);
				} else lastVel = rb.velocity;
			} else lastVel = rb.velocity;

			//Set the speed of the player
			rb.velocity = transform.up * keySpeed * speed;
		}
	}

	void OnCollisionEnter2D(Collision2D coll) {
		if (coll.transform.CompareTag("Bullet") || coll.transform.CompareTag("Enemy")) {
			Damage();

			if (coll.transform.CompareTag("Bullet")) {
				Destroy(coll.gameObject);
			}
		}
	}

	public void Damage() {
		if (imunityTimer > 0) return;
		imunityTimer = imunity;

		hp--;
		if (hp <= 0) {
			Kill();
		}

		GUILife.Damage();
	}

	public void Kill() {
		GUIManager.ChangeGameState(GUIManager.GUI.GAME_OVER);
		Destroy(gameObject);
		GUILife.Kill();
		
		for (int i = 0; i < 20; i++) {
			Transform p = Instantiate(particle);
			p.transform.position = transform.position;
			p.GetComponent<ParticleHandler>().direction = Mathf.Atan2((transform.position - transform.position).y, (transform.position - transform.position).x) * Mathf.Rad2Deg + (i / 20f * 360f);
		}

		Transform s = Instantiate(sound, transform.position, transform.rotation);
		s.GetComponent<AudioSource>().clip = explosion;
		s.GetComponent<AudioSource>().Play();
	}

	void LateUpdate() {
		Camera.main.transform.position = transform.position + Vector3.back * 10;
	}
}
