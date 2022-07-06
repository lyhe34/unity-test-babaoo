using UnityEngine;
using UnityEngine.UI;

using SceneTransitionSystem;


namespace TeasingGame
{
    public enum TeasingGameScene :int 
    {
        Home,
        Game,
    }
public class TeasingGameHomeSceneController : MonoBehaviour
{
    [SerializeField]
    private Text bestScoreText;

    public TeasingGameScene SceneForButton;

    private void Start()
    {
            if (PlayerPrefs.HasKey("Best Score"))
                bestScoreText.text = "BEST SCORE : " + PlayerPrefs.GetFloat("Best Score").ToString();
            else
                bestScoreText.text = "";
    }

   public void GoToGameScene()
    {
        STSSceneManager.LoadScene(SceneForButton.ToString());
    }
}
}