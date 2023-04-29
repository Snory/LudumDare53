
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;




public abstract class SceneTransitionBase : MonoBehaviour
{

    [SerializeField]
    private GeneralEvent _sceneLoaded;
    private string _nextSceneName;

    public void UnloadScene(string currentSceneName)
    {
        if (SceneManager.sceneCount == 1)
        {
            return;
        }
        StartCoroutine(UnloadSceneRoutine(currentSceneName));
    }

    public void LoadScene(string newSceneName)
    {
        StartCoroutine(LoadSceneRoutine(newSceneName));
    }

    private IEnumerator LoadSceneRoutine(string newSceneName)
    {

        AsyncOperation asyncOp = SceneManager.LoadSceneAsync(newSceneName);
        asyncOp.allowSceneActivation = false;

        while (!asyncOp.isDone)
        {
            if (asyncOp.progress >= 0.9f)
            {
                asyncOp.allowSceneActivation = true;
            }

            yield return null;
        }
    }

    private IEnumerator UnloadSceneRoutine(string currentSceneName)
    {
        AsyncOperation asyncOp = SceneManager.UnloadSceneAsync(currentSceneName);
        //run hide anim
        while (!asyncOp.isDone)
        {
            yield return null;
        }
    }

    public virtual void TransitToScene(string currentSceneName, string newSceneName)
    {
        UnloadScene(currentSceneName);
        LoadScene(newSceneName);
    }

    public void SceneLoaded()
    {
        _sceneLoaded.Raise();
        
    }
}
