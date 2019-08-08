using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotDNA 
{
    List<int> genes = new List<int>(); // genes is the character trait which is inherited

    int DNALength = 0; // it is the chromosome here i.e it is the total size of the list 
    int MaxValue = 0;

    public BotDNA(int Length, int Value)
    {
        DNALength = Length;
        MaxValue = Value;
        SetRandom();
    }

    public void SetInt(int pos , int Value)
    {
        genes[pos] = Value;
    }

    public void SetRandom()
    {
        genes.Clear();
        for (int i = 0; i < DNALength; i++)
        {
            genes.Add(Random.Range(0, MaxValue));
        }
    }

    public void OffSpringGen(BotDNA Parent1, BotDNA Parent2)
    {
        for (int i = 0; i < DNALength; i++)
        {
            // OffSprings is made in such a way that, it has half of its genes from Parent1 and the other half from the Parent2
            if (i < (DNALength / 2.0f))
            {
                int c = Parent1.genes[i];
                genes[i] = c;
            }
            else
            {
                int c = Parent2.genes[i];
                genes[i] = c;
            }
        }
    }

    public void Mutate()
    {
        genes[Random.Range(0, DNALength)] = Random.Range(0, MaxValue);
    }

    public int GetGene(int pos)
    {  // To get value of the gene placed inside the list 
        return genes[pos];
    }
}
