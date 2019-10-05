using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="Cards/MissileCard")]
public class MissileCard : BaseCard
{
    public int damage;

    public override string GetDescription() {
        return "Deal " + damage + " damage";
    }

    public override bool Cast(BoardTarget target, DeckManager manager) {
        if (target == null || target.unit == null) {
            manager.Alert("Invalid target!");
            return false;
        } else {
            if (target.unit.team == manager) {
                manager.Alert("Target is friendly!");
                return false;
            }
            //TODO: deal damage
            manager.Discard(this);
            return true;
        }
    }
}
