using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrencySpawner : MonoBehaviour
{
    public GameObject currencyPrefab;
    public GameObject parentObject;

    private CurrencyController currencyController;

    private void Awake()
    {
        currencyController = FindObjectOfType<CurrencyController>();
    }

    public void SpawnNewCurrency(Vector3 collisionPosition)
    {
        Vector3 newCurrencyPosition = new Vector3(collisionPosition.x + Random.Range(-0.25f, 0.25f), collisionPosition.y + Random.Range(-0.25f, 0.25f), 0);

        GameObject newCurrency = Instantiate(currencyPrefab, newCurrencyPosition, Quaternion.identity) as GameObject;

        newCurrency.transform.parent = parentObject.transform;

        currencyController.AddNewCurrencyToList(newCurrency);
    }
}
