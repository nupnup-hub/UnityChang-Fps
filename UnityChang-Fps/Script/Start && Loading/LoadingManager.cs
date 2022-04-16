using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingManager : MonoBehaviour
{
    //public static string nextScene
    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GameObject.FindGameObjectWithTag("LoadingCharacter").GetComponent<Animator>();
        StartCoroutine(LoadScene());
    }

  
    IEnumerator LoadScene()
    {
        yield return null;

        AsyncOperation async = SceneManager.LoadSceneAsync("unity-chanFPS");
        async.allowSceneActivation = false;
        while (!async.isDone)
        {
            yield return new WaitForSeconds(1.5f);
            yield return new WaitForSeconds(1.5f);
            async.allowSceneActivation = true;
        }
    }
}
