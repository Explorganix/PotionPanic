using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Potion : PhysicsObject
{

    public Color color;

    protected override void Awake()
    {
        color = Color.blue;
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Vat")
        {
            ProcessPotion();
        }
        base.OnTriggerEnter2D(other);
    }

    private void ProcessPotion()
    {
        Destroy(this.gameObject);
    }

    public void SetColor(char col)
    {
        switch (col)
        {
            case 'r': color = Color.red; break;
            case 'g': color = Color.red; break;
            case 'b': color = Color.red; break;
        }
    }
}
