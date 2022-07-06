using UnityEngine;

namespace Game
{
	public class InputManager : MonoBehaviour
	{
		public static Vector3 TouchDelta { get; private set; }

		private Vector3 lastTouchPosition;

		private void Update()
		{
			if (Input.GetMouseButtonDown(0))
			{
				lastTouchPosition = Input.mousePosition;
			}

			if (Input.GetMouseButton(0))
			{
				TouchDelta = Input.mousePosition - lastTouchPosition;
				lastTouchPosition = Input.mousePosition;
			}
		}
	}
}

