using UnityEngine;
using UnityEngine.UI;

namespace Jelly
{
    public class Enemy : MonoBehaviour
    {
        private Field m_field;
        private ActionSystem m_actionSystem;
        private Mover m_mover;

        void Awake()
        {
            m_field = FindObjectOfType<Field>();
            m_actionSystem = FindObjectOfType<ActionSystem>();
            m_mover = GetComponent<Mover>();
        }

        public void Init(int level)
        {

        }

        void FixedUpdate()
        {
            if(!m_mover.IsMoving())
            {
                m_field.DeleteEnemy(this);
            }
        }

        public void MoveTo(Vector3 pos)
        {
            m_mover.MoveTo(pos);
        }

        public void SetSpeed(float speed) { m_mover.SetSpeed(speed); }
    }

}