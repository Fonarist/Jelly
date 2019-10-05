using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jelly
{
    public class RushCollider : MonoBehaviour
    {
        private Player m_player;

        // Start is called before the first frame update
        void Start()
        {
            m_player = FindObjectOfType<Player>();
        }

        public void Enable()
        {
            gameObject.SetActive(true);
        }

        public void Disable()
        {
            gameObject.SetActive(false);
        }

        void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.tag == "Enemy")
            {
                Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();

                if (rb.constraints == RigidbodyConstraints.FreezePosition)
                {
                    rb.constraints = RigidbodyConstraints.None;

                    Vector3 v = collision.transform.position - transform.position;
                    v.Normalize();

                    if (v.y < 0.2f)
                        v.y = 0.2f;

                    v *= 15.0f;

                    rb.AddForce(v, ForceMode.Impulse);

                    Physics.IgnoreCollision(collision.collider, GetComponent<Collider>());
                    Physics.IgnoreCollision(collision.collider, m_player.GetComponent<Collider>());
                    Destroy(collision.gameObject, 5.0f);
                }
            }
        }
    }
}
