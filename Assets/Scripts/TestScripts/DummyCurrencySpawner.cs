using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyCurrencySpawner : MonoBehaviour
{
    public GameObject dummyCurrencyPrefab;
    public GameObject parentObject;

    private CurrencyController currencyController;

    private void Awake()
    {
        currencyController = FindObjectOfType<CurrencyController>();
    }

    // Start is called before the first frame update
    void Start()
    {
        SpawnNewCurrency();
        SpawnNewCurrency();

    }

    void SpawnNewCurrency()
    {
        Vector3 newCurrencyPosition = new Vector3(Random.Range(-5, 5), Random.Range(-5, 5), 0);

        GameObject newCurrency = Instantiate(dummyCurrencyPrefab, newCurrencyPosition, Quaternion.identity) as GameObject;
        Debug.Log(newCurrency);

        newCurrency.transform.parent = parentObject.transform;

        currencyController.AddNewCurrencyToList(newCurrency);
    }
}
