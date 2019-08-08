using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WalkingPopulationManager : MonoBehaviour
{
    public GameObject BotPrefab;
    public int PopulationSize = 100;
    List<GameObject> population = new List<GameObject>();
    public static float elapsed = 0;

    Vector3 LastPos;

    public int TrailTime = 5;
    int generation = 1;

    private float LongestDistance;


    GUIStyle guiStyle = new GUIStyle();
    void OnGUI()
    {
        guiStyle.fontSize = 20;
        guiStyle.normal.textColor = Color.white;
        GUI.BeginGroup(new Rect(10, 10, 250, 150));
        GUI.Box(new Rect(0, 0, 140, 140), "Stats", guiStyle);
        GUI.Label(new Rect(10, 25, 200, 30), "Gen: " + generation, guiStyle);
        GUI.Label(new Rect(10, 50, 200, 30), string.Format("Time: {0:0.00}", elapsed), guiStyle);
        GUI.Label(new Rect(10, 75, 200, 30), "Population :" + population.Count, guiStyle);
        GUI.Label(new Rect(10, 100, 200, 30), "Longest Distance :" + LongestDistance, guiStyle);
        GUI.EndGroup();
    }

    void Start()
    {
        for(int i =0; i< PopulationSize; i++)
        {
            Vector3 StartingPos = new Vector3(this.transform.position.x + Random.Range(-2, 2), this.transform.position.y, this.transform.position.z + Random.Range(-2, 2));
            GameObject b = Instantiate(BotPrefab, StartingPos, this.transform.rotation);
            b.GetComponent<WalkingBrain>().Init();  // Gives Bots a Random Value for the single gene
            population.Add(b);
        }
    }

    void Update()
    {
       
        elapsed += Time.deltaTime;
        if (elapsed > TrailTime)
        {
            elapsed = 0;
         
            // new population will be from the person left at the last 
            BreedPopulation();
        }
    }

    GameObject Breed(GameObject Parent1, GameObject Parent2)
    {
        Vector3 StartingPos = new Vector3(this.transform.position.x + Random.Range(-2, 2), this.transform.position.y, this.transform.position.z + Random.Range(-2, 2));
        GameObject offspring = Instantiate(BotPrefab, StartingPos, this.transform.rotation);
       
        // Gets Access to the Brain 
        WalkingBrain B = offspring.GetComponent<WalkingBrain>();

        // Mutates 1 out of 100
        if (Random.Range(0, 100) == 1)
        {
            B.Init();
            B.Walkingdna.Mutate();
        }
        else
        {
            B.Init();
            // Generation of OffSpring 
            B.Walkingdna.OffSpringGen(Parent1.GetComponent<WalkingBrain>().Walkingdna, Parent2.GetComponent<WalkingBrain>().Walkingdna);
        }
        return offspring;
    }

    void BreedPopulation()
    {
        // Here in this statement we decide on what basis the list will be sorted which decides that on which basis the fitness is decided 
        // Here fitness is decided by the DistanceTravelled. " GetComponent<WalkingBrain>().DistanceTravelled "
        List<GameObject> sortedList = population.OrderBy(x => x.GetComponent<WalkingBrain>().LifeSpan).ToList();

        // Gets us the best distance
        LongestDistance = sortedList[0].GetComponent<WalkingBrain>().DistanceTravelled;

        population.Clear();

        // Here we breeds the people with longest LifeSpan i.e we bread the lower half of the list 
        for (int i = (int)(sortedList.Count / 2.0f) - 1; i < sortedList.Count - 1; i++)
        {
            // we did it twice so to nake sure that it gives back the original population 
            population.Add(Breed(sortedList[i], sortedList[i + 1]));
            population.Add(Breed(sortedList[i + 1], sortedList[i]));
        }

        // Destroys Previous population and Parents
        for (int i = 0; i < sortedList.Count; i++)
        {
            Destroy(sortedList[i]);
        }
        generation++;
    }

}
