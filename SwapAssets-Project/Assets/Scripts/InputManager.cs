using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour
{
    [SerializeField]
    private Camera sceneCamera;

    private Vector3 lastPosition;

    [SerializeField]
    private LayerMask placementLayermask;


    public Vector3? GetSelectedMapPosition()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = sceneCamera.nearClipPlane;
        Ray ray = sceneCamera.ScreenPointToRay(mousePos);
        RaycastHit hit;

        int layerMask = LayerMask.GetMask("UI");

        // Effectuer le raycast en utilisant le masque de couche
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
        {
            // Si le raycast touche un composant avec le layer "UI", ne rien faire et quitter la méthode Update()
            return null;
        }

        if (Physics.Raycast(ray, out hit, 100, placementLayermask))
        {

            lastPosition = hit.point;
        }
        return lastPosition;
    }
}