using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopController : MonoBehaviour {

    public Button RoFIncreaseButton;

    public Button DmgIncreaseButton;

	// Use this for initialization
	void Start () {
        RoFIncreaseButton.onClick.AddListener(SubmitRofUpgrade);
        DmgIncreaseButton.onClick.AddListener(SubmitDamageUpgrade);
	}

    private void Update()
    {
        SetButtonStates();
    }

    void SubmitRofUpgrade()
    {
        var upgradeCost = IdleProjectileRateOfFireUpgrade.GetUpgradeCost();

        if (PlayerStats.InHandDollars >= upgradeCost)
        {
            PlayerStats.InHandDollars -= upgradeCost;
            PlayerStats.RateOfFireLevel += 1;
            UIController.UpdateUI();
        }
    }

    void SubmitDamageUpgrade()
    {
        var upgradeCost = IdleProjectileDamageUpgrade.GetUpgradeCost();

        if (PlayerStats.InHandDollars >= upgradeCost)
        {
            PlayerStats.InHandDollars -= upgradeCost;
            PlayerStats.DamageLevel += 1;
            UIController.UpdateUI();
        }
    }

    void SetButtonStates()
    {
        var RoFUpgradeCost = IdleProjectileRateOfFireUpgrade.GetUpgradeCost();

        if (PlayerStats.InHandDollars >= RoFUpgradeCost)
        {
            RoFIncreaseButton.interactable = true;
        }
        else
        {
            RoFIncreaseButton.interactable = false;
        }

        var DmgUpgradeCost = IdleProjectileDamageUpgrade.GetUpgradeCost();

        if (PlayerStats.InHandDollars >= DmgUpgradeCost)
        {
            DmgIncreaseButton.interactable = true;
        }
        else
        {
            DmgIncreaseButton.interactable = false;
        }
    }
}
