using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectorMovement : MonoBehaviour
{
    public float movementSpeed;


    private Transform targetTransform;

    private CurrencyController currencyController;
    private GameObject collectorHome;

    public static bool goHome = false;

    // Start is called before the first frame update
    void Start()
    {
        currencyController = FindObjectOfType<CurrencyController>();
        collectorHome = GameObject.Find("CollectorHome");
    }

    // Update is called once per frame
    void Update()
    {
        if (targetTransform)
        {
            MoveTowardsTarget();
        }
        else
        {
            GetNewTarget();
        }
    }

    void MoveTowardsTarget()
    {
        transform.position = Vector3.MoveTowards(transform.position, new Vector3 (targetTransform.position.x, targetTransform.position.y, 0), movementSpeed * Time.deltaTime);

        if (transform.position == targetTransform.position)
        {
            targetTransform = null;
        }
    }

    void GetNewTarget()
    {
        List<GameObject> currencies = currencyController.GetAllCurrenciesInScene();

        GameObject closestCurrency = null;

        if (currencies.Count == 0 || goHome)
        {
            targetTransform = collectorHome.transform;
        }
        else
        {
            foreach (GameObject currency in currencies)
            {
                if (closestCurrency == null)
                {
                    closestCurrency = currency;
                }
                else
                {
                    if (GetDistanceFromCollectorTransform(closestCurrency) > GetDistanceFromCollectorTransform(currency))
                    {
                        closestCurrency = currency.gameObject;
                    }
                }
            }

            targetTransform = closestCurrency.transform;
        }
    }

    float GetDistanceFromCollectorTransform(GameObject other)
    {
        float distance = Vector3.Distance(gameObject.transform.position, other.transform.position);

        return distance;
    }

    public static void SendCollectorsHome()
    {
        goHome = true;
    }

    public static void DeployCollectors()
    {
        goHome = false;
    }
}
