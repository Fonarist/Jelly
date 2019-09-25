using UnityEngine;
using UnityEngine.EventSystems;

namespace Jelly
{
    public class Controls : MonoBehaviour
    {
        private Player m_player;
        private ActionSystem m_actionSystem;

        [SerializeField] private float m_lenghtToStart;
        [SerializeField] private float m_distanceFullScale;

        private Vector3 m_pointStart;
        private bool m_wasTouch;


        // Use this for initialization
        void Start()
        {
            m_player = FindObjectOfType<Player>();
            m_actionSystem = FindObjectOfType<ActionSystem>();

            m_wasTouch = false;
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                m_pointStart = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.transform.position.z);
            }
            else if (Input.GetMouseButton(0))
            {
                Vector3 pointEnd = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.transform.position.z);
                float curDist = pointEnd.y - m_pointStart.y;
                if (!m_actionSystem.GetGameState())
                {
                    if (Mathf.Abs(curDist) > m_lenghtToStart)
                    {
                        m_actionSystem.SetGameState(true);
                        m_pointStart = pointEnd;
                    }
                }
                else
                {
                    m_player.Grow(curDist / m_distanceFullScale);
                    m_pointStart = pointEnd;
                }
            }
        }

        private bool IsOnUI(Vector3 pos)
        {
            int id = -1;
            if (Input.touchCount > 0)
            {
                id = Input.GetTouch(0).fingerId;
            }

            if (EventSystem.current.IsPointerOverGameObject(id))
            {
                if (EventSystem.current.currentSelectedGameObject
                    && EventSystem.current.currentSelectedGameObject.gameObject.tag == "Menu")
                {
                    return true;
                }
            }

            return false;
        }
    }
}
