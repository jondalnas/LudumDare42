using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour {
	public GameObject graphicsComponent;

	void Awake() {
		LineRenderer lr = graphicsComponent.GetComponent<LineRenderer>();
		lr.useWorldSpace = false;

		UpdateCollider();
	}

	protected void UpdateCollider() {
		LineRenderer lr = graphicsComponent.GetComponent<LineRenderer>();
		PolygonCollider2D collider = GetComponent<PolygonCollider2D>();

		Vector3[] positions = new Vector3[lr.positionCount];
		lr.GetPositions(positions);

		List<Vector2> points = new List<Vector2>();
		foreach (Vector3 pos in positions) {
			points.Add(pos);
		}

		collider.points = points.ToArray();
	}
}