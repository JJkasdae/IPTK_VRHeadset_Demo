using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttentionUI : MonoBehaviour
{
    public GameObject attentionMessage; // Reference to the UI Text or Canvas element

    public void ShowMessage()
    {
        attentionMessage.SetActive(true);
    }

    public void HideMessage()
    {
        attentionMessage.SetActive(false);
    }
}
