using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Currency : MonoBehaviour
{
    public int value = 1;

    private CurrencyController currencyController;

    // Start is called before the first frame update
    void Start()
    {
        currencyController = FindObjectOfType<CurrencyController>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Collector")
        {
            CollectCurrency();
        }
    }

    void OnMouseDown()
    {
        CollectCurrency();
    }

    void CollectCurrency()
    {
        PlayerStats.IncreaseInHandCurrency(value);
        UIController.UpdateUI();

        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        currencyController.RemoveCurrencyFromList(gameObject);
    }
}
