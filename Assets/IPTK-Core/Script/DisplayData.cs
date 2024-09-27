using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DisplayData : MonoBehaviour
{
    [SerializeField]
    private SessionData _sessionData;

    private TextMeshProUGUI _nameText;

    private TextMeshProUGUI _descriptionText;

    // Start is called before the first frame update
    void Start()
    {
        if (_sessionData != null)
        {
            //Debug.Log(_sessionData.Name + _sessionData.Description);
            _nameText.text = _sessionData.Name;
            _descriptionText.text = _sessionData.Description;
        }
        else
        {
            Debug.Log("SessionData is not assigned");
        }

    }

    // Update is called once per frame
    void Update()
    {

    }
}
