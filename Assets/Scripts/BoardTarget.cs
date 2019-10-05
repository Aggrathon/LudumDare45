using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;

public class BoardTarget : MonoBehaviour
{
    [NonSerialized] public Unit unit;
    MeshRenderer highlight;

    Board board;

    private void Start() {
        board = transform.parent.GetComponent<Board>();
        board.targets.Add(this);
        highlight = GetComponent<MeshRenderer>();
        highlight.enabled = false;
    }
    

    public void Highlight() {
        highlight.enabled = true;
    }

    public void Unlight() {
        highlight.enabled = false;
    }

    override public string ToString()
    {
        return "{BoardTarget " + Mathf.RoundToInt(transform.localPosition.x) + " " + Mathf.RoundToInt(transform.localPosition.z) + " " + unit + "}";
    }
}
