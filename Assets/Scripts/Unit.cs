﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;

public class Unit : MonoBehaviour
{
    [NonSerialized] public DeckManager team;
    public string unitName;

    [SerializeField] int health = 2;
    int maxHealth;
    [SerializeField] TMPro.TextMeshPro healthText;

    [NonSerialized] public bool hasMoved;
    public float moveDist = 1f;
    public float attackDist = 1f;
    public int damage = 1;

    private BoardTarget location;

    public void Setup(DeckManager team, BoardTarget target) {
        this.team = team;
        maxHealth = health;
        healthText.text = health + " / " + maxHealth;
        if (target.unit != null)
            Debug.LogWarning("Boardlocation is not empty ("+unitName+", "+target+")");
        location = target;
        location.unit = this;
        team.units.Add(this);
    }

    public bool Damage(int amount) {
        health -= amount;
        if (health <= 0) {
            healthText.text = "0 / " + maxHealth;
            if (location.unit == this)
                location.unit = null;
            Destroy(gameObject, 0.5f);
            team.units.Remove(this);
            return true;
        } else {
            healthText.text = health + " / " + maxHealth;
            return false;
        }
    }

    public void Heal(int amount) {
        health = Mathf.Min(health + amount, maxHealth);
        healthText.text = health + " / " + maxHealth;
    }

    public bool Move(BoardTarget target) {
        if (target == null) {
            return false;
        }
        var sqrDist = (target.transform.localPosition - location.transform.localPosition).sqrMagnitude;
        if (target.unit != null) {
            if (target.unit.team == team) {
                team.Alert("Space already occupied!");
                return false;
            } else if (sqrDist < attackDist * attackDist) {
                hasMoved = true;
                //TODO: Attack FX
                if (target.unit.Damage(damage) && sqrDist < moveDist * moveDist) {
                    StartCoroutine(Utils.LerpMoveTo(transform, target.transform.position));
                    target.unit = this;
                    location = target;
                    return true;
                } else {
                    return false;
                }
            } else {
                team.Alert("Target is too far away!");
                return false;
            }
        } else if (sqrDist < moveDist * moveDist) {
            hasMoved = true;
            StartCoroutine(Utils.LerpMoveTo(transform, target.transform.position));
            target.unit = this;
            if (location.unit == this)
                location.unit = null;
            location = target;
            return true;
        } else {
            team.Alert("Cannot move that far!");
            Debug.Log("dist: "+sqrDist);
        }
        return false;
    }

    private void OnDestroy() {
        if (location.unit == this)
            location.unit = null;
        team.units.Remove(this);
    }
}