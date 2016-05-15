using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;
using System;

public abstract class Item : Draggable, IPointerEnterHandler, IPointerExitHandler
{
    public string itemName;
    public string description;
    public int hpMod;
    public int calmMod;

    public override void OnEndDrag(PointerEventData eventData)
    {
        base.OnEndDrag(eventData);
        HideItemDesc();
    }

    //Pointer Enter and Exit function for items
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
            return;
        ShowItemDesc();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
            return;
        HideItemDesc();
    }

    void ShowItemDesc()
    {
        GameObject infoZone = GameObject.Find("InfoZone");
        //Set title
        infoZone.transform.GetChild(0).GetComponent<Text>().text = itemName;

        //Set description
        infoZone.transform.GetChild(1).GetComponent<Text>().text = description;
    }

    void HideItemDesc()
    {
        GameObject infoZone = GameObject.Find("InfoZone");
        //Set title
        infoZone.transform.GetChild(0).GetComponent<Text>().text = "";

        //Set description
        infoZone.transform.GetChild(1).GetComponent<Text>().text = "";
    }

    public virtual void UseItem()
    {
        //Item was used, placeholder not required anymore
        Destroy(placeholder);
        HideItemDesc();

    }
}

