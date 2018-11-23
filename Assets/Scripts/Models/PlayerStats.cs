using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Contains all stats for player and player objects
public class PlayerStats : MonoBehaviour {

    private static bool isTestData;

    public static int StageLevel { get; set; } // current stage level

    public static int InHandDollars { get; set; } // in hand gold

    public static int RateOfFireLevel { get; set; }    // projectile spawns per second LEVEL
    public static int DamageLevel { get; set; } // damage radius of each projectile LEVEL

    private void Awake()
    {
        isTestData = TestData.UsingTestData;
        if (isTestData)
        {
            InHandDollars = TestData.Dollars;
            StageLevel = TestData.StageLevel;

            DamageLevel = TestData.IPDamageLevel;
            RateOfFireLevel = TestData.IPRateOfFireLevel;
        }
        else
        {
            InHandDollars = PlayerPrefsManager.GetInHandDollars();

            DamageLevel = PlayerPrefsManager.GetIdleProjectileDamage();
            RateOfFireLevel = PlayerPrefsManager.GetIdleProjectileRateOfFire();

            // TODO add playerPrefs save/load
            StageLevel = 1;
        }
    }

    private void OnDestroy()
    {
        if (!isTestData)
        {
            PlayerPrefsManager.SaveGameData(RateOfFireLevel, DamageLevel, InHandDollars);
        }
    }
}
