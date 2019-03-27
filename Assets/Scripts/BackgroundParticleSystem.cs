using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundParticleSystem : MonoBehaviour
{
    private static ParticleSystem thisParticleSystem;

    private static float stageOneSpeedModifier = 2;
    private static float stageTwoSpeedModifier = 6;

    // Start is called before the first frame update
    void Start()
    {
        thisParticleSystem = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public static void StartStageOneOfBlackHoleSuction()
    {
        // cannot modify module directly from particle system reference,
        // so added a temp var to make the change
        var volt = thisParticleSystem.velocityOverLifetime;
        volt.speedModifier = stageOneSpeedModifier;

        ResetParticleSystem();
    }

    public static void StartStageTwoOfBlackHoleSuction()
    {
        // cannot modify module directly from particle system reference,
        // so added a temp var to make the change
        var volt = thisParticleSystem.velocityOverLifetime;
        volt.speedModifier = stageTwoSpeedModifier;
    }

    public static void EndBlackHoleSuction()
    {
        var volt = thisParticleSystem.velocityOverLifetime;
        volt.speedModifier = 0;

        // Must stop & clear system to set randomSeed
        thisParticleSystem.Stop();
        thisParticleSystem.Clear();
        thisParticleSystem.randomSeed = (uint)Random.Range(uint.MinValue, uint.MaxValue);

        ResetParticleSystem();
    }

    static void ResetParticleSystem()
    {
        thisParticleSystem.Clear();
        thisParticleSystem.Play();
    }
}
