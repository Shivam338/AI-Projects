using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DNA : MonoBehaviour
{
    public float red , green, blue;
    public float LifeSpan;

    SpriteRenderer sRenderer;
    Collider2D sCollider;

    private void OnMouseDown()
    {
        LifeSpan = PopulationManager.elapsed;
        sRenderer.enabled = false;
        sCollider.enabled = false;
    }

    private void Start()
    {
        sRenderer = GetComponent<SpriteRenderer>();
        sCollider = GetComponent<Collider2D>();

        sRenderer.color = new Color(red, green, blue);
    }
}
