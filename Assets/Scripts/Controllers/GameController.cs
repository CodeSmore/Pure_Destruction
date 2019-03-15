using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject BlackHole;

    private TargetController targetController;
    private LauncherController launcherController;

    private GameObject blackHoleInstance;

    // Start is called before the first frame update
    void Start()
    {
        targetController = FindObjectOfType<TargetController>();
        launcherController = FindObjectOfType<LauncherController>();
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
        // stop spawning projectiles
        // TODO make sure to turn it back on 
    }

    public void StartNextStage()
    {
        PlayerStats.StageLevel++;
        UIController.UpdateProgressUI();

        Destroy(blackHoleInstance);
        targetController.AddTarget();

        CollectorMovement.DeployCollectors();
        launcherController.SetProjectileSpawnStatus(true);

        Gravity2D.ChangeState();
    }
}
