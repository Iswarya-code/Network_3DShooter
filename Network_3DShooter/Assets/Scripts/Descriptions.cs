using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Descriptions : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
    public GameObject dropDown;
    public void OnPointerEnter(PointerEventData eventData)
    {
        dropDown.SetActive(true);

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        dropDown.SetActive(false);

    }

    // Start is called before the first frame update
    void Start()
    {
        dropDown.SetActive(false);
    }

    
}
