
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DeckManager))]
public class EnemyManager: MonoBehaviour, IPlayerManager
{
    DeckManager deckManager;
    GameManager gameManager;
    List<BaseCard> deck;
    
    IPlayerManager otherPlayer;
    
    [SerializeField] int energyGain = 1;
    [SerializeField] int cardGain = 2;

    public void StartCombat(GameManager manager) {
        gameManager = manager;
        gameObject.SetActive(true);
        deckManager = GetComponent<DeckManager>();
        deckManager.PrepareBattle(manager.groundTargets);
        deck = new List<BaseCard>();
        otherPlayer = manager.GetOtherPlayer(this);
    }

    public void StartTurn() {
        deckManager.energy += energyGain;
        for (int i = 0; i < cardGain; i++)
            deck.Add(deckManager.Draw());
        deckManager.interactable = true;
        for (int i = deck.Count - 1; i >= 0; i--)
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
                            return (dist, b.unit == null);
                        });
                        break;
                    case BaseCard.Target.Enemy:
                        bt = gameManager.groundTargets.FindBest((BoardTarget b) => {
                            var dist = (b.transform.position - transform.position).sqrMagnitude;
                            return (dist, b.unit != null && b.unit.team != deckManager);
                        });
                        break;
                    case BaseCard.Target.Friendly:
                        bt = gameManager.groundTargets.FindBest((BoardTarget b) => {
                            var dist = (b.transform.position - transform.position).sqrMagnitude;
                            return (dist, b.unit?.team == deckManager);
                        });
                        break;
                    case BaseCard.Target.Spawn:
                        bt = gameManager.groundTargets.FindBest((BoardTarget b) => {
                            var dist = (b.transform.position - transform.position).sqrMagnitude;
                            return (dist, b.unit == null && Utils.Vector3InBox(deckManager.transform.position, deckManager.spawnPositionLimit, b.transform.position));
                        });
                        break;
                }
                // Debug.Log("Enemy cast: " + bt + " " + deck[i].cardName);
                if (deck[i].target == BaseCard.Target.Global || bt != null)
                    if (deck[i].Cast(bt, deckManager))
                        deck.RemoveAt(i);
            }
        }
        var oppos = gameManager.groundTargets.transform.InverseTransformPoint(otherPlayer.GetPosition());
        for (int i = 0; i < deckManager.units.Count; i++)
        {
            var unit = deckManager.units[i];
            var upos = gameManager.groundTargets.transform.InverseTransformPoint(unit.transform.position);
            if (unit == null) {
                deckManager.units.RemoveAt(i);
                i--;
                continue;
            }
            if (unit.hasMoved) {
                unit.hasMoved = false;
                continue;
            }
            if ((oppos - upos).sqrMagnitude <= unit.attackDist * unit.attackDist) {
                otherPlayer.Damage(unit.damage);
                unit.AttackFX(otherPlayer.GetPosition());
                continue;
            }
            var bt = gameManager.groundTargets.FindBest((BoardTarget b) => {
                var dist = (b.transform.localPosition - upos).sqrMagnitude;
                return (dist, b.unit != null && b.unit.team != deckManager && dist < unit.attackDist * unit.attackDist);
            });
            if (bt != null) {
                // Debug.Log("a: " + bt);
                unit.Move(bt);
                unit.hasMoved = false;
                continue;
            }
            bt = gameManager.groundTargets.FindBest((BoardTarget b) => {
                var dist1 = (b.transform.localPosition - upos).sqrMagnitude;
                var dist2 = (b.transform.localPosition - oppos).sqrMagnitude;
                return (dist1 + dist2, b.unit == null && dist1 < unit.moveDist * unit.moveDist);
            });
            if (bt != null) {
                // Debug.Log("m: "+bt);
                unit.Move(bt);
                unit.hasMoved = false;
            }
        }
        EndTurn();
    }

    public void EndTurn() {
        deckManager.interactable = false;
        if (deckManager.units.Count == 0) {
            for (int i = 0; i < deck.Count; i++)
                if (deck[i]?.target == BaseCard.Target.Spawn)
                    goto Out;
            for (int i = 0; i < deckManager.drawPile.Count; i++)
                if (deckManager.drawPile[i]?.target == BaseCard.Target.Spawn)
                    goto Out;
            gameManager.Loose(this);
            return;
        }
        Out:
        gameManager.NextTurn();
    }

    public void EndCombat(bool won) {
        deckManager.interactable = false;
        if (!won) {
            deckManager.ClearBoard();
            gameObject.SetActive(false);
        }
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public void Damage(int damage) { }
}
