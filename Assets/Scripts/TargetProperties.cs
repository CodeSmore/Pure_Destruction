using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetProperties : MonoBehaviour {
    public float BaseResistance = 1;
    public float ResistanceCoefficient = 0.01f;

    public float HitReward = 1;
    public float DestroyReward = 10;

    public float HitRewardCoefficient = 0.5f;
    public float DestroyRewardCoefficient = 1f;


    public float GetTargetResistance()
    {
        var resistance = BaseResistance + ResistanceCoefficient * PlayerStats.StageLevel;

        return resistance;
    }

    public int GetHitReward()
    {
        var totalHitReward = Mathf.CeilToInt(HitReward + HitReward * HitRewardCoefficient);

        return totalHitReward;
    }

    public int GetDestroyReward()
    {
        var totalDestroyReward = Mathf.CeilToInt(DestroyReward + DestroyReward * DestroyRewardCoefficient);

        return totalDestroyReward;
    }
}
