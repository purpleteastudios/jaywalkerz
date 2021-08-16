using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class pedestrianSpawn : MonoBehaviour
{
    //Variables
    //Folders
    public GameObject CharactersFolder;
    public GameObject ClonesFolder;
    public GameObject SpawnPositionsFolder;

    //Lists
    public List<GameObject> NPCTemplates;
    public List<GameObject> SpawnPositions;

    //Settings
    public int MaxPedestrians;

    // Start is called before the first frame update
    void Start()
    {
        //Create NPC Template Dictionary
        //CharactersFolder = GameObject.Find("CharacterswithRagdoll");
        ClonesFolder = GameObject.Find("Pedestrians");
        foreach (Transform child in CharactersFolder.gameObject.transform)
            {
                NPCTemplates.Add(child.gameObject);
            }

        

        //Set Pedestrian Spawn Limit
        MaxPedestrians = 100;

        //Collect All Spawn Positions (962 Total)

        foreach (Transform child in SpawnPositionsFolder.transform)
            {
                SpawnPositions.Add(child.gameObject);
            }
        

            

        //Instantiate NPCs in World to Limit Loop
        for (int i = 0; i < MaxPedestrians; i += 1) {
            //Variables
            int randomNPC = Random.Range(0,NPCTemplates.Count);
            int randomIndex = Random.Range(0,SpawnPositions.Count);
            GameObject randomSpawnGameObject = SpawnPositions[randomIndex];
            BoxCollider randomSpawnGameObjectBox = randomSpawnGameObject.GetComponent<BoxCollider>();
            Vector3 centreOfObject = new Vector3(randomSpawnGameObject.transform.position.x + randomSpawnGameObjectBox.center.x, randomSpawnGameObject.transform.position.y + randomSpawnGameObjectBox.center.y, randomSpawnGameObject.transform.position.z + randomSpawnGameObjectBox.center.z);

            //Debug
            //randomSpawnGameObject.transform.GetComponent<Renderer>().material.color = Color.red;
            
            //Instantiate NPC
            GameObject spawnedNPC = Instantiate(NPCTemplates[randomNPC], centreOfObject, NPCTemplates[randomNPC].transform.rotation);

            //Destroy NPC if not set correctly
            if (randomSpawnGameObjectBox.bounds.Intersects(spawnedNPC.transform.GetComponent<BoxCollider>().bounds))
                    {
              NavMeshHit hit;
              if(NavMesh.SamplePosition(spawnedNPC.transform.position, out hit, 100f, NavMesh.AllAreas)){ 
                  spawnedNPC.transform.position = hit.position;
                  SpawnPositions.RemoveAt(randomIndex);
                 }
          }
                     else{
                        Destroy(spawnedNPC);
                        i-=1;
                    }

                  /*   if (Physics.OverlapSphere(spawnedNPC.transform.position, 0.1f) != null)
                    {
                        Destroy(spawnedNPC);
                        i-=1;
                    } */
            

            //Set NPC properties
            spawnedNPC.transform.parent = ClonesFolder.transform;
            spawnedNPC.gameObject.GetComponent<NavMeshAgent>().enabled = true;
            spawnedNPC.gameObject.GetComponent<Animator>().enabled = true;
            spawnedNPC.gameObject.GetComponent<pedestrianFunctions>().enabled = true;

        }
        
    }

    // Update is called once per frame
    void Update()
    {

    }

}
