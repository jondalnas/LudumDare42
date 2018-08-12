using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Astroid : Entity {
	public Transform astroid;
	public float speed = 300f;

	private Collider2D worldBox;
	
	public Transform particle;
	public Transform sound;
	public AudioClip explosion;

	void Start() {
		LineRenderer lr = GetComponentInChildren<LineRenderer>();

		Vector3[] positions = new Vector3[lr.positionCount];
		lr.GetPositions(positions);
		for (int i = 0; i < positions.Length; i++) {
			positions[i] *= Random.Range(0.7f, 1.3f);
		}

		lr.SetPositions(positions);

		UpdateCollider();

		if (GetComponent<PolygonCollider2D>().bounds.Intersects(GameObject.Find("Player").GetComponent<PolygonCollider2D>().bounds)) {
			Destroy(gameObject);
		}

		worldBox = GameObject.Find("World").GetComponent<Collider2D>();
	}

	void Update() {
		if (GUIManager.IsPuased()) return;

		if (Mathf.Max(Mathf.Abs(transform.position.x), Mathf.Abs(transform.position.y)) + Mathf.Max(transform.GetComponent<Collider2D>().bounds.extents.x, transform.GetComponent<Collider2D>().bounds.extents.y) > worldBox.bounds.extents.x) {
			for (int i = 0; i < 5; i++) {
				Transform p = Instantiate(particle);
				p.transform.position = transform.position;
				p.GetComponent<ParticleHandler>().direction = Mathf.Atan2((-transform.position).y, (-transform.position).x) * Mathf.Rad2Deg + (i / 4f * 90f - 45f);
			}

			Destroy(gameObject);
		}
	}

	void OnCollisionEnter2D(Collision2D coll) {
		if (coll.transform.CompareTag("Bullet")) {
			//TODO: Add health?
			
			if (coll.transform.GetComponent<Bullet>().IsPlayers()) GUIScore.UpdateScore((transform.localScale.x > 1) ? 1 : (transform.localScale.x > 0.6f ? 2 : 4));
			
			for (int i = 0; i < 5; i++) {
				Transform p = Instantiate(particle);
				p.transform.position = coll.transform.position;
				p.GetComponent<ParticleHandler>().direction = Mathf.Atan2((coll.transform.position-transform.position).y, (coll.transform.position - transform.position).x) * Mathf.Rad2Deg + (i/4f*90f-45f);
			}

			Transform s = Instantiate(sound, transform.position, transform.rotation);
			s.GetComponent<AudioSource>().clip = explosion;
			s.GetComponent<AudioSource>().Play();

			if (transform.localScale.x <= 0.25f) {
				Destroy(gameObject);
				Destroy(coll.gameObject);
				return;
			}

			Split(coll);

			//Destroy bullet or else... Oh God!
			Destroy(coll.gameObject);
		} else if (coll.transform.CompareTag("Enemy")) {
			for (int i = 0; i < 5; i++) {
				Transform p = Instantiate(particle);
				p.transform.position = coll.transform.position;
				p.GetComponent<ParticleHandler>().direction = Mathf.Atan2((coll.transform.position - transform.position).y, (coll.transform.position - transform.position).x) * Mathf.Rad2Deg + (i / 4f * 90f - 45f);
			}

			Transform s = Instantiate(sound, transform.position, transform.rotation);
			s.GetComponent<AudioSource>().clip = explosion;
			s.GetComponent<AudioSource>().Play();

			if (transform.localScale.x <= 0.25f) {
				Destroy(gameObject);
				Destroy(coll.gameObject);
				return;
			}

			Split(coll);
		}
	}

	private void Split(Collision2D coll) {
		//If hit by bullet, then split
		Transform right = Instantiate(astroid, transform.position, transform.rotation);
		Transform left = Instantiate(astroid, transform.position, transform.rotation);

		right.position += coll.transform.right * transform.localScale.x;
		right.transform.localScale *= 0.5f;
		right.GetComponent<Rigidbody2D>().AddForce(coll.transform.right * speed);
		right.GetComponent<Rigidbody2D>().mass *= 0.5f;

		left.position += -coll.transform.right * transform.localScale.x;
		left.transform.localScale *= 0.5f;
		left.GetComponent<Rigidbody2D>().AddForce(-coll.transform.right * speed);
		left.GetComponent<Rigidbody2D>().mass *= 0.5f;

		Destroy(gameObject);
	}
}
