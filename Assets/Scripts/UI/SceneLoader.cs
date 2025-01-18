using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// A smooth alternative to switching the scene immediately; <br />
/// Plays a short fade-out animation while loading the next scene asynchronously, then fades the next scene in.
/// </summary>
public class SceneLoader : MonoBehaviour
{
    [SerializeField] private Animator sceneTransitionAnimator;

    public static SceneLoader Instance { get; private set; }
    private void Awake()
    {
        //Destroy the OTHER instance instead of this one so that each scene can have its own transition.
        if (Instance != null)
            Destroy(Instance.gameObject);
        Instance = this;
        Debug.Log("Switched Scene Loader to " + this);
    }

    private AsyncOperation async;
    public IEnumerator LoadScene(string sceneName)
    {
        float timeElapsed = 0;

        async = SceneManager.LoadSceneAsync(sceneName);
        async.allowSceneActivation = false;
        sceneTransitionAnimator?.SetTrigger("Close Scene");
        while (async.progress < 0.9f || timeElapsed < sceneTransitionAnimator.GetCurrentAnimatorClipInfo(0)[0].clip.length)
        {
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        async.allowSceneActivation = true;
        sceneTransitionAnimator?.SetTrigger("Open Scene");
        yield return null;
    }
}
