using UnityEngine;

namespace Course_Library.Scripts
{
    public class IndicatorRotation : MonoBehaviour
    {

        protected const float RotationSpeed = 30;

        // Update is called once per frame
        protected void Update()
        {
            IndicatorRotate();
        }

        protected virtual void IndicatorRotate()
        {
            transform.Rotate(Vector3.up, RotationSpeed * Time.deltaTime);
        }
    }
}
