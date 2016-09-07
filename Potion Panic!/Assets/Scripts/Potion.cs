using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Potion : PhysicsObject
{

    public Color color;
    public Sprite redPotionSprite;
    public Sprite greenPotionSprite;
    public Sprite bluePotionSprite;

    protected SpriteRenderer spriteRend;

    protected override void Awake()
    {
        base.Awake();
        color = Color.blue;
        spriteRend = GetComponent<SpriteRenderer>();
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
        base.OnTriggerEnter2D(other);
        if (other.gameObject.tag == "Vat")
        {
            ProcessPotion();
        }
    }

    private void ProcessPotion()
    {
        Destroy(this.gameObject);
    }

    public void SetColor(char col)
    {
        switch (col)
        {
            case 'r': color = Color.red; spriteRend.sprite = redPotionSprite; break;
            case 'g': color = Color.green; spriteRend.sprite = greenPotionSprite; break;
            case 'b': color = Color.blue; spriteRend.sprite = bluePotionSprite;  break;
        }
    }
}
