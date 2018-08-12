using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDangerzone : MonoBehaviour {
	void OnTriggerEnter2D(Collider2D coll) {
		transform.parent.SendMessage("OnTriggerEnter2D", coll);
	}
}
