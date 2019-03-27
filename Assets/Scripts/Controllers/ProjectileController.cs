using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ProjectileController : MonoBehaviour {
    
    public int projectileValue = 1;
    public GameObject collectable;


    private CircleCollider2D circleColliderOfProjectile;
    private CurrencySpawner currencySpawner;
    private GameController gameController;

    ObjectPooler objectPooler;

    private void Start()
    {
        circleColliderOfProjectile = GetComponent<CircleCollider2D>();

        currencySpawner = FindObjectOfType<CurrencySpawner>();
        gameController = FindObjectOfType<GameController>();

        objectPooler = ObjectPooler.Instance;
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Target")
        {
            TargetController hitTargetController = collider.transform.parent.GetComponentInParent<TargetController>();
            hitTargetController.DamageTarget(collider, circleColliderOfProjectile);

            // Spawn hit effect
            objectPooler.SpawnFromPool("DestructionParticleSystem", transform.position, Quaternion.identity);

            gameObject.SetActive(false);
        } 
    }
}
