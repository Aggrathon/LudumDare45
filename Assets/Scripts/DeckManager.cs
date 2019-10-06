﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckManager : MonoBehaviour
{

    [SerializeField] protected TMPro.TextMeshProUGUI libraryText;
    [SerializeField] protected TMPro.TextMeshProUGUI discardText;
    [SerializeField] protected TMPro.TextMeshProUGUI energyText;

    [SerializeField] protected Transform cardPrefab;
    [SerializeField] protected Transform handTransform;
    [SerializeField] protected MessageWindow messageWindow;

    [System.NonSerialized] public Board board;
    public Vector3 spawnPositionLimit = new Vector3(10f, 1f, 1f);

    [SerializeField] protected List<BaseCard> defaultHand;
    private List<BaseCard> _deck;
    public List<BaseCard> deck { get => _deck; }
    private List<BaseCard> _discardPile;
    public List<BaseCard> discardPile { get => _discardPile; }
    private List<BaseCard> _drawPile;
    public List<BaseCard> drawPile { get => _drawPile; }
    [System.NonSerialized] public List<Unit> units;

    int _energy = 0;
    public int energy { get { return _energy; } set { _energy = value; if (energyText != null) energyText.text = _energy.ToString(); } }
    [System.NonSerialized] public bool interactable = false;

    private void Awake() {
        _deck = new List<BaseCard>(defaultHand);
        _discardPile = new List<BaseCard>();
        _drawPile = new List<BaseCard>();
        units = new List<Unit>();
    }

    public void PrepareBattle(Board board) {
        ClearBoard();
        _drawPile.AddRange(_deck);
        energy = 0;
        if (libraryText != null)
            libraryText.text = _drawPile.Count + " Cards";
        if (discardText != null)
            discardText.text = "0 Cards";
        this.board = board;
    }

    public void ClearBoard() {
        _discardPile.Clear();
        _drawPile.Clear();
        foreach (var u in units)
            Destroy(u);
        units.Clear();
    }

    public void Discard(BaseCard card) {
        _discardPile.Add(card);
        if (discardText != null)
            discardText.text = _discardPile.Count + " Cards";
    }

    public void Shuffle() {
        _drawPile.AddRange(_discardPile);
        _discardPile.Clear();
        Utils.ShuffleList(_drawPile);
        if (discardText != null)
            discardText.text = "0 Cards";
        if (libraryText != null)
            libraryText.text = _drawPile.Count + " Cards";
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
        if (_drawPile.Count == 0)
            Shuffle();
        if (_drawPile.Count == 0)
            return null;
        var tmp = _drawPile[_drawPile.Count - 1];
        _drawPile.RemoveAt(_drawPile.Count - 1);
        if (libraryText != null)
            libraryText.text = _drawPile.Count + " Cards";
        return tmp;
    }

    public void AddAtTop(BaseCard card) {
        _drawPile.Add(card);
        if (libraryText != null)
            libraryText.text = _drawPile.Count + " Cards";
    }

    public void AddAtBottom(BaseCard card) {
        _drawPile.Insert(0, card);
        if (libraryText != null)
            libraryText.text = _drawPile.Count + " Cards";
    }

    public void AddAtRandom(BaseCard card) {
        _drawPile.Insert(UnityEngine.Random.Range(0, _drawPile.Count+1), card);
        if (libraryText != null)
            libraryText.text = _drawPile.Count + " Cards";
    }

    public void AddToDeck(BaseCard card) {
        _deck.Add(card);
    }

    public bool RemoveFromDeck(BaseCard card) {
        return _deck.Remove(card);
    }

    public void Alert(string msg) {
        if (messageWindow != null)
            messageWindow.Alert(msg);
        #if UNITY_EDITOR
        else
            Debug.Log("Enemy: "+msg);
        #endif
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, spawnPositionLimit * 2);
    }

}
