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

        private Vector3 m_defPos;
        private Rigidbody m_body;

        private int m_curDest;
        private List<Vector3> m_destinations;

        private float m_reverseTimer;

        void Awake()
        {
            m_actionSystem = FindObjectOfType<ActionSystem>();
            m_body = GetComponent<Rigidbody>();

            m_destinations = new List<Vector3>();

            m_defPos = transform.position;
            m_reverseTimer = 0.0f;
        }

        void FixedUpdate()
        {
            if (m_actionSystem.GetGameState() && m_destinations.Count > 0)
            {
                float speed = m_speed;
                if (m_reverseTimer > 0.0f)
                {
                    m_reverseTimer -= Time.deltaTime;
                    speed = -speed;
                }

                Vector3 direction = m_destinations[m_curDest] - transform.position;

                m_body.velocity = new Vector3(speed * direction.normalized.x, m_body.velocity.y, speed * direction.normalized.z);

                if (direction.magnitude < 1.0f)
                {
                    m_curDest++;
                }

                Vector3 front = transform.forward;
                front.y = 0;
                front.Normalize();

                Vector3 vToTarget = m_destinations[m_curDest] - transform.position;
                vToTarget.y = 0;
                vToTarget.Normalize();

                float angle = Vector3.Angle(front, vToTarget);

                if(Vector3.Dot(front, vToTarget) < 0.0f)
                {
                    angle = 180 - angle;
                }

                Vector3 right = transform.right;
                right.y = 0;
                right.Normalize();
                float rightAngle = Vector3.Angle(right, vToTarget);
                if(rightAngle > 90)
                {
                    angle = -angle;
                }

                transform.Rotate(new Vector3(0, 1, 0), angle * Time.deltaTime * 13);
            }
        }

        public void SetDefaultTransform()
        {
            m_body.velocity = new Vector3();

            transform.position = m_defPos;
            transform.rotation = new Quaternion();
            transform.localScale = new Vector3(1.0f, 1.0f, transform.localScale.z);

            m_destinations.Clear();
            m_curDest = 0;
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
            if(collision.gameObject.tag == "Finish")
            {
                m_actionSystem.Win();
            }
        }

        void OnTriggerEnter(Collider collision)
        {
            if (collision.gameObject.tag == "Enemy")
            {
                if (m_reverseTimer <= 0)
                {
                    m_reverseTimer = 0.25f;
                }

                Destroy(collision.gameObject);
            }
        }
    }

}