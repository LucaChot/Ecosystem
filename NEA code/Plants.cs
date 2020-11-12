using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plants : MonoBehaviour
{
    int leaf = 20;
    int roots;
    int seedDispersal;
    float energyStoreRate;
    float drownThreshold;
    int minSeeds;
    int maxSeeds;
    int seedRange;

    void Start()
    {
        StartCoroutine("Live");
    }

    IEnumerator Live()
    {
        yield return new WaitForSeconds(10f);
        Destroy(this.gameObject);

    }
}
