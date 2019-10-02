using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jelly
{
    public class CameraMovement : MonoBehaviour
    {
        private ActionSystem m_actionSystem;
        private Player m_player;

        [SerializeField] float m_speed;

        private Vector3 m_defPos;

        private Vector3 m_offsetPos;
        private Vector3 m_offsetRot;

        void Awake()
        {
            m_actionSystem = FindObjectOfType<ActionSystem>();
            m_player = FindObjectOfType<Player>();

            m_defPos = transform.position;
        }

        public void SetDefaultTransform()
        {
            transform.position = m_defPos;

            Vector3 lookAtPos = new Vector3(m_player.transform.position.x, 0.0f, m_player.transform.position.z);
            transform.LookAt(lookAtPos);
        }

        public void StartMove()
        {
            m_offsetPos = transform.position - m_player.transform.position;
        }

        void Update()
        {
            if(m_actionSystem.GetGameState())
            {
                Vector3 destination = (m_player.transform.position + m_player.transform.forward * m_offsetPos.z) + m_player.transform.right * m_offsetPos.x;
                destination.y = transform.position.y;
                transform.position = Vector3.Lerp(transform.position, destination, m_speed * Time.deltaTime);

                Vector3 lookAtPos = new Vector3(m_player.transform.position.x, 0.0f, m_player.transform.position.z);
                transform.LookAt(lookAtPos);
            }
        }
    }
}
