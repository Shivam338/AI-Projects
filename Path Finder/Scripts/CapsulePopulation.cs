using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class CapsulePopulation : MonoBehaviour
{
    public GameObject BotPrefab;
    public int PopulationSize = 100;
    List<GameObject> Population = new List<GameObject>();
    public static float elapsed = 0;


    public int TrailTime = 20;
    int generation = 1;

    GUIStyle guiStyle = new GUIStyle();
    void OnGUI()
    {
        guiStyle.fontSize = 20;
        guiStyle.normal.textColor = Color.white;
        GUI.BeginGroup(new Rect(10, 10, 250, 150));
        GUI.Box(new Rect(0, 0, 140, 140), "Stats", guiStyle);
        GUI.Label(new Rect(10, 25, 200, 30), "Gen: " + generation, guiStyle);
        GUI.Label(new Rect(10, 50, 200, 30), string.Format("Time: {0:0.00}", elapsed), guiStyle);
        GUI.Label(new Rect(10, 75, 200, 30), "Population :" + Population.Count, guiStyle);
        GUI.EndGroup();
    }


    // Start is called before the first frame update
    void Start()
    {

        for (int i = 0; i < PopulationSize; i++)
        {
            Vector3 StartingPos = new Vector3(this.transform.position.x + Random.Range(-2, 2), this.transform.position.y, this.transform.position.z + Random.Range(-2, 2));
            GameObject bot = Instantiate(BotPrefab, StartingPos, this.transform.rotation);
            bot.GetComponent<CapsuleBrain>().Init();  // Gives Bots a Random Value for the single gene
            Population.Add(bot);
        }
    }

    // Update is called once per frame
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
        CapsuleBrain B = offspring.GetComponent<CapsuleBrain>();

        // Mutates 1 out of 100
        if (Random.Range(0, 100) == 1)
        {
            B.Init();
            B.bot_DNA.Mutate();
        }
        else
        {
            B.Init();
            // Generation of OffSpring 
            B.bot_DNA.OffSpringGen(Parent1.GetComponent<CapsuleBrain>().bot_DNA, Parent2.GetComponent<CapsuleBrain>().bot_DNA);
        }
        return offspring;
    }

    void BreedPopulation()
    {
        // Here in this statement we decide on what basis the list will be sorted which decides that on which basis the fitness is decided 
        List<GameObject> sortedList = Population.OrderBy(x => x.GetComponent<CapsuleBrain>().LifeSpan + x.GetComponent<CapsuleBrain>().WalkTime).ToList();

        Population.Clear();

        // Here we breeds the people with longest LifeSpan i.e we bread the lower half of the list 
        for (int i = (int)(sortedList.Count / 2.0f) - 1; i < sortedList.Count - 1; i++)
        {
            // we did it twice so to nake sure that it gives back the original population 
            Population.Add(Breed(sortedList[i], sortedList[i + 1]));
            Population.Add(Breed(sortedList[i + 1], sortedList[i]));
        }

        // Destroys Previous population and Parents
        for (int i = 0; i < sortedList.Count; i++)
        {
            Destroy(sortedList[i]);
        }
        generation++;
    }
}
