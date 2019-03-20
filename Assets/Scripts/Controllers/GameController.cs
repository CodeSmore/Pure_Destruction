using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject BlackHole;

    private TargetController targetController;
    private LauncherController launcherController;

    private GameObject blackHoleInstance;
    private ParticleSystem mainBackgroundParticleSystem;

    // Start is called before the first frame update
    void Start()
    {
        targetController = FindObjectOfType<TargetController>();
        launcherController = FindObjectOfType<LauncherController>();
        mainBackgroundParticleSystem = GameObject.Find("BackgroundParticleSystem").GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartEndGameSequence()
    {
        launcherController.SetProjectileSpawnStatus(false);

        var projectiles = GameObject.FindGameObjectsWithTag("Projectile");
        foreach (var projectile in projectiles)
        {
            Destroy(projectile);
        }

        blackHoleInstance = Instantiate(BlackHole) as GameObject;

        // change gravity of all collectables
        Gravity2D.ChangeState();

        CollectorMovement.SendCollectorsHome();

        // cannot enable module directly from particle system reference,
        // so added a temp var to make the change
        var volt = mainBackgroundParticleSystem.velocityOverLifetime;
        volt.enabled = true;
        mainBackgroundParticleSystem.Clear();
        mainBackgroundParticleSystem.Play();
    }

    public void StartNextStage()
    {
        PlayerStats.StageLevel++;
        UIController.UpdateProgressUI();

        Destroy(blackHoleInstance);
        targetController.AddTarget();

        CollectorMovement.DeployCollectors();
        launcherController.SetProjectileSpawnStatus(true);

        
        // cannot enable module directly from particle system reference,
        // so added a temp var to make the change
        var volt = mainBackgroundParticleSystem.velocityOverLifetime;
        volt.enabled = false;
        mainBackgroundParticleSystem.Clear();
        mainBackgroundParticleSystem.Play();


        Gravity2D.ChangeState();
    }
}
