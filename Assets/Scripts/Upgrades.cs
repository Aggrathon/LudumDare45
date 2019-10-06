using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Upgrades : MonoBehaviour
{
    public Transform addNewCard;
    public Transform replaceOldCard;
    public Transform replaceNewCard;
    public Transform equipmentOldCard;
    public Transform equipmentNewCard;

    public PlayerManager player;

    public List<BaseCard> baseCards;

    int numLeft;
    BaseCard _addNewCard;
    BaseCard _replaceOldCard;
    BaseCard _replaceNewCard;
    BaseCard _equipmentOldCard;
    BaseCard _equipmentNewCard;

    public void Show() {
        numLeft = 2;
        SetupUpgrades();
        gameObject.SetActive(true);
    }

    private void SetupUpgrades() {
        //TODO: A number somewhere telling how many upgrades you can select
        _addNewCard = baseCards[UnityEngine.Random.Range(0, baseCards.Count)];
        PopulateCard(_addNewCard, addNewCard);
        _replaceOldCard = player.deck.deck[UnityEngine.Random.Range(0, player.deck.deck.Count)];
        PopulateCard(_replaceOldCard, replaceOldCard);
        if (_replaceOldCard.upgrades.Length == 0) {
            _replaceNewCard = baseCards[UnityEngine.Random.Range(0, baseCards.Count)];
        } else {
            _replaceNewCard = _replaceOldCard.upgrades[UnityEngine.Random.Range(0, _replaceOldCard.upgrades.Length)];
        }
        PopulateCard(_replaceNewCard, replaceNewCard);
        //TODO: Implement Equipment
    }

    public void UpgradeHealth() {
        if (!gameObject.activeSelf)
            return;
        player.Heal(4, true);
        FinishUpgrade();
    }

    public void UpgradeNew() {
        if (!gameObject.activeSelf)
            return;
        player.deck.AddToDeck(_addNewCard);
        FinishUpgrade();
    }

    public void UpgradeReplace() {
        if (!gameObject.activeSelf)
            return;
        player.deck.RemoveFromDeck(_replaceOldCard);
        player.deck.AddToDeck(_replaceNewCard);
        FinishUpgrade();
    }

    public void UpgradeEquipment() {
        //TODO: Implement Equipment
        FinishUpgrade();
    }

    private void FinishUpgrade() {
        numLeft--;
        if (numLeft > 0) {
            SetupUpgrades();
        } else {
            gameObject.SetActive(false);
            player.Upgraded();
        }
    }

    private void PopulateCard(BaseCard card, Transform parent) {
        var col = card.color;
        col.a = 1f;
        parent.GetChild(0).GetComponent<Image>().sprite = card.sprite;
        parent.GetChild(1).GetComponent<Image>().color = col;
        parent.GetChild(2).GetComponent<TMPro.TextMeshProUGUI>().text = card.cardName;
        parent.GetChild(2).GetComponent<TMPro.TextMeshProUGUI>().color = col;
        parent.GetChild(3).GetComponent<TMPro.TextMeshProUGUI>().text = card.GetDescription();
        parent.GetChild(4).GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = card.cost.ToString();
    }
}
