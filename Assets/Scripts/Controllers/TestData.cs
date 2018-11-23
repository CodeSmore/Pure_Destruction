using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestData : MonoBehaviour {

    public bool UseTestData = false;
    public static bool UsingTestData = false;

    public int InHandDollars;
    public static int Dollars;

    public int CurrentStageLevel;
    public static int StageLevel;

    public int IdleProjectileRateOfFireLevel;
    public static int IPRateOfFireLevel;

    public int IdleProjectileDamageLevel;
    public static int IPDamageLevel;
    
	// Use this for initialization
	void Awake () {
        UsingTestData = UseTestData;

        Dollars = InHandDollars;
        StageLevel = CurrentStageLevel;

        IPRateOfFireLevel = IdleProjectileRateOfFireLevel;
        IPDamageLevel = IdleProjectileDamageLevel;
	}
}
