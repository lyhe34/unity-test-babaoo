using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace Game
{
    /// <summary>
    /// Used to load resources, create the cropped image, shuffle it and after a part is moved by the player, check if all parts are in correct order..
    /// </summary>
    public class ImageManager : MonoBehaviour
    {
        [SerializeField]
        [Range(0, 10)]
        private float timeBeforeShuffling;

        [SerializeField]
        [Range(0, 10)]
        private float shuffleDuration;

        [SerializeField]
        private ImagePart imagePart;

        [SerializeField]
        private Transform imagePartsParent;

        [SerializeField]
        private GridLayoutGroup gridLayoutGroup;

        [SerializeField]
        private Images3DManager images3DManager;

        private Sprite[] imagePartSprites = new Sprite[9];
        private Sprite resultImageSprite;

        private ImagePart[] imageParts = new ImagePart[9];

        [HideInInspector]
        public float spaceBetweenParts;

        public float MinX { get; private set; }
        public float MaxX { get; private set; }
        public float MinY { get; private set; }
        public float MaxY { get; private set; }

        [HideInInspector]
        public bool isMovingPart = false;

		private void Start()
		{
            GetResources();

            UIManager.Instance.resultImage.sprite = resultImageSprite;

            StartCoroutine(CreateCroppedImage());
		}

		private void GetResources()
        {
            // Get the current platform to load appropriate images.
            string platform = string.Empty;
            switch (Application.platform)
            {
                case RuntimePlatform.Android:
                    platform = "Android";
                    break;
                case RuntimePlatform.IPhonePlayer:
                    platform = "Apple";
                    break;
            }

            // Load the cropped images.
            for (int i = 1; i < 10; i++)
            {
                imagePartSprites[i - 1] = Resources.Load<Sprite>(platform + "/" + i.ToString());
            }

            // Load the result image.
            resultImageSprite = Resources.Load<Sprite>(platform + "/Result");
        }

        private void SetConstraints()
        {
            MinX = Mathf.Round(imageParts[0].transform.position.x);
            MaxX = Mathf.Round(imageParts[2].transform.position.x);
            MinY = Mathf.Round(imageParts[6].transform.position.y);
            MaxY = Mathf.Round(imageParts[0].transform.position.y);

            spaceBetweenParts = imageParts[1].transform.position.x - imageParts[0].transform.position.x;
        }

        private IEnumerator CreateCroppedImage()
        {
            for(int i = 0; i < 9; i++)
            {
                ImagePart imagePart = Instantiate(this.imagePart, imagePartsParent);
                imageParts[i] = imagePart;
            }

            // Wait for next two frames, because disabling grid layout group in the same frame cause trouble.
            yield return 0;

            // Disable grid layout group so we can move image parts freely.
            gridLayoutGroup.enabled = false;

            yield return 0;

            for (int i = 0; i < 9; i++)
            {
                imageParts[i].Initialize(imagePartSprites[i], this);
            }

            images3DManager.Initialize(imageParts, imagePartSprites);

            // Remove the center part.
            Destroy(imageParts[4].gameObject);

            SetConstraints();

            StartCoroutine(Shuffle());
        }

        private IEnumerator Shuffle()
        {
            yield return new WaitForSeconds(timeBeforeShuffling);

            GameManager.Instance.ChangeState(GameState.Shuffling);

            float endShuffleTime = Time.time + shuffleDuration;

            while (Time.time < endShuffleTime)
            {
                // Get all moveable parts.
                List<ImagePart> moveableParts = new List<ImagePart>();
                foreach (var imagePart in imageParts)
                {
                    if (imagePart == null) continue;

                    imagePart.CheckAllowedMovement();
                    if (imagePart.allowedMovement != AllowedMovement.None)
                    {
                        moveableParts.Add(imagePart);
                    }
                }

                // Pick random moveable part and move it.
                int random = Random.Range(0, moveableParts.Count);
                moveableParts[random].ShuffleMove();

                yield return new WaitForFixedUpdate();
            }

            // Start the game.
            UIManager.Instance.DisplayMessage("START", 2);
            GameManager.Instance.ChangeState(GameState.Playing);
        }

        public void CheckCorrectPositions()
        {
            for (int i = 0; i < 9; i++)
            {
                // Ignore the center image previously destroyed.
                if (i == 4)
                    continue;

                if (imageParts[i].transform.position.Round() != imageParts[i].CorrectPosition)
                    return;
            }

            UIManager.Instance.DisplayMessage("WIN");
            GameManager.Instance.Result = GameResult.Win;
            GameManager.Instance.ChangeState(GameState.Ended);
        }
    }
}

