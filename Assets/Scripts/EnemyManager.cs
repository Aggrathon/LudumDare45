
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DeckManager))]
public class EnemyManager: MonoBehaviour, IPlayerManager
{
    DeckManager deckManager;
    GameManager gameManager;
    private List<BaseCard> deck;
    
    [SerializeField] protected int energyGain = 1;
    [SerializeField] protected int cardGain = 2;

    public void StartCombat(GameManager manager) {
        gameManager = manager;
        deckManager = GetComponent<DeckManager>();
        deckManager.PrepareBattle(manager.groundTargets);
        deck = new List<BaseCard>();
    }

    public void StartTurn() {
        deckManager.energy += energyGain;
        for (int i = 0; i < cardGain; i++)
            deck.Add(deckManager.Draw());
        deckManager.interactable = true;
        for (int i = 0; i < deck.Count; i++)
        {
            if (deck[i] == null) {
                deck.RemoveAt(i);
                continue;
            }
            if (deckManager.energy >= deck[i].cost) {
                BoardTarget bt = null;
                switch (deck[i].target)
                {
                    case BaseCard.Target.Empty:
                        bt = gameManager.groundTargets.FindBest((BoardTarget b) => {
                            var dist = (b.transform.position - transform.position).sqrMagnitude;
                            return dist + (b.unit != null? 100f : 0f);
                        });
                        break;
                    case BaseCard.Target.Enemy:
                        bt = gameManager.groundTargets.FindBest((BoardTarget b) => {
                            var dist = (b.transform.position - transform.position).sqrMagnitude;
                            return dist + (b.unit?.team != deckManager? -100f : 0f);
                        });
                        break;
                    case BaseCard.Target.Friendly:
                        bt = gameManager.groundTargets.FindBest((BoardTarget b) => {
                            var dist = (b.transform.position - transform.position).sqrMagnitude;
                            return dist + (b.unit?.team == deckManager? -100f : 0f);
                        });
                        break;
                    case BaseCard.Target.Spawn:
                        bt = gameManager.groundTargets.FindBest((BoardTarget b) => {
                            var dist = (b.transform.position - transform.position).sqrMagnitude;
                            return dist + (b.unit == null && Utils.Vector3InBox(deckManager.transform.position, deckManager.spawnPositionLimit, b.transform.position)? -100f : 0f);
                        });
                        break;
                }
                //Debug.Log("Enemy cast: " + bt + " " + deck[i].cardName);
                if (deck[i].Cast(bt, deckManager))
                    deck.RemoveAt(i);
            }
        }
        for (int i = 0; i < deckManager.units.Count; i++)
        {
            //TODO: try to move
        }
        EndTurn();
    }

    public void EndTurn() {
        deckManager.interactable = false;
        gameManager.Next();
    }

    public void EndCombat(bool won) {
        deckManager.interactable = false;
        if (!won)
            deckManager.ClearBoard();
    }


}
