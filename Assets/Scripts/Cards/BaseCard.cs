﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="Cards/BaseCard")]
public class BaseCard : ScriptableObject
{

    public string cardName;
    public int cost;
    public Sprite sprite;
    public Color color;

    public BaseCard[] upgrades;
    
    public virtual string GetDescription() {
        return "This is a test card";
    }

    public virtual bool Cast(BoardTarget target, DeckManager manager) {
        Debug.Log("Casted 'Test Card' on "+target);
        manager.Discard(this);
        return true;
    }
}
