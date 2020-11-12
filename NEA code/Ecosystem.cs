using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ecosystem : MonoBehaviour
{
    public GameObject Animal;
    public GameObject Plant;
    public LayerMask unwalkableMask;
    public LayerMask AnimalMask;
    public LayerMask PlantMask;

    private void Start()
    {
        StartCoroutine("Spawning", 2f);
    }

    IEnumerator Spawning(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            int x = Random.Range(0, 200);
            int z = Random.Range(0, 200);
            Vector3 Pos = new Vector3(x, 1, z);
            createAnimal(Plant, Pos);
        }
    }
    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            Debug.Log("Done!");
            int x = Random.Range(0, 200);
            int z = Random.Range(0, 200);
            Vector3 Pos = new Vector3(x, 1, z);
            createAnimal(Animal, Pos);
        }
    }

    public void createAnimal(GameObject creature, Vector3 Pos)
    {
        while (true)
        {
            
            if (!(Physics.CheckSphere(new Vector3(Pos.x,0,Pos.z), 0.5f, unwalkableMask))){
                Quaternion upright = new Quaternion(0, 0, 0, 0);
                GameObject child = Instantiate(creature, Pos, upright) as GameObject;
                if(creature == Animal)
                {
                    Animal animal = child.GetComponent<Animal>();
                    animal.eco = this;
                }
                break;
            }
            Pos.x = Random.Range(0, 200);
            Pos.z = Random.Range(0, 200);
        }    
    }
}
