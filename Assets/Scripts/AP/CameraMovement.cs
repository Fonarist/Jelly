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

        private Transform m_defTransform;

        private Vector3 m_offsetPos;
        private Vector3 m_offsetRot;

        void Awake()
        {
            m_actionSystem = FindObjectOfType<ActionSystem>();
            m_player = FindObjectOfType<Player>();

            m_defTransform = transform;
        }

        public void SetDefaultTransform()
        {
            transform.position = m_defTransform.position;
            transform.rotation = m_defTransform.rotation;
        }

        public void StartMove()
        {
            m_offsetPos = transform.position - m_player.transform.position;
        }

        void Update()
        {
            if(m_actionSystem.GetGameState())
            {
                Vector3 destination = m_player.transform.position + m_offsetPos;
                destination.y = transform.position.y;
                transform.position = Vector3.Lerp(transform.position, destination, m_speed * Time.deltaTime);

                //transform.LookAt(m_player.transform);
            }
        }
    }
}
