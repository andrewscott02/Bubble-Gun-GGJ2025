using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOVer : MonoBehaviour
{
    [SerializeField]
    private GameObject playBubble;
    [SerializeField]
    private GameObject menuBubble;
    [SerializeField]
    private GameObject quitBubble;

    [SerializeField]
    private Button playButton;
    [SerializeField]
    private Button menuButton;
    [SerializeField]
    private Button quitButton;

    [SerializeField]
    private Object bubblePopFX;

    [SerializeField]
    private string levelName = "SampleScene";
    [SerializeField]
    private string menuName = "SplashScreen";

    private void Awake()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;

        playButton.onClick.AddListener(() => ButtonPressed(Play, playBubble, 0.75f));
        menuButton.onClick.AddListener(() => ButtonPressed(Menu, menuBubble, 0.75f));
        quitButton.onClick.AddListener(() => ButtonPressed(Quit, quitBubble, 0.75f));
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

    private void Menu()
    {
        SceneManager.LoadScene(menuName);
    }

    private void Quit()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}