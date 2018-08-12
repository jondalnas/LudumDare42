using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity {
	private Rigidbody2D rb;
	private Collider2D worldBox;

	public float speed = 4f;
	public float rotationSpeed = 5f;
	public float transferSpeedAngel = 20f;

	public Transform bullet;
	public Transform head;
	public float cooldown = 0.5f;

	private float targetRotation;
	private bool lockedOn;
	public float lockOnDistance = 10.5f;
	public float sight = 25f;
	private GameObject player;
	private bool hasTarget;
	private Transform close;
	public float AIUpdate = 0.1f;

	private float invisibility = 0.5f;

	public Transform particle;
	private bool isDead;
	public Transform sound;
	public AudioClip explosion;

	void Start() {
		rb = GetComponent<Rigidbody2D>();
		rb.centerOfMass = Vector3.zero;
		player = GameObject.Find("Player");

		worldBox = GameObject.Find("World").GetComponent<Collider2D>();
	}

	float cooldownTimer;
	float AITimer;
	float invisibilityTimer;
	void Update() {
		if (GUIManager.IsPuased()) return;

		invisibilityTimer += Time.deltaTime;
		AITimer += Time.deltaTime;

		if (Mathf.Max(Mathf.Abs(transform.position.x), Mathf.Abs(transform.position.y)) + Mathf.Max(transform.GetComponent<Collider2D>().bounds.extents.x, transform.GetComponent<Collider2D>().bounds.extents.y) > worldBox.bounds.extents.x) {
			Kill();
		}

		if (AITimer < AIUpdate) return;

		if (Mathf.Max(Mathf.Abs(transform.position.x), Mathf.Abs(transform.position.y)) + worldBox.bounds.extents.x * 0.15f > worldBox.bounds.extents.x) {
				targetRotation = Mathf.Atan2(-transform.position.y, -transform.position.x) * Mathf.Rad2Deg + 270 + Random.Range(-3, 3);
		} else if (!close) {
			if (!player) return; 
			Vector3 diff = player.transform.position - transform.position;
			if (diff.sqrMagnitude < sight * sight) {
				targetRotation = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg + 270 + Random.Range(-3, 3);
				targetRotation %= 360;
				hasTarget = true;
			} else if (Random.Range(0, 300) == 0) {
				targetRotation = Random.Range(0, 360);
			}
		} else {
			Vector3 diff = close.position - transform.position;
			if (diff.sqrMagnitude > GetComponentInChildren<CircleCollider2D>().radius * GetComponentInChildren<CircleCollider2D>().radius) {
				close = null;
			}
		}

		RaycastHit2D hit;
		if ((hit = Physics2D.Raycast(head.transform.position, transform.up, lockOnDistance, ~(1<<8 | 1<<2 | 1<<9)))) {
			if (hit.transform.CompareTag("Astroid") || hit.transform.CompareTag("Player")) lockedOn = true;
		}

		cooldownTimer += Time.deltaTime;
		if (lockedOn) {
			if (cooldownTimer > cooldown) {
				cooldownTimer = 0;
				Instantiate(bullet, head.transform.position, transform.rotation);
			}

			lockedOn = false;
		}
	}

	Vector2 lastVel;
	void FixedUpdate() {

		//Set rotation based on input
		if (Mathf.Abs(rb.rotation - targetRotation) > rotationSpeed)
			rb.MoveRotation(rb.rotation - rotateDirection(rb.rotation, targetRotation) * rotationSpeed);
		else
			rb.MoveRotation(targetRotation);

		//Check if the player is pressing the forward or backward keys
		if (rb.rotation % 360 <= targetRotation + 5f && rb.rotation % 360 >= targetRotation - 5f) {
			rb.velocity = transform.up * speed;
		}
		
		if (rb.rotation < 0) {
			rb.rotation += 360;
		}
	}

	void OnCollisionEnter2D(Collision2D coll) {
		Kill();

		if (coll.gameObject.CompareTag("Bullet")) {
			if (coll.transform.GetComponent<Bullet>().IsPlayers()) GUIScore.UpdateScore(5);

			Destroy(coll.gameObject);
		}
	}

	void OnTriggerEnter2D(Collider2D coll) {
		if (coll.CompareTag("Astroid") && Random.Range(0, 1) == 0) {
			close = coll.transform;

			Vector3 diff = coll.transform.position - transform.position;
			targetRotation = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg + 270 + Random.Range(-3, 3);
			targetRotation %= 360;
		} else {
			close = coll.transform;

			Vector3 diff = coll.transform.position - transform.position;
			targetRotation = Mathf.Atan2(-diff.y, -diff.x) * Mathf.Rad2Deg + 270 + Random.Range(-3, 3);
			targetRotation %= 360;
		}
	}

	public void Kill() {
		if (invisibilityTimer < invisibility || isDead) return;

		for (int i = 0; i < 20; i++) {
			Transform p = Instantiate(particle);
			p.transform.position = transform.position;
			p.GetComponent<ParticleHandler>().direction = Mathf.Atan2((transform.position - transform.position).y, (transform.position - transform.position).x) * Mathf.Rad2Deg + (i / 20f * 360f);
		}

		Transform s = Instantiate(sound, transform.position, transform.rotation);
		s.GetComponent<AudioSource>().clip = explosion;
		s.GetComponent<AudioSource>().Play();

		isDead = true;
		Destroy(gameObject);
	}

	private float rotateDirection(float start, float end) {
		start %= 360f;
		end %= 360f;

		float standard = start - end;
		float smart = start - (end + 360);
		float smart2 = (start + 360) - end;

		float best = Mathf.Min(Mathf.Abs(standard), Mathf.Abs(smart)) == Mathf.Abs(standard) ? standard : smart;
		best = Mathf.Min(Mathf.Abs(best), Mathf.Abs(smart2)) == Mathf.Abs(best) ? best : smart2;

		return Mathf.Clamp(-best, 1, -1);
	}
}
