using UnityEngine;

namespace Course_Library.Scripts
{
    public class MissilesIndicatorRotation : IndicatorRotation
    {
        
        protected override void IndicatorRotate()
        {
            transform.Rotate(Vector3.up, -RotationSpeed * Time.deltaTime);
        }
    }
}
