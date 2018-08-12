using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour {

	public int numberOfAstoirds = 2500;
	public Transform astroid;
	public float maxSize = 3f;
	public float minSize = 0.6f;

	public int numberOfEnemies = 100;
	public Transform enemy;
	public Transform noEnemyZone;

	public int berrierPrecision = 500;
	private LineRenderer lr;

	public float matchTime = 5 * 60;

	void Start () {
		lr = GetComponent<LineRenderer>();
		Generate();
		GenerateBarrier();
	}

	int updates;
	Vector3 v = new Vector3(0.3f, 0.3f);
	float time;
	float lastSize = 1;
	void Update () {
		if (GUIManager.IsPuased()) return;

		//Shrink universe to create gameplay!
		time += Time.deltaTime;
		transform.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, 1-Mathf.Cos((time / matchTime) * Mathf.PI * 0.5f));

		if (updates++%10 != 0) return;

		for (int i = 0; i < berrierPrecision/2; i++) {
			lr.SetPosition(i * 2, lr.GetPosition(i * 2) + v * (updates % 20 == 1 ? 1 : -1));
			lr.SetPosition(i * 2 + 1, lr.GetPosition(i * 2 + 1) - v * (updates % 20 == 1 ? 1 : -1));
		}

		lastSize = time / matchTime;
	}

	void OnTriggerExit2D(Collider2D coll) {
		if (coll.CompareTag("Astroid")) {
			Destroy(coll.gameObject);
		} else if (coll.CompareTag("Player")) {
			coll.GetComponent<Player>().Kill();
		} else if (coll.CompareTag("Enemy")) {
			coll.GetComponent<Enemy>().Kill();
		}
	}

	void Generate() {
		//Generate astroids
		Collider2D coll = GetComponent<BoxCollider2D>();
		for (int i = 0; i < numberOfAstoirds; i++) {
			float scale = Random.Range(minSize, maxSize);

			Instantiate(astroid, new Vector3(Random.Range(coll.bounds.min.x, coll.bounds.max.x), Random.Range(coll.bounds.min.y, coll.bounds.max.y), 0), Quaternion.Euler(new Vector3(0, 0, Random.Range(0, 360)))).localScale = Vector3.one * scale;
		}

		for (int i = 0; i < numberOfEnemies; i++) {
			Transform enemyTransform = Instantiate(enemy, new Vector3(Random.Range(coll.bounds.min.x, coll.bounds.max.x), Random.Range(coll.bounds.min.y, coll.bounds.max.y), 0), Quaternion.Euler(new Vector3(0, 0, Random.Range(0, 360))));

			if (noEnemyZone.GetComponent<CircleCollider2D>().bounds.Intersects(enemyTransform.GetComponent<PolygonCollider2D>().bounds)) Destroy(enemyTransform.gameObject);
		}
	}

	void GenerateBarrier() {
		lr = GetComponent<LineRenderer>();
		float width = GetComponent<Collider2D>().bounds.extents.x;
		float height = GetComponent<Collider2D>().bounds.extents.y;
		Vector3 topLeft = GetComponent<Collider2D>().bounds.center + -GetComponent<Collider2D>().bounds.extents;
		Vector3 topRight = GetComponent<Collider2D>().bounds.center + new Vector3(width, -height);
		Vector3 bottomLeft = GetComponent<Collider2D>().bounds.center + new Vector3(-width, height);
		Vector3 bottomRight = GetComponent<Collider2D>().bounds.center + GetComponent<Collider2D>().bounds.extents;

		width *= 2;
		height *= 2;

		float stepX = width / (berrierPrecision / 4);
		float stepY = height / (berrierPrecision / 4);

		Vector3[] positions = new Vector3[berrierPrecision];
		for (int i = 0; i < positions.Length; i++) {
			if (i < berrierPrecision / 4)
				positions[i] = new Vector3(i*stepX, 0) + topLeft;
			else if (i < berrierPrecision / 4 * 2)
				positions[i] = new Vector3(0, (i % (berrierPrecision / 4)) * stepY) + topRight;
			else if (i < berrierPrecision / 4 * 3)
				positions[i] = new Vector3(-(i % (berrierPrecision / 4)) * stepX, 0) + bottomRight;
			else
				positions[i] = new Vector3(0, -(i % (berrierPrecision / 4)) * stepY) + bottomLeft;
		}

		lr.positionCount = berrierPrecision;
		lr.SetPositions(positions);
	}
}
