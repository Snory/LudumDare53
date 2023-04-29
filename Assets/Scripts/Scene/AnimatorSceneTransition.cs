using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;





public class AnimatorSceneTransition : SceneTransitionBase
{
    [SerializeField]
    private Animator _sceneTransitionAnimator;

    [SerializeField]
    private float _initTransitionDelay;

    [SerializeField]
    private float _initLoadDelay = 1.5f;

    public override void TransitToScene(string currentSceneName, string newSceneName)
    {
        StartCoroutine(RunAnimator(currentSceneName, newSceneName));
    }

    public IEnumerator RunAnimator(string currentSceneName, string newSceneName)
    {
        yield return new WaitForSeconds(_initTransitionDelay);
        _sceneTransitionAnimator.SetTrigger("End");
        yield return new WaitForSeconds(_initLoadDelay);
        LoadScene(newSceneName);
    }
}

