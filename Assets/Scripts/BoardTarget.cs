using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BoardTarget : MonoBehaviour
{
    //TODO: replace with ynit class
    [NonSerialized] public GameObject unit;

    override public string ToString() {
        return "{BoardTarget "+Mathf.RoundToInt(transform.localPosition.x)+" "+ Mathf.RoundToInt(transform.localPosition.z)+" "+unit+"}";
    }
}
