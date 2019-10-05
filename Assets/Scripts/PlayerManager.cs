using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DeckManager))]
public class PlayerManager : MonoBehaviour, IPlayerManager
{
    
    DeckManager deckManager;

    [SerializeField] protected GameObject endTurnButton;
    [SerializeField] protected TMPro.TextMeshProUGUI healthText;

    GameManager gameManager;

    GameObject headItem;
    GameObject chestItem;
    GameObject handItem;

    private void Awake() {
        endTurnButton?.SetActive(false);
        deckManager = GetComponent<DeckManager>();
        maxHealth = health;
        healthText.text = string.Format("{0}<color=black><size=20>\n{1}</size></color>", health, maxHealth);
    }

    [SerializeField] protected int energyGain = 1;
    [SerializeField] protected int cardGain = 2;
    [SerializeField] protected int health = 10;
    protected int maxHealth;

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

    public void Damage(int amount) {
        health -= amount;
        healthText.text = string.Format("{0}<color=black><size=20>\n{1}</size></color>", health, maxHealth);
        if (health <= 0)
            gameManager?.Loose(this);
    }

    public void Heal(int amount, bool over = false) {
        health += amount;
        if (health > maxHealth) {
            if (over)
                maxHealth += (health - maxHealth)/2;
            health = maxHealth;
        }
        healthText.text = string.Format("{0}<color=black><size=20>\n{1}</size></color>", health, maxHealth);
    }
}
