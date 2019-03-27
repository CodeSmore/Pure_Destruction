using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LauncherController : MonoBehaviour {

    public float rateOfLaunch;
    public float rangeToSpawn = 1.2f;

    public static bool isProjectileSpawnEnabled = true;

    ObjectPooler objectPooler;

    // Use this for initialization
    void Start () {
        objectPooler = ObjectPooler.Instance;

        StartCoroutine("LaunchProjectiles");
	}
	
	// Update is called once per frame
	void Update () {

        // TODO why not just check when it gets upgraded?
        rateOfLaunch = IdleProjectileRateOfFireUpgrade.GetCurrentValue();
    }

    IEnumerator LaunchProjectiles()
    {
        for (;;)
        {
            objectPooler.SpawnFromPool("Projectile", CalculateStartPosition(), Quaternion.identity);

            yield return new WaitForSeconds(1 / rateOfLaunch);
        }
        
    }

    Vector3 CalculateStartPosition()
    {
        float x = 0, y = 0;

        Vector2 topRightCorner = new Vector2(1, 1);
        Vector2 edgeVector = Camera.main.ViewportToWorldPoint(topRightCorner);

        while (x > -edgeVector.x && x < edgeVector.x && y > -edgeVector.y && y < edgeVector.y)
        {
            x = Random.Range(-edgeVector.x * rangeToSpawn, edgeVector.x * rangeToSpawn);
            y = Random.Range(-edgeVector.y * rangeToSpawn, edgeVector.y * rangeToSpawn);
        }

        return new Vector3(x, y, 0f);
    }

    public void SetProjectileSpawnStatus(bool newStatus)
    {
        if (newStatus)
        {
            StartCoroutine("LaunchProjectiles");
        }
        else
        {
            StopCoroutine("LaunchProjectiles");
        }
    }
}
