using UnityEngine;
using System;
using SceneTransitionSystem;

namespace Game
{
    public class GameManager : MonoBehaviour
    {
		public static GameManager Instance { get; private set; }

		public float gameTime;

		public Action OnStartPlaying;
		public Action OnEndGame;

		[HideInInspector]
		public GameState State;

		[HideInInspector]
		public GameResult Result;

		private void Awake() => Instance = this;

		public void ChangeState(GameState newState)
		{
			State = newState;

			switch (State)
			{
				case GameState.Playing:
					OnStartPlaying?.Invoke();
					break;
				case GameState.Ended:
					OnEndGame?.Invoke();
					break;
			}
		}

		public void Quit()
		{
			STSSceneManager.LoadScene("Home");
		}
	}
}

