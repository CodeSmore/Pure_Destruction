using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ProjectileController : MonoBehaviour {
    
    public int projectileValue = 1;

    private Polygon2D destructionPolygon;
    private Destruction2DLayer destructionLayer;

    private CircleCollider2D circle;

    private void Start()
    {
        circle = GetComponent<CircleCollider2D>();

        destructionLayer = new Destruction2DLayer();
        destructionLayer.SetLayer(0, true);
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Target")
        {
            DamageTarget(collider);
            PlayerStats.InHandDollars += TargetController.targetHitReward;
            UIController.UpdateUI();
            
            Destroy(gameObject);
        } 
    }

    void DamageTarget(Collider2D targetCollider)
    {
        if (targetCollider.gameObject.name == "Map")
        {
            // create polygon object that will be taken from target
            circle.radius = IdleProjectileDamageUpgrade.GetCurrentValue() - TargetController.targetResistance;

            if (circle.radius <= 0)
            {
                circle.radius = 0;
            }

            var newPolygon = new Polygon2D(Polygon2DList.CreateFromCircleCollider(circle));
            destructionPolygon = new Polygon2D(newPolygon);

            Destruction2D.DestroyByPolygonAll(destructionPolygon, destructionLayer);
        }
        else if (targetCollider.gameObject.name == "WeakSpot")
        {
            // Destroy target
            Destroy(targetCollider.transform.parent.gameObject);
            PlayerStats.StageLevel++;
            PlayerStats.InHandDollars += TargetController.targetDestroyReward;

            // Destroy all projectiles
            // TODO put this somewhere better, you sexy beast
            var projectiles = GameObject.FindGameObjectsWithTag("Projectile");

            foreach (var GameObject in projectiles)
            {
                Destroy (GameObject);
            }
        }
    }

    // Doesn't quite work, but it's a start
    //void FaceTowardsDirectionOfMovement()
    //{
    //    var dir = rb.velocity;

    //    if (dir != Vector2.zero)
    //    {
    //        Debug.Log("turning");

    //        transform.rotation = Quaternion.Slerp(
    //            transform.rotation,
    //            Quaternion.LookRotation(dir),
    //            Time.deltaTime * rotationSpeed
    //        );
    //    }
    //}
}
