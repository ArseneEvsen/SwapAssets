using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enable : MonoBehaviour
{
    public GameObject UI;

    public void activeUI()
    {
        UI.SetActive(true);
    }
}
