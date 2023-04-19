using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIToggleGroup : MonoBehaviour
{
    List<UIToggle> uIToggles = new List<UIToggle>();
    public void AddUIToggle(UIToggle uIToggle)
    {
        if (!uIToggles.Contains(uIToggle))
        {
            uIToggles.Add(uIToggle);
        }
    }
    public void ToggleClickEvent(UIToggle uIToggle)
    {
        foreach (var item in uIToggles)
        {
            if (item == uIToggle)
            {
                item.SetSelectedByTg(true);
            }
            else
            {
                item.SetSelectedByTg(false);
            }
        }
    }
}
