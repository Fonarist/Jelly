using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Jelly
{
    public class Player : MonoBehaviour
    {
        private GameManager m_gameManager;
        private ActionSystem m_actionSystem;
        private MainMenu m_mainMenu;

        [SerializeField] private float m_maxScale;
        [SerializeField] private float m_minScale;

        [SerializeField] private float m_additionalSpeed;
        private float m_addSpeedTimer;

        [SerializeField] private float m_reverseTime;
        private float m_reverseTimer;

        [SerializeField] private float m_maxDistProjection;
        [SerializeField] private GameObject m_projBoxPref;
        private GameObject m_projectionBox;

        [SerializeField] private float m_rushModeTime;
        [SerializeField] private RushCollider m_rushModeCollider;
        private float m_rushModeTimer;
        private bool m_isInRushMode;
        private bool m_wasInRushMode;

        private float m_speed;

        private Vector3 m_defPos;
        private Rigidbody m_body;
        private BoxCollider m_collider;

        private int m_curDest;
        private List<Vector3> m_destinations;

        private bool m_isFalling;

        void Awake()
        {
            m_gameManager = FindObjectOfType<GameManager>();
            m_actionSystem = FindObjectOfType<ActionSystem>();
            m_mainMenu = FindObjectOfType<MainMenu>();

            m_body = GetComponent<Rigidbody>();
            m_collider = FindObjectOfType<BoxCollider>();

            m_destinations = new List<Vector3>();

            m_defPos = transform.position;

            SetDefault();
        }

        void FixedUpdate()
        {
            if (m_actionSystem.GetGameState())
            {
                if (transform.position.y < -5.0f)
                {
                    Time.timeScale = 0;
                    m_actionSystem.SetWinState(false);
                    FindObjectOfType<AdsManager>().ShowAd();
                }
                else
                {
                    UpdateSpeed();

                    UpdateProjection();

                    UpdateRushMode();

                    UpdateMovement();
                }
            }
        }

        private void UpdateSpeed()
        {
            float needSpeed = Formulas.CalculatePlayerSpeed(m_gameManager.GetLevel()) + m_additionalSpeed;
            if(m_speed < needSpeed)
            {
                m_addSpeedTimer -= Time.deltaTime;
                if(m_addSpeedTimer <= 0.0f)
                {
                    m_speed += 1.0f;
                    m_addSpeedTimer = 2.0f;
                }
            }
        }

        private void UpdateProjection()
        {
            if (m_isInRushMode)
                return;

            LayerMask masks = LayerMask.GetMask("Enemy");

            RaycastHit hit;
            bool isColl = Physics.BoxCast(transform.position, transform.localScale, transform.forward, out hit, transform.rotation, m_maxDistProjection, masks);
            if(isColl)
            {
                if(!m_projectionBox)
                    m_projectionBox = Instantiate(m_projBoxPref, FindObjectOfType<Field>().transform);

                m_projectionBox.transform.rotation = transform.rotation;
                m_projectionBox.transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, hit.distance - 0.15f);
                m_projectionBox.transform.position = transform.position +  transform.forward.normalized * (hit.distance / 2.0f + 0.3f);
            }
            else
            {
                Destroy(m_projectionBox);
            }
        }

        private void UpdateRushMode()
        {
            if(m_wasInRushMode)
                return;

            if(m_rushModeTimer > 0)
            {
                m_rushModeTimer -= Time.deltaTime;
            }

            if (m_isInRushMode)
            {
                if(m_rushModeTimer <= 0)
                {
                    m_mainMenu.UpdateBar(-1.0f);

                    m_rushModeCollider.Disable();

                    m_isInRushMode = false;
                    m_wasInRushMode = true;
                }
                else
                {
                    m_mainMenu.UpdateBar(m_rushModeTimer / m_rushModeTime);
                }
            }
            else
            {
                if (m_rushModeTimer <= 0)
                {
                    m_isInRushMode = true;
                    m_rushModeTimer = m_rushModeTime;

                    m_rushModeCollider.Enable();
                }

                m_mainMenu.UpdateBar(1.0f - m_rushModeTimer / m_rushModeTime);
            }

        }

        private void UpdateMovement()
        {
            if (m_destinations.Count > 0 && !m_isFalling)
            {
                float speed = m_speed;
                if (m_reverseTimer > 0.0f)
                {
                    m_reverseTimer -= Time.deltaTime;
                    speed *= m_reverseTimer * m_reverseTime;
                    speed *= -1;
                }


                Vector3 direction = m_destinations[m_curDest] - transform.position;
                direction.y = 0;

                if (direction.magnitude < 0.4f)
                {
                    transform.position = new Vector3(m_destinations[m_curDest].x, transform.position.y, m_destinations[m_curDest].z);

                    m_curDest++;

                    if (m_curDest >= m_destinations.Count)
                        m_curDest = 0;

                    direction = m_destinations[m_curDest] - transform.position;
                    direction.y = 0;
                }

                m_body.velocity = new Vector3(speed * direction.normalized.x, m_body.velocity.y, speed * direction.normalized.z);

                Vector3 front = transform.forward;
                front.y = 0;
                front.Normalize();

                direction.Normalize();

                float angle = Vector3.Angle(front, direction);

                if (Vector3.Dot(front, direction) < 0.0f)
                {
                    angle = 180 - angle;
                }

                Vector3 right = transform.right;
                right.y = 0;
                right.Normalize();
                float rightAngle = Vector3.Angle(right, direction);
                if (rightAngle > 90)
                {
                    angle = -angle;
                }

                transform.Rotate(new Vector3(0, 1, 0), angle * Time.deltaTime * 13);
            }
        }

        public void SetDefault()
        {
            m_body.velocity = new Vector3();

            transform.position = m_defPos;
            transform.rotation = new Quaternion();
            transform.localScale = new Vector3(1.0f, 1.0f, transform.localScale.z);

            m_destinations.Clear();
            m_curDest = 0;

            m_reverseTimer = 0.0f;
            m_isFalling = false;

            m_rushModeTimer = m_rushModeTime;
            m_isInRushMode = false;
            m_wasInRushMode = false;

            m_speed = Formulas.CalculatePlayerSpeed(m_gameManager.GetLevel());

            m_addSpeedTimer = 2.0f;
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
            if (collision.gameObject.tag == "Finish")
            {
                Time.timeScale = 0;
                m_actionSystem.SetWinState(true);
                Destroy(collision.gameObject);
                FindObjectOfType<AdsManager>().ShowAd();
            }
            else if (collision.gameObject.tag == "Enemy")
            {
                Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();

                if (rb.constraints == RigidbodyConstraints.FreezePosition)
                {
                    if (m_reverseTimer <= 0)
                    {
                        m_reverseTimer = m_reverseTime;
                    }

                    if (!m_isInRushMode)
                    {
                        m_rushModeTimer = m_rushModeTime;
                    }

                    rb.constraints = RigidbodyConstraints.None;

                    Vector3 v = collision.transform.position - transform.position;
                    v.Normalize();

                    if (v.y < 0.2f)
                        v.y = 0.2f;

                    v *= 3.0f;

                    rb.AddForce(v, ForceMode.Impulse);

                    Physics.IgnoreCollision(collision.collider, GetComponent<Collider>());
                    Destroy(collision.gameObject, 5.0f);
                }

                if(m_gameManager.IsVibroEnabled())
                {
                    Handheld.Vibrate();
                }
            }
            else if(collision.gameObject.tag == "Fall")
            {
                m_isFalling = true;
                m_body.velocity = new Vector3();
            }
            else if (collision.gameObject.tag == "Diamond")
            {
                m_gameManager.AddMoney(1);
                Destroy(collision.gameObject);
            }
        }
    }

}