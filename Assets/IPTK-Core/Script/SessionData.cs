using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SessionData : ScriptableObject
{
    [SerializeField]
    private string _name;

    [SerializeField]
    [TextArea()]
    private string _description;

    [SerializeField]
    private string _sceneName;

    public string sceneName => _sceneName;
    public string Name => _name;
    public string Description => _description;

    public void initialize(string sceneName)
    {
        _sceneName = sceneName;
    }
}
