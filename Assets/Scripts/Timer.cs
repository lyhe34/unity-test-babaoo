using UnityEngine;

namespace Game
{
	public class Timer : MonoBehaviour
	{
		public float RemainingTime { get; private set; }

		private void Awake()
		{
			GameManager.Instance.OnStartPlaying += StartTimer;
			GameManager.Instance.OnEndGame += StopTimer;
		}

		private void OnDestroy()
		{
			GameManager.Instance.OnStartPlaying -= StartTimer;
			GameManager.Instance.OnEndGame -= StopTimer;
		}

		private void StartTimer()
		{
			RemainingTime = GameManager.Instance.gameTime;
			enabled = true;
		}

		private void StopTimer()
		{
			enabled = false;
		}

		private void Update()
		{
			RemainingTime -= Time.deltaTime;

			UIManager.Instance.timerText.text = RemainingTime.ToString();

			if (RemainingTime <= 0)
			{
				RemainingTime = 0;

				UIManager.Instance.DisplayMessage("LOSE");

				GameManager.Instance.Result = GameResult.Lose;
				GameManager.Instance.ChangeState(GameState.Ended);
			}
		}
	}
}
