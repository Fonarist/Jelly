
using UnityEngine;

namespace Jelly
{
    public class Mover : MonoBehaviour
    {
        [SerializeField] private float m_speed;

        private Vector3 m_needPos;
        private bool m_isMove;

        void Awake()
        {
            m_isMove = false;
        }

        public void SetSpeed(float speed) { m_speed = speed; }

        public bool IsMoving() { return m_isMove; }
        public void Clear() { m_isMove = false; }

        public void MoveTo(Vector3 pos)
        {
            m_needPos = pos;
            m_isMove = true;
        }

        void FixedUpdate()
        {
            if (m_isMove)
            {
                Vector3 direction = m_needPos - transform.position;
                Vector3 offset = m_speed * Time.deltaTime * direction.normalized;

                transform.position += offset;

                float sqrMagnitude = offset.sqrMagnitude;
                float sqrMagnitudeMax = direction.sqrMagnitude;

                if (sqrMagnitude == 0 || sqrMagnitude > sqrMagnitudeMax || Mathf.Approximately(sqrMagnitudeMax, sqrMagnitude))
                {
                    transform.position = m_needPos;
                    m_isMove = false;
                }
            }
        }
    }
}
