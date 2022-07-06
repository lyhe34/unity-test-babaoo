using UnityEngine;

namespace Game
{
	public class ScoreManager : MonoBehaviour
	{
		[SerializeField]
		private Timer timer;

		private void Awake() => GameManager.Instance.OnEndGame += CheckBestScore;

		private void OnDestroy() => GameManager.Instance.OnEndGame -= CheckBestScore;

		private void CheckBestScore()
		{
			if (PlayerPrefs.GetFloat("Best Score") < timer.RemainingTime)
			{
				PlayerPrefs.SetFloat("Best Score", timer.RemainingTime);
			}
		}
	}
}

