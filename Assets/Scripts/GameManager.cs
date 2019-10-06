using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    [SerializeField] PlayerManager player;
    [SerializeField] Transform enemyHolder;
    IPlayerManager enemy;
    int enemyIndex;
    bool playersTurn;

    public Board groundTargets;

    private void Start() {
        enemyIndex = 0;
        enemy = null;
        Next();
    }

    public void Next() {
        groundTargets.Unlight();
        if (enemy == null) {
            if (enemyHolder.childCount <= enemyIndex) {
                Debug.LogWarning("Player Won");
                //TODO: Win
                return;
            }
            var en = enemyHolder.GetChild(enemyIndex);
            en.gameObject.SetActive(true);
            enemy = en.GetComponent<EnemyManager>();
            enemyIndex++;
            playersTurn = false;
            enemy.StartCombat(this);
            player.StartCombat(this);
        }
        if (playersTurn) {
            playersTurn = false;
            player?.StartTurn();
        } else {
            playersTurn = true;
            enemy?.StartTurn();
        }
    }

    public void Loose(IPlayerManager player) {
        if (this.player == (Object)player) {
            enemy.EndCombat(true);
            enemy = null;
            player.EndCombat(false);
        } else if (enemy == player) {
            enemy.EndCombat(false);
            enemy = null;
            player.EndCombat(true);
        }
    }
}
