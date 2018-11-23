using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LauncherController : MonoBehaviour {

    public float rateOfLaunch;
    public float rangeToSpawn = 1.2f;

    public GameObject projectile;
    public GameObject parent;

	// Use this for initialization
	void Start () {
        StartCoroutine("LaunchProjectiles");
	}
	
	// Update is called once per frame
	void Update () {
        rateOfLaunch = IdleProjectileRateOfFireUpgrade.GetCurrentValue();
    }

    IEnumerator LaunchProjectiles()
    {
        for (;;)
        {
            Instantiate(projectile, CalculateStartPosition(), Quaternion.identity);

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
}
