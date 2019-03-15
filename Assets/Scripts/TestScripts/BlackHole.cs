using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHole : MonoBehaviour
{
    private CurrencyController currencyController;
    private GameController gameController;

    // Start is called before the first frame update
    void Start()
    {
        currencyController = FindObjectOfType<CurrencyController>();
        gameController = FindObjectOfType<GameController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!currencyController.doCurrenciesExist())
        {
            gameController.StartNextStage();
        }
         
    }
}
