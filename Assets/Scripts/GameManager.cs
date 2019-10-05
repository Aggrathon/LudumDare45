using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    [SerializeField] PlayerManager player;
    IPlayerManager enemy;

    private void Start() {
        player.StartCombat(this);
        //TODO: Enemies
        Next();
    }

    public void Next() {
        if (enemy == null) {
            //TODO: Next story step
        }
        //TODO: Implement next
        player?.StartTurn();
        enemy?.StartTurn();
    }

    public void Loose(IPlayerManager player) {
        if (this.player == (Object)player) {
            enemy.EndCombat(true);
            player.EndCombat(false);
            enemy = null;
        } else if (enemy == player) {
            enemy.EndCombat(false);
            player.EndCombat(true);
            enemy = null;
        }
    }
}
