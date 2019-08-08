using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

[RequireComponent(typeof(ThirdPersonCharacter))]
public class WalkingBrain : MonoBehaviour
{
    public int DNAlength = 1;
    public float LifeSpan;
    public WalkingPopulationManager DistanceCovered;

    private WalkingBrain Movement;
    public WalkingDNA Walkingdna;
    public float DistanceTravelled; 

    private ThirdPersonCharacter Bot;
    private Vector3 BotMovement;

    Vector3 StartPos;
   

    bool Alive = true;

    void Start()
    {
        Movement = GetComponent<WalkingBrain>();
    }
     
    // Declare Player as Dead whenever Player falls from the Path 
    void OnCollisionEnter(Collision obj)
    {
        if(obj.gameObject.tag == "dead")
        {
            Movement.enabled = !Movement.enabled;
            Alive = false;
        }
    }

    public void Init()
    {
        // 0 Forward
        // 1 Back
        // 2 Left
        // 3 Right
        // 4 Crouch

    // Here its going to generate a no. from 0-3 
    // This no. will make the Bot move accordingly
        Walkingdna = new WalkingDNA(DNAlength, 5);
        Bot = GetComponent<ThirdPersonCharacter>();
        LifeSpan = 0;
        StartPos = transform.position;
        Alive = true;
    }

    // Movements
    private void FixedUpdate()
    {
        float Horizontal = 0;
        float Vertical = 0;

        bool crouch = false;
        bool jump = false;

        if (Walkingdna.GetGene(0) == 0)
            Vertical = 1; // Forward 
        else if (Walkingdna.GetGene(0) == 1)
            Vertical = -1; // Backwards
        else if (Walkingdna.GetGene(0) == 2)
            Horizontal = 1; //Right
        else if (Walkingdna.GetGene(0) == 3)
            Horizontal = -1; // Left 
        else if (Walkingdna.GetGene(0) == 4)
            crouch = true;

            BotMovement = Vertical * Vector3.forward + Horizontal * Vector3.right;
        Bot.Move(BotMovement, crouch, jump); // we added false because we dont have any Jump movement 

        if (Alive)
        {
            LifeSpan += Time.deltaTime;
            DistanceTravelled = Vector3.Distance(transform.position, StartPos);
            
        }
    }
}
