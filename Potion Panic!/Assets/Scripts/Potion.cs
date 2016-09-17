﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Potion : PhysicsObject
{

    public char color;
    public Sprite redPotionSprite;
    public Sprite greenPotionSprite;
    public Sprite bluePotionSprite;

    protected SpriteRenderer spriteRend;

    protected override void Awake()
    {
        base.Awake();
        color = 'b';
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
        if (other.gameObject.tag.Substring(0,3) == "Vat")
        {
            string vatTag = other.gameObject.tag;
            ProcessPotion(vatTag);
        }
    }

    private void ProcessPotion(string vatTag)
    {
        Vat vat = GameObject.FindGameObjectWithTag(vatTag).GetComponent<Vat>();
        vat.ProcessPotion(color);
        Destroy(this.gameObject);
    }

    public void SetColor(char col)
    {
        color = col;
        switch (color)
        {
            case 'r': spriteRend.sprite = redPotionSprite; break;
            case 'g': spriteRend.sprite = greenPotionSprite; break;
            case 'b': spriteRend.sprite = bluePotionSprite;  break;
        }
    }
}
