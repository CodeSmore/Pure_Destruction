using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    private static Animator currencyTextAnimator;

    // Start is called before the first frame update
    void Start()
    {
        currencyTextAnimator = GameObject.Find("CurrentCurrencyValueText").GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void PlayAddCurrencyAnimation()
    {
        currencyTextAnimator.SetTrigger("IncreaseCurrencyTrigger");
    }
}
