using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class ImagePart : MonoBehaviour
    {
        [SerializeField]
        private Image image;

        private ImageManager imageManager;

        [HideInInspector]
        public Vector3 CorrectPosition { get; private set; }

        public AllowedMovement allowedMovement { get; private set; } = AllowedMovement.None;
        private Vector3 allowedPosition;

        private bool selected;

        private Vector3 initialPosition;

        public Image3D image3D;

        private void Awake()
        {
            GameManager.Instance.OnStartPlaying += Enable;
            GameManager.Instance.OnEndGame += Disable;
        }

        private void OnDestroy()
        {
            GameManager.Instance.OnStartPlaying -= Enable;
            GameManager.Instance.OnEndGame -= Disable;
        }

		private void Update()
		{
            if (selected)
            {
                // Move the image part.
                Vector3 position = transform.position;
                switch (allowedMovement)
                {
                    case AllowedMovement.Up:
                        position.y += InputManager.TouchDelta.y;
                        position.y = Mathf.Clamp(position.y, initialPosition.y, initialPosition.y + imageManager.spaceBetweenParts);
                        break;
                    case AllowedMovement.Right:
                        position.x += InputManager.TouchDelta.x;
                        position.x = Mathf.Clamp(position.x, initialPosition.x, initialPosition.x + imageManager.spaceBetweenParts);
                        break;
                    case AllowedMovement.Down:
                        position.y += InputManager.TouchDelta.y;
                        position.y = Mathf.Clamp(position.y,initialPosition.y - imageManager.spaceBetweenParts, initialPosition.y);
						break;
                    case AllowedMovement.Left:
                        position.x += InputManager.TouchDelta.x;
                        position.x = Mathf.Clamp(position.x, initialPosition.x - imageManager.spaceBetweenParts, initialPosition.x);
						break;
                }
                transform.position = position;
            }
		}

		public void Initialize(Sprite sprite, ImageManager imageManager)
        {
            this.imageManager = imageManager;

            image.sprite = sprite;

            CorrectPosition = transform.position.Round();
        }


        /// <summary>
        /// Check in all directions if the image part can be moved. 
        /// </summary>
        public void CheckAllowedMovement()
        {
            allowedPosition = transform.position;

            if (Mathf.Round(transform.position.x) != imageManager.MinX && !CheckAdjacentPart(Vector2.left))
            {
                allowedMovement = AllowedMovement.Left;
                allowedPosition.x -= imageManager.spaceBetweenParts;
            }
            else if (Mathf.Round(transform.position.x) != imageManager.MaxX && !CheckAdjacentPart(Vector2.right))
            {
                allowedMovement = AllowedMovement.Right;
                allowedPosition.x += imageManager.spaceBetweenParts;
            }
            else if (Mathf.Round(transform.position.y) != imageManager.MinY && !CheckAdjacentPart(Vector2.down))
            {
                allowedMovement = AllowedMovement.Down;
                allowedPosition.y -= imageManager.spaceBetweenParts;
            }
            else if (Mathf.Round(transform.position.y) != imageManager.MaxY && !CheckAdjacentPart(Vector2.up))
            {
                allowedMovement = AllowedMovement.Up;
                allowedPosition.y += imageManager.spaceBetweenParts;
            }
            else
                allowedMovement = AllowedMovement.None;
        }

        public void ShuffleMove()
        {
            transform.position = allowedPosition;
            image3D.Move(allowedMovement);
        }

        private bool CheckAdjacentPart(Vector2 direction)
        {
            RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, direction, 150);

            foreach (var hit in hits)
            {
                if(hit.collider != null && hit.transform.GetComponent<ImagePart>() != null && hit.collider != GetComponent<Collider2D>())
                    return true;
            }

            return false;
        }

        public void OnSelect()
        {
            if (imageManager.isMovingPart)
                return;

            CheckAllowedMovement();

            selected = true;

            initialPosition = transform.position;
        }

        public void OnDeselect()
        {
            imageManager.isMovingPart = false;
            selected = false;

            // Check which position is closer, the initial or the allowed one.
            float distanceFromInitial = Vector2.Distance(transform.position, initialPosition);
            float distanceFromAllowedMovement = Vector2.Distance(transform.position, allowedPosition);

            // Then we snap to it.
            if (distanceFromAllowedMovement < distanceFromInitial)
                transform.position = allowedPosition;
            else
                transform.position = initialPosition;

            image3D.Move(allowedMovement);

            imageManager.CheckCorrectPositions();
        }

        private void Disable()
        {
            enabled = false;
        }

        private void Enable()
        {
            enabled = true;
        }
    }
}

