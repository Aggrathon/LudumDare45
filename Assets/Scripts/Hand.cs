using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hand : MonoBehaviour
{
    [SerializeField] public BaseCard testCard;

    public Transform cardPrefab;

    [ContextMenu("Test Spawn")]
    private void TestSpawn() {
        SpawnCard(testCard);
    }

    private void SpawnCard(BaseCard card) {
        var tr = Instantiate<Transform>(cardPrefab, transform);
        tr.GetComponent<CardUI>().Setup(card);
    }
}
