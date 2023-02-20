using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarClicked : MonoBehaviour
{
    public GameObject sendTo;
    void OnMouseDown()
    {
        // Destroy the gameObject after clicking on it
        sendTo.GetComponent<Scope>().StarClicked(this.gameObject);
    }
}
