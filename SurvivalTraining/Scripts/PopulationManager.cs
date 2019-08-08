using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using System;

public class PopulationManager : MonoBehaviour
{
    public GameObject person;
    public int PopulationSize = 10;
    List<GameObject> Population = new List<GameObject>();
    public static float elapsed = 0;

    int trailTime = 10;
    int generation = 1;

     GUIStyle guiStyle = new GUIStyle();
     void OnGUI()
    {
        guiStyle.fontSize = 50;
        guiStyle.normal.textColor = Color.white;
        GUI.Label(new Rect(10, 10, 100, 20), "Generation: " + generation, guiStyle);
        GUI.Label(new Rect(10, 65, 100, 20), "Trial Time: " + (int)elapsed, guiStyle);
    }

    // Start is called before the first frame update
    void Start()
    {
        for(int i=0; i<= PopulationSize; i++)
        {
            Vector3 pos = new Vector3(UnityEngine.Random.Range(-14, 14), UnityEngine.Random.Range(-4,4), 0);
            GameObject go = Instantiate(person, pos, Quaternion.identity);

            // Random Colour Values
            go.GetComponent<DNA>().red = UnityEngine.Random.Range(0.0f, 1.0f);
            go.GetComponent<DNA>().green = UnityEngine.Random.Range(0.0f, 1.0f);
            go.GetComponent<DNA>().blue = UnityEngine.Random.Range(0.0f, 1.0f);
            Population.Add(go);
        }
    }

     void Update()
    {

        elapsed += Time.deltaTime;
        if (elapsed > trailTime)
        {
            elapsed = 0;

            // new population will be from the person left at the last 
            BreedNewPopulation();
        }
     }



    void BreedNewPopulation()
    {
        List<GameObject> newPopulation = new List<GameObject>();

        // Sort in Decreasing order such that Person with longest lifeSpan is at the bottom 
        List<GameObject> sortedList =Population.OrderBy(x => x.GetComponent<DNA>().LifeSpan).ToList();  // "x" stores the lifespan of the gameobject and compares it with the rest

        // We clear the population so that the new generation can be stored
        Population.Clear();

        // Here we breeds the people with longest LifeSpan i.e we bread the lower half of the list 
        for (int i = (int) (sortedList.Count / 2.0f)-1; i < sortedList.Count-1; i++)
        {
            // we did it twice so to nake sure that it gives back the original population 
            Population.Add(Breed(sortedList[i], sortedList[i + 1]));
            Population.Add(Breed(sortedList[i + 1], sortedList[i]));
        }

        for(int i=0; i< sortedList.Count; i++)
        {
            Destroy(sortedList[i]);
        }
        generation++;
    }

    // Next Generation 
     GameObject Breed(GameObject Parent1, GameObject Parent2)
    {
        Vector3 Pos = new Vector3(UnityEngine.Random.Range(-14, 14), UnityEngine.Random.Range(-4, 4), 0);
        GameObject Offspring = Instantiate(person, Pos, Quaternion.identity);

        // DNA of Parents 
        DNA Parent1_DNA = Parent1.GetComponent<DNA>();
        DNA Parent2_DNA = Parent2.GetComponent<DNA>();

        // Breeding 
        // if Random value is less than 5 then it gets parent1's DNA and if greater then parent2's DNA
        // So here we have a 50-50 chance which implies to our real world scenario
        // This is what makes the Genetic Algo works.
        Offspring.GetComponent<DNA>().red = UnityEngine.Random.Range(0, 10) < 5 ? Parent1_DNA.red : Parent2_DNA.red;
        Offspring.GetComponent<DNA>().green = UnityEngine.Random.Range(0, 10) < 5 ? Parent1_DNA.green : Parent2_DNA.green;
        Offspring.GetComponent<DNA>().blue = UnityEngine.Random.Range(0, 10) < 5 ? Parent1_DNA.blue : Parent2_DNA.blue;
        return Offspring;
    }
}
