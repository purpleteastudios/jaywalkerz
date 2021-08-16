using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Debug=UnityEngine.Debug;
using System.Diagnostics;

public class pedestrianFunctions : MonoBehaviour
{
    //public GameMechanics IncreaseScoreScript;
    
    //Stats
    public string Name;
    public int Points;
    public bool isWalking;
    public bool isStopped;
    public bool isStuck;


    //Components
    public NavMeshAgent AgentComponent;
    public Animator AnimatorComponent;
    public pedestrianFunctions PedestrianScriptComponent;

    // City Destinations
    public GameObject DestinationsFolder;
    public List<GameObject> Destinations;

    //Other
    public GameObject Player;
    public GameObject Ragdoll;

    //Data
    public float DistanceFromPlayer;
    public float Speed;
    public float timer;
    public float timecheck;
    public Vector3 positionCheck;
    public GameObject AgentDestination;



    private float nextTurnTime;
     private Transform startTransform;


    // Start is called before the first frame update
    void Start()
    {
        positionCheck = this.transform.position;
        timer = 0.0f;
        timecheck = 0.0f;
        
        //IncreaseScoreScript = GameObject.Find("Scripts").GetComponent<GameMechanics>();
       //Get Components
       AgentComponent = this.gameObject.GetComponent<NavMeshAgent>();
       AnimatorComponent = this.gameObject.GetComponent<Animator>();
       PedestrianScriptComponent = this.gameObject.GetComponent<pedestrianFunctions>();
       

       //Get Destinations
       //Collect All (962 Total)
        foreach (Transform child in DestinationsFolder.transform)
            {
                Destinations.Add(child.gameObject);
            }
        
       



       
        


    }

    // Update is called once per frame
    void Update()
    {   
        timer += Time.deltaTime;
        //Debug.Log(Mathf.Round(timer));
        if(isStuck){
            AgentComponent.enabled = false;
            AgentComponent.enabled = true;
            int randomDestination = Random.Range(0, Destinations.Count);
            AgentComponent.destination = Destinations[randomDestination].transform.position;
            AgentDestination = Destinations[randomDestination];
            isStuck = false;
            timecheck = timer;

        }

        if(Mathf.Round(timer) % 5 == 0){
            //Debug.Log(Mathf.Round(timer));
            if(this.transform.position == positionCheck){
                isStuck = true;
                positionCheck = this.transform.position;

        }
        }


        Speed = this.transform.GetComponent<NavMeshAgent>().velocity.magnitude; 

        
        
       
        
        //Variables to Update Every Frame
        if(Player !=null){
            DistanceFromPlayer = Vector3.Distance(Player.transform.position, this.gameObject.transform.position);
        }
        

        //Walking to Random Destination
        NavMeshHit hit;
              if(NavMesh.SamplePosition(this.transform.position, out hit, 100f, NavMesh.AllAreas)){ 
                  this.transform.position = hit.position; 
                  
                  if(AgentComponent.remainingDistance < 2.0f){
           int randomDestination = Random.Range(0, Destinations.Count);
            AgentComponent.destination = Destinations[randomDestination].transform.position;
            AgentDestination = Destinations[randomDestination];
            
        }
              }         
       



            if(Speed <= 1.0f && timer > 10.0f){
                AnimatorComponent.SetBool("Idle", true);
                AnimatorComponent.SetBool("Walk", false);


                

            }else{
                AnimatorComponent.SetBool("Idle", false);
                AnimatorComponent.SetBool("Walk", true);
            }

        //Running from Player
         if(Player !=null){
        if(DistanceFromPlayer < 15.0f){
            //AnimatorComponent.SetTrigger("Run");
            AgentComponent.speed = 7.0f;
            RunFrom();
        } else{
            //AnimatorComponent.ResetTrigger("Run");
            //AnimatorComponent.SetTrigger("Walk");
            AgentComponent.speed = 1.5f;
        }
         }
    
    }

    void OnTriggerEnter(Collider collision){

         
         
    }

    void OnCollisionEnter(Collision collider){


           
        
    }

    public void RunFrom()
     {
     
         // store the starting transform
         startTransform = transform;
         
         //temporarily point the object to look away from the player
         this.transform.rotation = Quaternion.LookRotation(this.transform.position - Player.transform.position);
 
         //Then we'll get the position on that rotation that's multiplyBy down the path (you could set a Random.range
         // for this if you want variable results) and store it in a new Vector3 called runTo
         Vector3 runTo = transform.position + transform.forward * 5.0f;
         //Debug.Log("runTo = " + runTo);
         
         //So now we've got a Vector3 to run to and we can transfer that to a location on the NavMesh with samplePosition.
         
         NavMeshHit hit;    // stores the output in a variable called hit
 
         // 5 is the distance to check, assumes you use default for the NavMesh Layer name
         NavMesh.SamplePosition(runTo, out hit, 5, 1 << NavMesh.GetNavMeshLayerFromName("Default")); 
         //Debug.Log("hit = " + hit + " hit.position = " + hit.position);
 
 
         // reset the transform back to our start transform
         transform.position = startTransform.position;
         transform.rotation = startTransform.rotation;
 
         // And get it to head towards the found NavMesh position
         AgentComponent.SetDestination(hit.position);
     }


}