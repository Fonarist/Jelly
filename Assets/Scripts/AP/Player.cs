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
        [SerializeField] private Transform m_target;
        [SerializeField] private GameObject m_camera;

        [SerializeField] private Vector3 m_defPos;
        private float m_offsetZCamera;

        private Rigidbody m_body;

        void Awake()
        {
            m_actionSystem = FindObjectOfType<ActionSystem>();
            m_body = GetComponent<Rigidbody>();

            SetDefaulTransform();

            m_offsetZCamera = m_camera.transform.position.z - transform.position.z;

        }

        void FixedUpdate()
        {
            if (m_actionSystem.GetGameState())
            {
                Vector3 direction = m_target.position - transform.position;
                m_body.velocity = new Vector3(
                    m_speed * direction.normalized.x, m_body.velocity.y, m_speed * direction.normalized.z);

                m_camera.transform.position = new Vector3(
                    m_camera.transform.position.x, m_camera.transform.position.x, transform.position.z + m_offsetZCamera);
            }
        }

        public void SetDefaulTransform()
        {
            transform.position = m_defPos;
            transform.localScale = new Vector3(1.0f, 1.0f, transform.localScale.z);
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
                m_body.AddForce(new Vector3(0.0f, 0.0f, -20.0f), ForceMode.Impulse);
                Destroy(collision.gameObject);
            }
        }
    }

}