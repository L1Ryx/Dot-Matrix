using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class LoadingScreen : MonoBehaviour
{
    [Header("Settings")]
    public TMP_Text loadingText;
    public string sceneToLoadName;
    public float placeboTime = 1;

    private void Start()
    {
        StartCoroutine(LoadGameScene());
    }

    private IEnumerator LoadGameScene()
    {
        // Start loading the game scene asynchronously
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneToLoadName);
        asyncOperation.allowSceneActivation = false;

        // Wait until the scene has finished loading
        while (!asyncOperation.isDone)
        {
            // Output the current progress
            if (asyncOperation.progress >= 0.9f)
            {
                loadingText.text = "Loading...";
                // Allow scene activation after a delay
                yield return new WaitForSeconds(placeboTime);
                asyncOperation.allowSceneActivation = true;
            }
            else
            {
                loadingText.text = "Loading...";
            }
            yield return null;
        }
    }
}
