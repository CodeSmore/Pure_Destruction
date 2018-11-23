using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleProjectile : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

public class IdleProjectileRateOfFireUpgrade : MonoBehaviour
{
    private static int RoFIncreaseCost;
    private static int RoFCurrentCost;

    private static float RoFCurrentValue;

    public static int Level { get; set; }
    public static float RoFCoefficient { get; set; }

    static IdleProjectileRateOfFireUpgrade()
    {
        RoFIncreaseCost = 1;
        RoFCoefficient = 0.1f;
    }

    public static int GetUpgradeCost()
    {
        Level = PlayerStats.RateOfFireLevel;
        RoFCurrentCost = Level * RoFIncreaseCost;

        return RoFCurrentCost;
    }

    public static float GetCurrentValue()
    {
        Level = PlayerStats.RateOfFireLevel;
        RoFCurrentValue = 1 + Level * RoFCoefficient;

        return RoFCurrentValue;
    }
}

public class IdleProjectileDamageUpgrade : MonoBehaviour
{
    private static int DmgIncreaseCost;
    private static int DmgCurrentCost;

    private static float DmgCurrentValue;

    public static int Level { get; set; }
    public static float DmgCoefficient { get; set; }

    static IdleProjectileDamageUpgrade()
    {
        DmgIncreaseCost = 10;
        DmgCoefficient = 0.2f;
    }

    public static int GetUpgradeCost()
    {
        Level = PlayerStats.DamageLevel;
        DmgCurrentCost = Level * DmgIncreaseCost;

        return DmgCurrentCost;
    }

    public static float GetCurrentValue()
    {
        Level = PlayerStats.DamageLevel;
        DmgCurrentValue = Level * DmgCoefficient;

        return DmgCurrentValue;
    }
}
