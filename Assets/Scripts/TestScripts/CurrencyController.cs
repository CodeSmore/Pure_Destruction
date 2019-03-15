using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrencyController : MonoBehaviour
{
    public List<GameObject> allCurrencyObjectsInScene;

    // Start is called before the first frame update
    void Awake()
    {
        allCurrencyObjectsInScene = new List<GameObject>();
    }


    public void AddNewCurrencyToList(GameObject newCurrency)
    {
        allCurrencyObjectsInScene.Add(newCurrency);
    }

    public void RemoveCurrencyFromList(GameObject currency)
    {
        allCurrencyObjectsInScene.Remove(currency);
    }

    public List<GameObject> GetAllCurrenciesInScene()
    {
        return allCurrencyObjectsInScene;
    }

    public bool doCurrenciesExist()
    {
        return allCurrencyObjectsInScene.Count > 0;
    }
}