using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Gravity2D script is attached to an object that is to be affected by gravity. The 'gravityModifier' will
// determine the intensity and direction of force.

public class Gravity2D : MonoBehaviour {
	Rigidbody2D body;

    public float gravityModifier = 2f;
    public float blackHoleModifier = -10f;
    private Vector2 targetPosition;

    private static string State = "pushed";
	
	void Start () {
		body = GetComponent<Rigidbody2D>();
        targetPosition = new Vector3 (0, 0, 0);
	}

	void Update () {
		Vector3 worldPosition =  (Vector2)(transform.position + new Vector3(body.centerOfMass.x, body.centerOfMass.y, 0));

        if (State == "pushed")
        {
            body.AddForce(Vector2D.RotToVec(Vector2D.Atan2(targetPosition, worldPosition)).ToVector2() * gravityModifier);
        }
        else if (State == "sucked")
        {
            // sucked objects (into BlackHole) have their gravity reversed and intensity doubled
            body.AddForce(Vector2D.RotToVec(Vector2D.Atan2(targetPosition, worldPosition)).ToVector2() * gravityModifier * blackHoleModifier);
        }
    }

    public static void ChangeState()
    {
        if (State == "pushed")
        {
            State = "sucked";
        }
        else if (State == "sucked")
        {
            State = "pushed";
        }
    }
}
