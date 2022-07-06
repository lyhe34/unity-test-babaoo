using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
	public class Image3D : MonoBehaviour
	{
		private Images3DManager images3DManager;
		[SerializeField]
		private SpriteRenderer spriteRenderer;

		public void Move(AllowedMovement allowedMovement)
		{
			Vector3 lastPosition = transform.position;

			switch (allowedMovement)
			{
				case AllowedMovement.Up:
					transform.position += Vector3.forward;
					break;
				case AllowedMovement.Down:
					transform.position += Vector3.back;
					break;
				case AllowedMovement.Right:
					transform.position += Vector3.right;
					break;
				case AllowedMovement.Left:
					transform.position += Vector3.left;
					break;
			}

			if (transform.position == images3DManager.emptyImage3D.transform.position)
			{
				images3DManager.emptyImage3D.transform.position = lastPosition;
			}
		}

		public void Initialize(Images3DManager images3DManager, Sprite sprite)
		{
			this.images3DManager = images3DManager;
			spriteRenderer.sprite = sprite;
		}
	}
}

