using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Jelly
{
    public class Player : MonoBehaviour
    {
        private GameManager m_gameManager;
        private ActionSystem m_actionSystem;

        [SerializeField] private float m_maxScale;
        [SerializeField] private float m_minScale;

        [SerializeField] private float m_additionalSpeed;
        private float m_addSpeedTimer;

        [SerializeField] private float m_reverseTime;
        private float m_reverseTimer;

        private float m_speed;

        private Vector3 m_defPos;
        private Rigidbody m_body;

        private int m_curDest;
        private List<Vector3> m_destinations;



        private bool m_isFalling;

        void Awake()
        {
            m_gameManager = FindObjectOfType<GameManager>();
            m_actionSystem = FindObjectOfType<ActionSystem>();
            m_body = GetComponent<Rigidbody>();

            m_destinations = new List<Vector3>();

            m_defPos = transform.position;

            SetDefault();
        }

        void FixedUpdate()
        {
            if (m_actionSystem.GetGameState())
            {
                UpdateSpeed();

                if (transform.position.y < -5.0f)
                {
                    m_actionSystem.Lose();
                }
                else if (m_destinations.Count > 0 && !m_isFalling)
                {
                    float speed = m_speed;
                    if (m_reverseTimer > 0.0f)
                    {
                        m_reverseTimer -= Time.deltaTime;
                        speed *= m_reverseTimer * m_reverseTime;
                        speed *= -1;
                    }


                    Vector3 direction = m_destinations[m_curDest] - transform.position;
                    direction.y = 0;

                    if (direction.magnitude < 0.4f)
                    {
                        transform.position = new Vector3(m_destinations[m_curDest].x, transform.position.y, m_destinations[m_curDest].z);

                        m_curDest++;

                        direction = m_destinations[m_curDest] - transform.position;
                        direction.y = 0;
                    }

                    m_body.velocity = new Vector3(speed * direction.normalized.x, m_body.velocity.y, speed * direction.normalized.z);

                    Vector3 front = transform.forward;
                    front.y = 0;
                    front.Normalize();

                    direction.Normalize();

                    float angle = Vector3.Angle(front, direction);

                    if (Vector3.Dot(front, direction) < 0.0f)
                    {
                        angle = 180 - angle;
                    }

                    Vector3 right = transform.right;
                    right.y = 0;
                    right.Normalize();
                    float rightAngle = Vector3.Angle(right, direction);
                    if (rightAngle > 90)
                    {
                        angle = -angle;
                    }

                    transform.Rotate(new Vector3(0, 1, 0), angle * Time.deltaTime * 13);
                }
            }
        }

        private void UpdateSpeed()
        {
            float needSpeed = Formulas.CalculatePlayerSpeed(m_gameManager.GetLevel()) + m_additionalSpeed;
            if(m_speed < needSpeed)
            {
                m_addSpeedTimer -= Time.deltaTime;
                if(m_addSpeedTimer <= 0.0f)
                {
                    m_speed += 1.0f;
                    m_addSpeedTimer = 2.0f;
                }
            }
        }

        public void SetDefault()
        {
            m_body.velocity = new Vector3();

            transform.position = m_defPos;
            transform.rotation = new Quaternion();
            transform.localScale = new Vector3(1.0f, 1.0f, transform.localScale.z);

            m_destinations.Clear();
            m_curDest = 0;

            m_reverseTimer = 0.0f;
            m_isFalling = false;

            m_speed = Formulas.CalculatePlayerSpeed(m_gameManager.GetLevel());

            m_addSpeedTimer = 2.0f;
        }

        public void Grow(float percent)
        {
            float val = (m_maxScale - m_minScale) * percent;

            float newXScale = m_body.transform.localScale.x - val;
            if(newXScale > m_maxScale)
            {
                newXScale = m_maxScale;
            }
            else if(newXScale < m_minScale)
            {
                newXScale = m_minScale;
            }

            float newYScale = m_body.transform.localScale.y + val;
            if (newYScale > m_maxScale)
            {
                newYScale = m_maxScale;
            }
            else if(newYScale < m_minScale)
            {
                newYScale = m_minScale;
            }

            m_body.transform.localScale = new Vector3(newXScale, newYScale, transform.localScale.z);
        }

        public void AddDestination(Vector3 pos)
        {
            m_destinations.Add(pos);
        }

        void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.tag == "Finish")
            {
                m_actionSystem.Win();
            }
            else if (collision.gameObject.tag == "Enemy")
            {
                if (m_reverseTimer <= 0)
                {
                    m_reverseTimer = m_reverseTime;
                    /*Vector3 v = transform.forward * -10.0f;
                    v.y = 1.5f;
                    m_body.AddForce(v, ForceMode.Impulse);*/
                }

                Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();

                rb.constraints = RigidbodyConstraints.None;

                /*Vector3 v = transform.forward * 10.0f;
                v.y = 20.0f;
                rb.AddForce(v, ForceMode.Impulse);*/

                Physics.IgnoreCollision(collision.collider, GetComponent<Collider>());

                Destroy(collision.gameObject, 5.0f);
            }
            else if(collision.gameObject.tag == "Fall")
            {
                m_isFalling = true;
                m_body.velocity = new Vector3();
            }
            else if (collision.gameObject.tag == "Diamond")
            {
                m_gameManager.AddMoney(1);
                Destroy(collision.gameObject);
            }
        }
    }

}