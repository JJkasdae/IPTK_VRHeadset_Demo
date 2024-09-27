using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class mainFunction : MonoBehaviour
{

    // Start is called before the first frame update
    [SerializeField]
    private PresentationData _presentationData;

    //[SerializeField]
    //private GameObject _loadingScreen; // Here is the transition effect which should be set by the presenter

    //[SerializeField]
    //private Slider _progressBar;

    private int _currentSessionIndex = 0;
    private string _currentSceneName;
    

    void Start()
    {
        // Add a check method to avoid the same name of scenes.


        _currentSceneName = SceneManager.GetActiveScene().name; // Get the name of the current scene.

        Debug.Log(_presentationData.Timeline.transitionData.Length);

        for (int i = 0; i < _presentationData.Timeline.transitionData.Length; i++) // Find the index of the current session through the current scene.
        {
            if (_presentationData.Timeline.transitionData[i].lastSession.sceneName == _currentSceneName)
            {
                _currentSessionIndex = i;
                break;
            }
        }

        Debug.Log(_presentationData.Timeline.transitionData[_currentSessionIndex].lastSession.sceneName);
    }

    // Update is called once per frame
    void Update()
    {
        SwitchScene();
    }

    void SwitchScene()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow) && _currentSessionIndex < _presentationData.Timeline.transitionData.Length - 1)
        {
            SceneManager.LoadScene(_presentationData.Timeline.transitionData[_currentSessionIndex].nextSession.sceneName);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow) && _currentSessionIndex > 0)
        {
            SceneManager.LoadScene(_presentationData.Timeline.transitionData[_currentSessionIndex - 1].lastSession.sceneName);
        }
    }

    /*IEnumerable loadSceneAsync(string sceneName)
    {
        AsyncOperation _asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        _asyncLoad.allowSceneActivation = false;

        _loadingScreen.SetActive(true);

        while (!_asyncLoad.isDone)
        {
            float _progress = Mathf.Clamp01(_asyncLoad.progress / 0.9f);
            _progressBar.value = _progress;

            if (_asyncLoad.progress > 0.9f)
            {
                _asyncLoad.allowSceneActivation = true;
            }
            yield return null;
        }
    }*/
}
