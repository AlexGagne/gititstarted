using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class DropZone : MonoBehaviour, IDropHandler {

    public void OnDrop(PointerEventData eventData)
    {
        Item d = eventData.pointerDrag.GetComponent<Item>();
        if (d != null)
        {
            Debug.Log("Worked!");
            //Get the patient gamescript to use the item
            Patient patient = gameObject.transform.parent.parent.gameObject.GetComponent<Patient>();

            //He may use the item if he is seated only
            if(patient.IsSeated)
            {
                Debug.Log("WasSeated!");
                d.UseItem(patient);
            }
                
        }
    }
}
