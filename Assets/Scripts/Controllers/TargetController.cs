using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetController : MonoBehaviour {

    public float rotationSpeed;

    public GameObject[] Targets;

    public static float targetResistance;
    public static int targetHitReward;
    public static int targetDestroyReward;

    private void Update()
    {
        if (!TargetExists())
        {
            AddTarget();
        }
        RotateTarget();    
    }

    bool TargetExists()
    {
        if (transform.childCount > 0)
        {
            return true; 
        }
        return false;
    }

    void AddTarget()
    {
        int stage = Mathf.Clamp(PlayerStats.StageLevel, 1, Targets.Length) - 1;

        var newTargetInstance = Instantiate(Targets[stage], transform);
        var newTarget = newTargetInstance.GetComponentInChildren<TargetProperties>();

        Debug.Log(newTarget.GetHitReward());
        targetResistance = newTarget.GetTargetResistance();
        targetHitReward = newTarget.GetHitReward();
        targetDestroyReward = newTarget.GetDestroyReward();
    }

    void RotateTarget()
    {
        transform.Rotate(new Vector3(0, 0, rotationSpeed));
    }
}
