using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class SceneSwitch : MonoBehaviour
{
    public string CurrentScene;

    [SerializeField]
    private EventSystem _eventSystemInScene;

    [SerializeField]
    private AudioListener _audioListenerInScene;

    [SerializeField]
    private SceneTransitionBase _sceneTransition;

    
    public void SwitchScene(string NextSceneName)
    {
        if (_eventSystemInScene != null)
        {
            _eventSystemInScene.enabled = false;
        }

        if (_audioListenerInScene != null)
        {
            _audioListenerInScene.enabled = false;
        }

        _sceneTransition.TransitToScene(CurrentScene, NextSceneName);
    }

    public void RelodeScene()
    {
        _sceneTransition.TransitToScene(CurrentScene, CurrentScene);
    }

}
