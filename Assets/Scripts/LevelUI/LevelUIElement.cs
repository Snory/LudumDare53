using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUIElement : MonoBehaviour
{
    public GameObject Item;

    public void SwitchElementVisibility()
    {
        if (Item.activeSelf)
        {
            Item.SetActive(false);
        } else
        {
            Item.SetActive(true);
        }

    }

}
