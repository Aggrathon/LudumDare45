﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckManager : MonoBehaviour
{

    [SerializeField] protected TMPro.TextMeshProUGUI libraryText;
    [SerializeField] protected TMPro.TextMeshProUGUI discardText;

    [SerializeField] protected Transform cardPrefab;
    [SerializeField] protected Transform handTransform;

    [SerializeField] protected List<BaseCard> defaultHand;
    private List<BaseCard> deck;
    private List<BaseCard> discardPile;
    private List<BaseCard> drawPile;

    [System.NonSerialized] public int energy = 0;
    [System.NonSerialized] public bool interactable = false;

    private void Awake() {
        deck = new List<BaseCard>(defaultHand);
        discardPile = new List<BaseCard>();
        drawPile = new List<BaseCard>();
    }

    public void PrepareBattle() {
        discardPile.Clear();
        drawPile.Clear();
        drawPile.AddRange(deck);
        energy = 0;
        if (libraryText != null)
            libraryText.text = drawPile.Count + " Cards";
        if (discardText != null)
            discardText.text = "0 Cards";
    }

    public void Discard(BaseCard card) {
        discardPile.Add(card);
        if (discardText != null)
            discardText.text = discardPile.Count + " Cards";
    }

    public void Shuffle() {
        drawPile.AddRange(discardPile);
        discardPile.Clear();
        Utils.ShuffleList(drawPile);
        if (discardText != null)
            discardText.text = "0 Cards";
        if (libraryText != null)
            libraryText.text = drawPile.Count + " Cards";
    }

    [ContextMenu("DrawToHand")]
    public void DrawToHand() {
        var tmp = Draw();
        if (tmp != null) {
            var tr = Instantiate<Transform>(cardPrefab, handTransform);
            tr.GetComponent<CardUI>().Setup(tmp, this);
        }
    }

    public BaseCard Draw() {
        if (drawPile.Count == 0)
            Shuffle();
        if (drawPile.Count == 0)
            return null;
        var tmp = drawPile[drawPile.Count - 1];
        drawPile.RemoveAt(drawPile.Count - 1);
        if (libraryText != null)
            libraryText.text = drawPile.Count + " Cards";
        return tmp;
    }

    public void AddAtTop(BaseCard card) {
        drawPile.Add(card);
        if (libraryText != null)
            libraryText.text = drawPile.Count + " Cards";
    }

    public void AddAtBottom(BaseCard card) {
        drawPile.Insert(0, card);
        if (libraryText != null)
            libraryText.text = drawPile.Count + " Cards";
    }

    public void AddAtRandom(BaseCard card) {
        drawPile.Insert(UnityEngine.Random.Range(0, drawPile.Count+1), card);
        if (libraryText != null)
            libraryText.text = drawPile.Count + " Cards";
    }

    public void AddToDeck(BaseCard card) {
        deck.Add(card);
        Utils.ShuffleList(deck);
    }

    public bool RemoveFromDeck(BaseCard card) {
        return deck.Remove(card);
    }

}