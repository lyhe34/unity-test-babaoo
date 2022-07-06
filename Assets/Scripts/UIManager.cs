using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [SerializeField]
    private Text messageText;

    public Text timerText;

    public Image resultImage;

    private void Awake() => Instance = this;

    // Display a permanent message (until changed).
    public void DisplayMessage(string message)
    {
        messageText.text = message;
    }

    // Display a message that disappear after the specified duration.
	public void DisplayMessage(string message, float duration)
    {
        StartCoroutine(DisplayMessageCoroutine(message, duration));
    }

    private IEnumerator DisplayMessageCoroutine(string message, float duration)
    {
        messageText.text = message;

        yield return new WaitForSeconds(duration);

        messageText.text = "";
    }
}
