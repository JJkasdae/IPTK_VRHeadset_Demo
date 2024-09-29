using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayAttentionUI : MonoBehaviour
{
    private GameObject attentionUIPrefab;
    private Canvas displayCanvas;
    private bool isActive = false;


    // Start is called before the first frame update
    void Start()
    {
        attentionUIPrefab = this.gameObject;
        if (attentionUIPrefab == null)
        {
            Debug.LogError("attentionUIPrefab is null!");
        }

        displayCanvas = attentionUIPrefab.GetComponentInChildren<Canvas>();

        if (displayCanvas != null)
        {
            displayCanvas.gameObject.SetActive(isActive);
        }
        else
        {
            Debug.LogError("displayCanvas of the reminder prefab is null!");
        }
    }

    public void ToggleUI()
    {
        isActive = !isActive;
        displayCanvas.gameObject.SetActive(isActive);
    }
}
