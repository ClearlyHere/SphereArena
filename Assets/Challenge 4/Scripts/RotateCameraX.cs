using UnityEngine;

namespace Challenge_4.Scripts
{
    public class RotateCameraX : MonoBehaviour
    {
        private const float Speed = 200;
        public GameObject player;

        // Update is called once per frame
        void Update()
        {
            float horizontalInput = Input.GetAxis("Horizontal");
            Transform transform1;
            (transform1 = transform).Rotate(Vector3.up, horizontalInput * Speed * Time.deltaTime);

            transform1.position = player.transform.position; // Move focal point with player

        }
    }
}
