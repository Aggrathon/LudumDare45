using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DeckManager))]
public class PlayerManager : MonoBehaviour, IPlayerManager
{
    
    DeckManager deckManager;

    [SerializeField] protected GameObject endTurnButton;

    GameManager gameManager;

    GameObject headItem;
    GameObject chestItem;
    GameObject handItem;

    private void Awake() {
        endTurnButton?.SetActive(false);
        deckManager = GetComponent<DeckManager>();
    }

    public int energyGain = 1;
    public int cardGain = 2;
    public int health;

    public void StartCombat(GameManager manager) {
        gameManager = manager;
        deckManager.PrepareBattle();
        endTurnButton?.SetActive(false);
    }

    public void StartTurn() {
        if (health <= 0) {
            gameManager.Loose(this);
            return;
        }
        deckManager.energy += energyGain;
        for (int i = 0; i < cardGain; i++)
            deckManager.DrawToHand();
        deckManager.interactable = true;
        endTurnButton?.SetActive(true);
    }

    public void EndTurn() {
        deckManager.interactable = false;
        endTurnButton?.SetActive(false);
        gameManager.Next();
    }

    public void EndCombat(bool won) {
        deckManager.interactable = false;
        endTurnButton?.SetActive(false);
        if (won) {
            Upgrade();
        } else {
            //TODO: Loose
        }
    }

    public void Upgrade() {
        //TODO: Upgrade
        gameManager.Next();
    }
}
