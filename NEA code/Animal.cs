using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Animal : MonoBehaviour
{
    int speed = 10;
    int sight;
    float repDrive;
    float thirstHungerRate = 1f;
    int coneVision;
    float maxhunger = 10f;
    float hunger;
    float thirst;
    float repUrge;
    bool isAlive = true;
    bool isBusy = false;
    public bool arrived = false;
    FieldOfView fov;
    Transform target;
    Vector3 targetpos;
    public LayerMask unwalkableMask;
    public LayerMask AnimalMask;
    public LayerMask PlantMask;
    public Ecosystem eco;
    Animal mate;
    bool found = false;
    public GameObject species;

    public HealthScript hungerbar;

    private void Awake()
    {
        fov = GetComponent<FieldOfView>();
        hunger = maxhunger;
        thirst = maxhunger;
        repUrge = maxhunger;
        repDrive = 7* maxhunger / 8;
        hungerbar.SetMaxHealth(maxhunger);
    }

    void Start()
	{
		StartCoroutine("Refreshing", .2f);
	}

    IEnumerator Refreshing(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            Refresh();
        }
    }

    void Refresh()
    {
        found = false;
        if (hunger < 0 || thirst < 0)
        {
            Destroy(gameObject);
        }
        hunger -= thirstHungerRate * Time.deltaTime;
        thirst -= thirstHungerRate * Time.deltaTime;
        repUrge -= thirstHungerRate * Time.deltaTime;
        hungerbar.SetHealth(repUrge);
        //Debug.Log(thirst);
        if (!isBusy)
        {
            if(repUrge < repDrive)
            {
                target = fov.FindVisibleTargets(AnimalMask);
                //Debug.Log(target);
                if (target != null)
                {
                    found = true;
                    mate = target.gameObject.GetComponent<Animal>();
                    SendMateRequest(mate);

                }
            }
            if (found != true)
            {
                fov.FindVisibleTargets(PlantMask);
                target = fov.closestTarget;
                if(target != null)
                {
                    targetpos = target.position;
                    StartCoroutine("FollowPath", "Plant");
                }              

            }
        }
    }

    void SendMateRequest(Animal mate)
    {
        if (!mate.isBusy && mate.repUrge < mate.repDrive)
        {
            mate.isBusy = true;
            isBusy = true;
            mate.mate = this;
            Vector3 dirToTarget = (target.position - transform.position).normalized;
            float dstToTarget = Vector3.Distance(transform.position, target.position);
            targetpos = transform.position + dirToTarget * dstToTarget / 2;
            mate.targetpos = targetpos;
            StartCoroutine("FollowPath", "Animal0");
            mate.StartCoroutine("FollowPath", "Animal1");
        }
    }

    IEnumerator FollowPath(string tag)
    {
        while (true)
        {
            if(targetpos == transform.position)
            {
                setArrived();
                //Debug.Log(arrived);
                if (tag == "Animal0" || tag == "Animal1")
                {
                    yield return new WaitUntil(checkArrived);
                    yield return new WaitForSeconds(0.5f);
                    
                    if (tag == "Animal0")
                    {
                        
                        eco.createAnimal(species, transform.position);
                    }
                }
                else
                {
                    Destroy(target.gameObject);
                }
                HasArrived();
                yield break;

            }           
            transform.position = Vector3.MoveTowards(transform.position, targetpos, speed * Time.deltaTime);
            yield return null;
        }

    }
    bool checkArrived()
    {
        if(mate.arrived == true){
            return true;
        }
        else
        {
            return false;
        }
    }
    void setArrived()
    {
        arrived = true;
        Debug.Log("Set");
    }

    public void HasArrived()
    {
        isBusy = false;
        arrived = false;
        repUrge = maxhunger;
        Debug.Log("Done");
        StopCoroutine("FollowPath");
    }
}
