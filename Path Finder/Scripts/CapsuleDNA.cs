using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapsuleDNA
{
    List<int> Genes = new List<int>(); // genes is the character trait which is inherited

    int DNALength = 0; // it is the chromosome here i.e it is the total size of the list 
    int MaxValue = 0;

    public CapsuleDNA(int Length, int Value)
    {
        DNALength = Length;
        MaxValue = Value;
        SetRandom();
    }

    public void SetInt(int pos, int Value)
    {
        Genes[pos] = Value;
    }

    public void SetRandom()
    {
        Genes.Clear();
        for (int i = 0; i < DNALength; i++)
        {
            Genes.Add(Random.Range(0, MaxValue));
        }
    }

    public void OffSpringGen(CapsuleDNA parent1, CapsuleDNA parent2)
    {
        for (int i = 0; i < DNALength; i++)
        {
            // OffSprings is made in such a way that, it has half of its genes from Parent1 and the other half from the Parent2
            if (i < (DNALength / 2.0f))
            {
                int c = parent1.Genes[i];
                Genes[i] = c;
            }
            else
            {
                int c = parent2.Genes[i];
                Genes[i] = c;
            }
        }
    }

    public void Mutate()
    {
        Genes[Random.Range(0, DNALength)] = Random.Range(0, MaxValue);
    }

    public int GetGene(int pos)
    {  // To get value of the gene placed inside the list 
        return Genes[pos];
    }

}
