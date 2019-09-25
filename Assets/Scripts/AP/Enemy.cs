using UnityEngine;
using UnityEngine.UI;

namespace Jelly
{
    public class Enemy : MonoBehaviour
    {
        private ActionSystem m_actionSystem;

        void Awake()
        {
            m_actionSystem = FindObjectOfType<ActionSystem>();
        }

        public void Init(int level)
        {

        }
    }

}