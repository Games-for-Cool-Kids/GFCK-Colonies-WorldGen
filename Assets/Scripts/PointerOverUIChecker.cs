using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Simple script that will check every update if the user pointer is over UI or not.
/// </summary>
public class PointerOverUIChecker : MonoBehaviour
{
    private int UILayer;

    private bool pointerOverUIElement = false;
    public bool PointerOverUI => pointerOverUIElement;

    void OnEnable()
    {
        UILayer = LayerMask.NameToLayer("UI");
    }

    void Update()
    {
        pointerOverUIElement = IsPointerOverUIElement(GetEventSystemRaycastResults());
    }


    //Returns 'true' if we touched or hovering on Unity UI element.
    private bool IsPointerOverUIElement(List<RaycastResult> eventSystemRaysastResults)
    {
        for (int index = 0; index < eventSystemRaysastResults.Count; index++)
        {
            RaycastResult curRaysastResult = eventSystemRaysastResults[index];
            if (curRaysastResult.gameObject.layer == UILayer)
                return true;
        }
        return false;
    }


    //Gets all event system raycast results of current mouse or touch position.
    private List<RaycastResult> GetEventSystemRaycastResults()
    {
        PointerEventData eventData = new(EventSystem.current)
        {
            position = Input.mousePosition
        };
        List<RaycastResult> raycastResults = new();

        EventSystem.current.RaycastAll(eventData, raycastResults);
        return raycastResults;
    }
}
