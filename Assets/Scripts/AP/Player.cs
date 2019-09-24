using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Jelly
{
    public class Player : MonoBehaviour
    {
        private ActionSystem m_actionSystem;
        private Field m_field;
        private Mover m_mover;

        [SerializeField] private float m_maxScale;
        [SerializeField] private float m_minScale;

        void Awake()
        {
            m_actionSystem = FindObjectOfType<ActionSystem>();
            m_field = FindObjectOfType<Field>();
            m_mover = GetComponent<Mover>();

            SetDefaultPosition();
        }

        public void SetDefaultPosition()
        {
            m_mover.Clear();
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

        void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.gameObject.tag == "Enemy")
            {
                m_actionSystem.ClearScore();

                m_field.DeleteEnemies();
            }
        }
    }

}