using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class DropZone : MonoBehaviour, IDropHandler {

    public void OnDrop(PointerEventData eventData)
    {
        Item d = eventData.pointerDrag.GetComponent<Item>();
        if (d != null)
        {
            d.UseItem();
        }
    }
}
