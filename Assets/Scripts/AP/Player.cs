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
        [SerializeField] private Vector3 m_defPosCamera;

        void Awake()
        {
            m_actionSystem = FindObjectOfType<ActionSystem>();

            SetDefaulTransform();
        }

        void FixedUpdate()
        {
            if (m_actionSystem.GetGameState())
            {
                Vector3 direction = m_target.position - transform.position;
                Vector3 offset = m_speed * Time.deltaTime * direction.normalized;

                transform.position += offset;
                m_camera.transform.position += offset;

                float sqrMagnitude = offset.sqrMagnitude;
                float sqrMagnitudeMax = direction.sqrMagnitude;

                if (sqrMagnitude == 0 || sqrMagnitude > sqrMagnitudeMax || Mathf.Approximately(sqrMagnitudeMax, sqrMagnitude))
                {
                    transform.position = m_target.position;
                }
            }
        }

        public void SetDefaulTransform()
        {
            m_camera.transform.position = m_defPosCamera;

            transform.position = m_defPos;
            transform.localScale = new Vector3(1.0f, 1.0f, transform.localScale.z);
        }

        public void Grow(float percent)
        {
            float val = (m_maxScale - m_minScale) * percent;

            float newXScale = transform.localScale.x + val;
            if(newXScale > m_maxScale)
            {
                newXScale = m_maxScale;
            }
            else if(newXScale < m_minScale)
            {
                newXScale = m_minScale;
            }

            float newYScale = transform.localScale.y - val;
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
                FindObjectOfType<ActionSystem>().SetGameState(false);
                FindObjectOfType<MainMenu>().UpdateUI();
                FindObjectOfType<Player>().SetDefaulTransform();
            }
        }
    }

}