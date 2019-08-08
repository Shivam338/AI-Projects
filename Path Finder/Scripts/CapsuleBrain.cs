using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapsuleBrain : MonoBehaviour
{// DNA=2 because it has to make 2 decisions to make
    // First , when it sees the platform
    // Second  , when it does not see any platform
    int DNALength = 2;

    public float LifeSpan;
    public float WalkTime = 0; // So that our bots keep moving 
    public CapsuleDNA bot_DNA;
    public GameObject Eyes_down;
    public GameObject Eyes_straight;

    bool Alive = true;
    bool SeePlatform = true;

    Vector3 StartPos;

    void OnCollisionEnter(Collision obj)
    {
        if (obj.gameObject.tag == "dead")
        {
            Alive = false;
        }
    }

    public void Init()
    {
        // 0 Forward
        // 1 Left
        // 2 Right

        // Here its going to generate a no. from 0-3 
        // This no. will make the Bot move accordingly
        bot_DNA = new CapsuleDNA(DNALength, 3);
        LifeSpan = 0;
        StartPos = transform.position;
        Alive = true;
    }

    void Update()
    {
        if (!Alive)
            return;

        // Debug.DrawRay(Eyes_down.transform.position, Eyes_down.transform.forward * 10, Color.green, 20);
        // Debug.DrawRay(Eyes_straight.transform.position, Eyes_straight.transform.forward * 2, Color.red, 20);

        Debug.Log("1233");

        SeePlatform = false;

        RaycastHit hit;
        if (Physics.Raycast(Eyes_down.transform.position, Eyes_down.transform.forward * 10, out hit))
        {
            if (hit.collider.gameObject.tag == "Platform")
            {
                SeePlatform = true;
            }
        }

        else if (Physics.Raycast(Eyes_straight.transform.position, Eyes_straight.transform.forward * 10, out hit))
        {
            if (hit.collider.gameObject.tag == "Platform")
            {
                SeePlatform = true;
            }
        }

        LifeSpan = BotPopulation.elapsed;

        float turn = 0;
        float move = 0;


        if (SeePlatform)
        {
            // If the bot sees the ground and has gene[0] stored in it.
            // gene[0] means when BOT IS ABLE TO SEE THE GROUND.
            //  gene[1] means when BOT IS UNABLE TO SEE THE GROUND.
            // That's why we made DNALength= 2
            if (bot_DNA.GetGene(0) == 0)
            {
                move = 1;  // Forward
                WalkTime += 1;
            }


            else if (bot_DNA.GetGene(0) == 1)
                turn = -90; // Left

            else if (bot_DNA.GetGene(0) == 2)
                turn = 90;  // Right
        }
        else
        {
            if (bot_DNA.GetGene(1) == 0)
            {
                move = 1;
                WalkTime += 1;
            }
            else if (bot_DNA.GetGene(1) == 1)
                turn = -90;

            else if (bot_DNA.GetGene(1) == 2)
                turn = 90;
        }

        this.transform.Translate(0, 0, move * 0.5f);
        this.transform.Rotate(0, turn, 0);
    }
}
