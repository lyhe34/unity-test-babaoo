using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class Images3DManager : MonoBehaviour
    {
        [SerializeField]
        private Image3D image3DPrefab;

        [SerializeField]
        private Transform images3DParent;

        public Image3D emptyImage3D { get; private set; }

        public void Initialize(ImagePart[] imageParts, Sprite[] sprites)
        {
            Vector3 position = new Vector3(-1, 0, 1);

            // Instantiate the 3D images.
            for (int i = 0; i < 9; i++)
            {
                Image3D image3D = Instantiate(image3DPrefab, position, image3DPrefab.transform.rotation, images3DParent);
                imageParts[i].image3D = image3D;
                image3D.Initialize(this, sprites[i]);

                if (i == 4)
                    emptyImage3D = image3D;

                position.x += 1;

                if (position.x == 2)
                {
                    position.x = -1;
                    position.z += -1;
                }
            }
        }
    }
}

