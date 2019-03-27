using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHole : MonoBehaviour
{
    public float fadeOutStrength = 1;

    private CurrencyController currencyController;
    private GameController gameController;
    private Animator animator;

    bool isFadingAway = false;

    // Start is called before the first frame update
    void Start()
    {
        currencyController = FindObjectOfType<CurrencyController>();
        gameController = FindObjectOfType<GameController>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isFadingAway)
        {
            if (animator.isActiveAndEnabled)
            {
                animator.enabled = false;
            }

            Vector3 fadedScale = new Vector3(Time.deltaTime * fadeOutStrength, Time.deltaTime * fadeOutStrength, transform.localScale.z);
            transform.localScale -= fadedScale;

            if (transform.localScale.x <= 0)
            {
                EndFadeAway();
            }
        }
        else if (!currencyController.doCurrenciesExist())
        {
            animator.SetTrigger("FadeAwayTrigger");
        }
         
    }

    public void StartFadeAway()
    {
        isFadingAway = true;
        BackgroundParticleSystem.StartStageTwoOfBlackHoleSuction();
    }

    void EndFadeAway()
    {
        isFadingAway = false;
        gameController.StartNextStage();
    }
}
