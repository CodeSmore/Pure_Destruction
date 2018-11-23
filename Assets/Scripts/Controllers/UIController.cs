using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Manages player UI
public class UIController : MonoBehaviour {

    // CurrencyUI
    public static Text InHandGoldText; // Text Display for current amount of Gold

    // ProgressUI
    public static Text StageNumberText; // Text Display for current stage

    // StatsUI
    public static Text RateOfFireStatText; // Text Display for Rate of Fire
    public static Text DamageStatText; // Text Display for Damage

    // StoreUI
    public static Text RateOfFireCostText; // Text Display cost for upgrading Rate of Fire
    public static Text DamageCostText; // Text Display cost for upgrading Damage

	// Use this for initialization
	void Start () {
        InHandGoldText = GameObject.Find("CurrentCurrencyValueText").GetComponent<Text>();
        StageNumberText = GameObject.Find("StageNumberText").GetComponent<Text>();

        RateOfFireStatText = GameObject.Find("RoF Text").GetComponent<Text>();
        RateOfFireCostText = GameObject.Find("RoFCostText").GetComponent<Text>();
        DamageStatText = GameObject.Find("Dmg Text").GetComponent<Text>();
        DamageCostText = GameObject.Find("DmgCostText").GetComponent<Text>();

        UpdateUI();
	}
	
	// Updates all UI elements
	public static void UpdateUI () {
        UpdateCurrencyUI();
        UpdateProgressUI();
        UpdateStatsUI();
        UpdateStoreUI();
    }

    private static void UpdateCurrencyUI()
    {
        var InHandGold = PlayerStats.InHandDollars;
        InHandGoldText.text = "$ " + InHandGold.ToString();
    }

    private static void UpdateProgressUI()
    {
        var CurrentStage = PlayerStats.StageLevel;
        StageNumberText.text = CurrentStage.ToString();
    }

    private static void UpdateStatsUI()
    {
        var RateOfFire = IdleProjectileRateOfFireUpgrade.GetCurrentValue();
        RateOfFireStatText.text = RateOfFire.ToString("0.##") + "/Second";

        var Damage = IdleProjectileDamageUpgrade.GetCurrentValue();
        DamageStatText.text = Damage.ToString("0.##");
    }

    private static void UpdateStoreUI()
    {
        var RateOfFireUpgradeCost = IdleProjectileRateOfFireUpgrade.GetUpgradeCost();
        RateOfFireCostText.text = "$ " + RateOfFireUpgradeCost.ToString();

        var DamageUpgradeCost = IdleProjectileDamageUpgrade.GetUpgradeCost();
        DamageCostText.text = "$ " + DamageUpgradeCost.ToString();
    }
}
