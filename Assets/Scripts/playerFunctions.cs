using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;
using UnityEngine.SceneManagement;

public class playerFunctions : MonoBehaviour
{

    //Variables
    //Stats
    public int SessionScore;
    public int SessionKills;
    public List<string> TopScores;
    public List<int> TopScoresInt;
    public string TopScoresString;
    public bool isGameOver;

    //UI Elements
    public TextMeshProUGUI InGameScoreUI;
    public TextMeshProUGUI InGameKillsUI;
    public TextMeshProUGUI FinalScoreUI;
    public TextMeshProUGUI TotalKillsUI;
    public TextMeshProUGUI TopScoresUI;

    //UI GameObjects
    public GameObject InGameScreen;
    public GameObject GameOverScreen;


    // Start is called before the first frame update
    void Start()
    {
        SessionKills = 0;
        SessionScore = 0;
        GameOverScreen.SetActive(false);
        //PlayerPrefs.SetString("TopScores", "1,2,3");
        TopScoresString = PlayerPrefs.GetString("TopScores", "1,2,3");
        TopScores = new List<string>();
        TopScoresInt = new List<int>();



        
    }

    // Update is called once per frame
    void Update()
    {

    }

   void OnCollisionEnter(Collision collision) //For Buildings and objects
    {

        if(collision.gameObject.name.Contains("Bld_")){
            //Debug.Log("Collision with Building");
            if(!isGameOver){
                GameOver(SessionScore, SessionKills);
            }
            
        }
    } 

     void OnTriggerEnter(Collider collision) // For people 
    {
        if(collision.name.Contains("Character_") && collision.name.Contains("_01")){
            //Debug.Log("Collided with Pedestrian");
            
            
            //Increase Score
            int points = collision.GetComponent<pedestrianFunctions>().Points;
            IncreaseScore(points);
            Destroy(collision.gameObject);

            


            //TODO: RagDoll Pedestrian
            /* collision.GetComponent<Animator>().enabled=false;
            collision.GetComponent<NavMeshAgent>().enabled=false;
            collision.GetComponent<pedestrianFunctions>().enabled=false;
            //collision.GetComponent<Collider>().isTrigger = false;
             */
            
   
        }


                
        }

        public void IncreaseScore(int points){
            SessionScore += points;
            SessionKills += 1;
            InGameScoreUI.text = SessionScore.ToString();
            InGameKillsUI.text = SessionKills.ToString();
            

        }

        public void GameOver(int finalScore, int finalKills){
            Debug.Log("Game Over");
            this.GetComponent<carFunctions>().enabled = false;
            InGameScreen.SetActive(false);
            GameOverScreen.SetActive(true);
            FinalScoreUI.text = SessionScore.ToString();
            TotalKillsUI.text = SessionKills.ToString();
            //TopScores
            foreach (var item in TopScoresString.Split(','))
            {
                TopScores.Add(item);
            }
            TopScores.Add(SessionScore.ToString());
            
            foreach (var item in TopScores)
            {
               TopScoresInt.Add(int.Parse(item)); 
            }
            
            TopScoresInt.Sort();
            TopScoresInt.Reverse();

            if(TopScoresInt.Count >= 5){
                TopScoresInt.Remove(TopScoresInt.Count - 1);
            }
    
 

            TopScoresUI.text = TopScoresInt[0] + "\n" + TopScoresInt[1] + "\n" + TopScoresInt[2] + "\n" + TopScoresInt[3] + "\n" + TopScoresInt[4];
            PlayerPrefs.SetString("TopScores", string.Join(",", TopScoresInt));
            isGameOver = true;
            
            

        }

        public void OnClick(){
            Debug.Log("Restart");
            SceneManager.LoadScene(0, LoadSceneMode.Single);
        }

     
        


        
}

