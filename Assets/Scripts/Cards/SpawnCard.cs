using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="Cards/SpawnCard")]
public class SpawnCard : BaseCard
{
    public Unit unit;

    public override string GetDescription() {
        return "Summon a " + unit.unitName;
    }

    public override bool Cast(BoardTarget target, DeckManager manager) {
        if (target == null) {
            manager.Alert("Must target the board!");
            return false;
        }
        if (target.unit != null) {
            manager.Alert("Space is already occupied!");
            return false;
        } else {
            //TODO: check if close enough
            Instantiate(unit, target.transform.position, Quaternion.identity).Setup(manager, target);
            return true;
        }
    }
}
