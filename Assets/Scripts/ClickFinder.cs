using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickFinder : MonoBehaviour, IPointerClickHandler 
{
    public event Action<int, int, int> OnMapVisualClickAction;
    public void OnPointerClick(PointerEventData eventData)
    {
        GameObject getCube = eventData.rawPointerPress;
        int mousebutton = eventData.pointerId;

        if (getCube != null)
        {
            CubeStateInfo cubInfo = getCube.GetComponent<CubeStateInfo>();
            if (cubInfo!=null) {
                this.OnMapVisualClickAction?.Invoke(cubInfo.x, cubInfo.y, mousebutton);
            }
        }

    }

}
