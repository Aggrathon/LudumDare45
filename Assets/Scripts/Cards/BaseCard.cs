using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="Cards/BaseCard")]
public class BaseCard : ScriptableObject
{

    public string cardName;
    public int cost;
    public Sprite sprite;

    public BaseCard[] upgrades;
    
    public virtual string GetDescription() {
        return "This is a test card";
    }

    public virtual bool Cast(BoardTarget target) {
        Debug.Log("Casted 'Test Card' on "+target);
        return true;
    }
}
