using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetController : MonoBehaviour {

    public float rotationSpeed;

    public GameObject[] Targets;

    public static float targetResistance;
    public static int targetHitReward;
    public static int targetDestroyReward;

    private CurrencySpawner currencySpawner;
    private GameController gameController;
    private Destruction2DLayer destructionLayer;


    private void Start()
    {
        AddTarget();

        currencySpawner = FindObjectOfType<CurrencySpawner>();
        gameController = FindObjectOfType<GameController>();

        destructionLayer = new Destruction2DLayer();
        destructionLayer.SetLayer(0, true);
    }

    private void Update()
    {
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

    public void DamageTarget(Collider2D targetCollider, CircleCollider2D circleColliderOfProjectile)
    {
        if (targetCollider.gameObject.name == "Map")
        {
            DamageTargetMap(circleColliderOfProjectile);
        }
        else if (targetCollider.gameObject.name == "WeakSpot")
        {
            PlayerStats.InHandDollars += TargetController.targetDestroyReward;

            gameController.StartEndGameSequence();

            Destroy(targetCollider.transform.parent.gameObject);
        }
    }

    void DamageTargetMap(CircleCollider2D circleColliderOfProjectile)
    {
        float initialRadius = circleColliderOfProjectile.radius;
        circleColliderOfProjectile.radius = IdleProjectileDamageUpgrade.GetCurrentValue() - TargetController.targetResistance;

        if (circleColliderOfProjectile.radius <= 0)
        {
            circleColliderOfProjectile.radius = 0;
            // TODO spawn paticle system and/or sound effect that shows no dmg is done
            return;
        }

        SpawnCollectables(circleColliderOfProjectile.radius, circleColliderOfProjectile.transform.position);

        // create polygon object that will be taken from target sprite and collider
        var newPolygon = new Polygon2D(Polygon2DList.CreateFromCircleCollider(circleColliderOfProjectile));
        Polygon2D destructionPolygon = new Polygon2D(newPolygon);

        Destruction2D.DestroyByPolygonAll(destructionPolygon, destructionLayer);

        // reset collider radius
        circleColliderOfProjectile.radius = initialRadius;
    }

    public void AddTarget()
    {
        int stage = Mathf.Clamp(PlayerStats.StageLevel, 1, Targets.Length) - 1;

        var newTargetInstance = Instantiate(Targets[stage], transform);
        var newTarget = newTargetInstance.GetComponentInChildren<TargetProperties>();

        targetResistance = newTarget.GetTargetResistance();
        targetHitReward = newTarget.GetHitReward();
        targetDestroyReward = newTarget.GetDestroyReward();
    }

    void RotateTarget()
    {
        transform.Rotate(new Vector3(0, 0, rotationSpeed));
    }

    void SpawnCollectables(float radius, Vector3 spawnPosition)
    {
        for (int i = 0; i < radius; ++i)
        {
            currencySpawner.SpawnNewCurrency(spawnPosition);
        }
    }
}
