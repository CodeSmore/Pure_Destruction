using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boundary : MonoBehaviour {

    private BoxCollider2D collider;

	// Use this for initialization
	void Start () {
        collider = GetComponent<BoxCollider2D>();

        SetBoundary();
	}

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Collectable")
        {
            Destroy(collision.gameObject);
        }
    }

    void SetBoundary()
    {
        collider.size = new Vector2(Camera.main.orthographicSize * Screen.width / Screen.height, Camera.main.orthographicSize) * 2;
    }
}
