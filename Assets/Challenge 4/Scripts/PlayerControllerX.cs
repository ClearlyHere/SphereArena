using System.Collections;
using UnityEngine;

namespace Challenge_4.Scripts
{
    public class PlayerControllerX : MonoBehaviour
    {
        private Rigidbody _playerRb;
        private const float Speed = 500;
        private GameObject _focalPoint;

        public bool hasPowerup;
        public GameObject powerupIndicator;
        public int powerUpDuration = 5;

        private bool _hasDash = true;
        private int _dashCooldown = 3;
        private ParticleSystem _dashParticles;

        private const float DashStrength = 2000;
        private const float NormalStrength = 10; // how hard to hit enemy without powerup
        private const float PowerupStrength = 25; // how hard to hit enemy with powerup
    
        void Start()
        {
            _playerRb = GetComponent<Rigidbody>();
            _focalPoint = GameObject.Find("Focal Point");
            _dashParticles = GameObject.Find("Smoke_Particle").GetComponent<ParticleSystem>();
        }

        void Update()
        {
            // Add force to player in direction of the focal point (and camera)
            float verticalInput = Input.GetAxis("Vertical");
            _playerRb.AddForce(_focalPoint.transform.forward * (verticalInput * Speed * Time.deltaTime)); 

            // Set powerup indicator position to beneath player
            powerupIndicator.transform.position = transform.position + new Vector3(0, -0.6f, 0);

            Dash();
        }

        // If Player collides with powerup, activate powerup
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Powerup"))
            {
                Destroy(other.gameObject);
                hasPowerup = true;
                powerupIndicator.SetActive(true);
                StartCoroutine(PowerupCooldown());
            }
        }

        // Coroutine to count down powerup duration
        IEnumerator PowerupCooldown()
        {
            yield return new WaitForSeconds(powerUpDuration);
            hasPowerup = false;
            powerupIndicator.SetActive(false);
        }

        private void Dash()
        {
            if (Input.GetAxis("Dash") > 0 && _hasDash)
            {
                _playerRb.AddForce(_focalPoint.transform.forward * DashStrength);
                _dashParticles.Play();
                _hasDash = false;
                StartCoroutine(DashCooldown());
            }
        }
        
        IEnumerator DashCooldown()
        {
            yield return new WaitForSeconds(_dashCooldown);
            _hasDash = true;
        }

        // If Player collides with enemy
        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag("Enemy"))
            {
                Rigidbody enemyRigidbody = other.gameObject.GetComponent<Rigidbody>();
                Vector3 awayFromPlayer = other.gameObject.transform.position - transform.position; 
           
                if (hasPowerup) // if have powerup hit enemy with powerup force
                {
                    enemyRigidbody.AddForce(awayFromPlayer * PowerupStrength, ForceMode.Impulse);
                }
                else // if no powerup, hit enemy with normal strength 
                {
                    enemyRigidbody.AddForce(awayFromPlayer * NormalStrength, ForceMode.Impulse);
                }


            }
        }
        
    }
}
