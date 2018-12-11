using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour {

    public int value = 1;

	void OnMouseDown()
    {
        PlayerStats.InHandDollars += value;
        UIController.UpdateUI();

        Destroy(gameObject);
    }
}
