using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CardUI : MonoBehaviour
{

    public BaseCard card;
    public Image sprite;
    public TMPro.TextMeshProUGUI description;
    public TMPro.TextMeshProUGUI energy;

    public EventTrigger eventTrigger;
    public LineRenderer line;
    bool dragging = false;
    Camera cam;

    public void Setup(BaseCard card) {
        this.card = card;
        sprite.sprite = card.sprite;
        description.text = card.GetDescription();
        energy.text = card.cost.ToString();
        cam = Camera.main;

        var start = new EventTrigger.Entry();
        start.eventID = EventTriggerType.BeginDrag;
        start.callback.AddListener((d) => OnBeginDrag((PointerEventData)d));
        eventTrigger.triggers.Add(start);
        var drag = new EventTrigger.Entry();
        drag.eventID = EventTriggerType.Drag;
        drag.callback.AddListener((d) => OnDrag((PointerEventData)d));
        eventTrigger.triggers.Add(drag);
        var stop = new EventTrigger.Entry();
        stop.eventID = EventTriggerType.PointerUp;
        stop.callback.AddListener((d) => OnPointerUp((PointerEventData)d));
        eventTrigger.triggers.Add(stop);
    }
    

    private void OnBeginDrag(PointerEventData eventData) {
        dragging = true;
        line.enabled = true;
        var pos = GetGroundIntersect(eventData.position, 3f);
        line.SetPosition(0, pos);
        line.SetPosition(1, pos);
    }

    private void OnPointerUp(PointerEventData eventData) {
        if (dragging && !EventSystem.current.IsPointerOverGameObject(eventData.pointerId)) {
            BoardTarget target = null;
            RaycastHit hit;
            if (Physics.Raycast(cam.ScreenPointToRay(eventData.position), out hit, 100f)) {
                target = hit.transform.GetComponent<BoardTarget>();
            }
            if (card.Cast(target))
                Destroy(gameObject);
        }
        dragging = false;
        line.enabled = false;
    }

    private void OnDrag(PointerEventData eventData) {
        line.SetPosition(1, GetGroundIntersect(eventData.position, 0.1f));
    }

    private Vector3 GetGroundIntersect(Vector3 screenPos, float height) {
        var ray = cam.ScreenPointToRay(screenPos);
        if (height > ray.origin.y)
            return ray.origin;
        height =  - (ray.origin.y - height) / ray.direction.y;
        return ray.origin + ray.direction * height;
    }
}
