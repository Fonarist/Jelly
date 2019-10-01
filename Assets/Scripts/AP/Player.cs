using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Jelly
{
    public class Player : MonoBehaviour
    {
        private ActionSystem m_actionSystem;

        [SerializeField] private float m_maxScale;
        [SerializeField] private float m_minScale;

        [SerializeField] private float m_speed;

        private Transform m_defTransform;
        private Rigidbody m_body;
        private float m_stopTimer;

        private int m_curDest;
        private List<Vector3> m_destinations;

        private float m_rotateTimer;

        void Awake()
        {
            m_actionSystem = FindObjectOfType<ActionSystem>();
            m_body = GetComponent<Rigidbody>();

            m_destinations = new List<Vector3>();

            m_defTransform = transform;
            m_stopTimer = 0.0f;
            m_rotateTimer = 0.0f;
        }

        void FixedUpdate()
        {
            if (m_actionSystem.GetGameState())
            {
                if(m_stopTimer > 0.0f)
                {
                    m_stopTimer -= Time.deltaTime;
                }
                else
                {
                    Vector3 direction = m_destinations[m_curDest] - transform.position;

                    m_body.velocity = new Vector3(m_speed * direction.normalized.x, m_body.velocity.y, m_speed * direction.normalized.z);

                    if (direction.magnitude < 1.0f)
                    {
                        m_curDest++;
                        m_rotateTimer = 0.5f;
                    }
                }

                if(/*m_rotateTimer > 0.0f*/true)
                {
                    /*m_rotateTimer -= Time.deltaTime;
                    if (m_rotateTimer <= 0.0f)
                        m_rotateTimer = 0.0f;

                    Vector3 direction = m_destinations[m_curDest] - transform.position;
                    direction.Normalize();

                    Quaternion deltaRotation = Quaternion.Euler(direction * Time.deltaTime * 100);
                    m_body.MoveRotation(m_body.rotation * deltaRotation);*/

                    //transform.LookAt(m_destinations[m_curDest]);
                }
            }
        }

        public void SetDefaultTransform()
        {
            transform.position = m_defTransform.position;
            transform.rotation = m_defTransform.rotation;
            transform.localScale = new Vector3(1.0f, 1.0f, m_defTransform.localScale.z);

            m_destinations.Clear();
            m_curDest = 0;
        }

        public void Grow(float percent)
        {
            float val = (m_maxScale - m_minScale) * percent;

            float newXScale = transform.localScale.x - val;
            if(newXScale > m_maxScale)
            {
                newXScale = m_maxScale;
            }
            else if(newXScale < m_minScale)
            {
                newXScale = m_minScale;
            }

            float newYScale = transform.localScale.y + val;
            if (newYScale > m_maxScale)
            {
                newYScale = m_maxScale;
            }
            else if(newYScale < m_minScale)
            {
                newYScale = m_minScale;
            }

            transform.localScale = new Vector3(newXScale, newYScale, transform.localScale.z);
        }

        public void AddDestination(Vector3 pos)
        {
            m_destinations.Add(pos);
        }

        void OnCollisionEnter(Collision collision)
        {
            if(collision.gameObject.tag == "Finish")
            {
                m_actionSystem.Win();
            }
        }

        void OnTriggerEnter(Collider collision)
        {
            if (collision.gameObject.tag == "Enemy")
            {
                m_body.AddForce(new Vector3(0.0f, 0.0f, -4.0f), ForceMode.Impulse);
                Destroy(collision.gameObject);

                if(m_stopTimer <= 0)
                {
                    m_stopTimer = 0.3f;
                }
            }
        }
    }

}