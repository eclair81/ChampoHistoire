using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemButton : MonoBehaviour
{
    public GameObject card;
    public GameObject block;
    private Boolean isActive;

    void Start()
    {
        isActive = false;
    }
    public void OpenItem()
    {
        if (!isActive) { 
            block.SetActive(true);
            card.SetActive(true);
            isActive = true;
        }
    }

    public void CloseItem()
    {
        block.SetActive(false);
        card.SetActive(false);
        isActive = false;
    }
}
