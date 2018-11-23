using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravity2D : MonoBehaviour {
	Rigidbody2D body;

    public float gravityModifier = 2f;
    private Vector2 targetPosition;
	
	void Start () {
		body = GetComponent<Rigidbody2D>();
        targetPosition = GameObject.FindGameObjectWithTag("Target").transform.position;
	}
	
	void Update () {
		Vector3 worldPosition =  (Vector2)(transform.position + new Vector3(body.centerOfMass.x, body.centerOfMass.y, 0)) ;
		body.AddForce(Vector2D.RotToVec(Vector2D.Atan2(targetPosition, worldPosition)).ToVector2() * 10f * gravityModifier);
	}
}
