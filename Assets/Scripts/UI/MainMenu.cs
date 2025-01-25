using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject playBubble;
    [SerializeField]
    private GameObject infoBubble;
    [SerializeField]
    private GameObject quitBubble;
    [SerializeField]
    private GameObject closeBubble;

    [SerializeField]
    private Button playButton;
    [SerializeField]
    private Button infoButton;
    [SerializeField]
    private Button quitButton;
    [SerializeField]
    private Button closeButton;

    [SerializeField]
    private Object bubblePopFX;

    [SerializeField]
    private string levelName = "SampleScene";

    private void Awake()
    {
        playButton.onClick.AddListener(() => ButtonPressed(Play, playBubble, 0.75f));
        infoButton.onClick.AddListener(() => ButtonPressed(Info, infoBubble, 0));
        quitButton.onClick.AddListener(() => ButtonPressed(Quit, quitBubble, 0.75f));
        closeButton.onClick.AddListener(() => ButtonPressed(Close, closeBubble, 0));
    }

    private void ButtonPressed(System.Action func, GameObject bubble, float delay)
    {
        Instantiate(bubblePopFX, bubble.transform.position, Quaternion.identity);
        bubble.SetActive(false);
        StartCoroutine(IDelayButton(delay, func));
    }

    private IEnumerator IDelayButton(float delay, System.Action func)
    {
        yield return new WaitForSeconds(delay);

        func();
    }

    private void Play()
    {
        SceneManager.LoadScene(levelName);
    }

    private void Info()
    {

    }

    private void Quit()
    {
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #else
                Application.Quit();
        #endif
    }

    private void Close()
    {

    }
}