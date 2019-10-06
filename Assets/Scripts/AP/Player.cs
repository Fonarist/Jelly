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
        private VfxManager m_vfxManager;

        [SerializeField] private float m_maxScale;
        [SerializeField] private float m_minScale;

        [SerializeField] private float m_acc;
        [SerializeField] private float m_additionalSpeed;
        private float m_leftAddSpeed;
        private float m_addSpeedTimer;
        private float m_needSpeed;

        [SerializeField] private float m_reverseTime;
        private float m_reverseTimer;

        [SerializeField] private float m_maxDistProjection;
        [SerializeField] private GameObject m_projBoxPref;
        private GameObject m_projectionBox;

        [SerializeField] private GameObject m_back;
        [SerializeField] private GameObject m_backRush;
        [SerializeField] private float m_rushModeTime;
        [SerializeField] private RushCollider m_rushModeCollider;
        private float m_rushModeTimer;
        private bool m_isInRushMode;
        private bool m_wasInRushMode;
        private int m_rushModeProgress;

        private float m_speed;

        private Vector3 m_defPos;
        private Rigidbody m_body;

        private int m_curDest;
        private List<Vector3> m_destinations;

        private bool m_isFalling;

        private float m_finishTimer;

        private float m_enemyPassedTimer;

        void Awake()
        {
            m_gameManager = FindObjectOfType<GameManager>();
            m_actionSystem = FindObjectOfType<ActionSystem>();
            m_mainMenu = FindObjectOfType<MainMenu>();
            m_vfxManager = FindObjectOfType<VfxManager>();

            m_body = GetComponent<Rigidbody>();

            m_destinations = new List<Vector3>();

            m_defPos = transform.position;

            SetDefault();
        }

        void FixedUpdate()
        {
            if (m_actionSystem.GetGameState())
            {
                if(m_finishTimer > 0.0f)
                {
                    m_finishTimer -= Time.deltaTime;

                    if(m_finishTimer <= 0.0f)
                    {
                        Time.timeScale = 0;
                        FindObjectOfType<AdsManager>().ShowAd();
                    }
                }
                else
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

                        UpdatePassedEnemies();
                    }
                }
            }
        }

        private void UpdatePassedEnemies()
        {
            if(m_enemyPassedTimer > 0.0f)
            {
                m_enemyPassedTimer -= Time.deltaTime;
                if(m_enemyPassedTimer <= 0.0f)
                {
                    m_rushModeProgress++;
                }
            }
        }

        private void UpdateSpeed()
        {
            if (m_addSpeedTimer > 0)
            {
                m_addSpeedTimer -= Time.deltaTime;
            }
            else if(m_leftAddSpeed > 0.0f)
            {
                m_addSpeedTimer = 2.0f;

                m_leftAddSpeed -= 1.0f;
                m_needSpeed += 1.0f;
            }

            float needSpeed = m_needSpeed;

            if (m_isInRushMode)
            {
                needSpeed *= 1.3f;
                m_speed = needSpeed;
            }
            else if (m_speed < needSpeed)
            {
                m_speed += m_acc * Time.deltaTime;
            }
            else if(m_speed > needSpeed)
            {
                m_speed = needSpeed;
            }
        }

        private void UpdateProjection()
        {
            if (m_isInRushMode)
            {
                if (m_projectionBox)
                {
                    Destroy(m_projectionBox);
                }

                return;
            }

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

            if (m_isInRushMode)
            {
                m_rushModeTimer -= Time.deltaTime;

                if (m_rushModeTimer <= 0)
                {
                    m_mainMenu.UpdateBar(-1.0f);

                    m_rushModeCollider.Disable();

                    m_backRush.SetActive(false);
                    m_back.SetActive(true);

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
                if (m_rushModeProgress == 5)
                {
                    m_isInRushMode = true;
                    m_rushModeTimer = m_rushModeTime;

                    m_rushModeCollider.Enable();

                    m_back.SetActive(false);
                    m_backRush.SetActive(true);
                }

                m_mainMenu.UpdateBar(m_rushModeProgress / 5.0f);
            }
        }

        private void UpdateMovement()
        {
            if (m_destinations.Count > 0 && !m_isFalling)
            {
                if (m_reverseTimer > 0.0f)
                {
                    m_reverseTimer -= Time.deltaTime;
                }
                else
                {
                    Vector3 direction = m_destinations[m_curDest] - transform.position;
                    direction.y = 0;

                    if (direction.magnitude < 0.6f)
                    {
                        transform.position = new Vector3(m_destinations[m_curDest].x, transform.position.y, m_destinations[m_curDest].z);

                        m_curDest++;

                        if (m_curDest >= m_destinations.Count)
                        {
                            return;
                        }

                        direction = m_destinations[m_curDest] - transform.position;
                        direction.y = 0;
                    }

                    m_body.velocity = new Vector3(m_speed * direction.normalized.x, m_body.velocity.y, m_speed * direction.normalized.z);

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
            m_rushModeProgress = 0;

            m_speed = 0.0f;

            m_addSpeedTimer = 2.0f;
            m_leftAddSpeed = m_additionalSpeed;

            m_backRush.SetActive(false);
            m_back.SetActive(true);

            m_finishTimer = 0.0f;
            m_enemyPassedTimer = 0.0f;

            m_needSpeed = Formulas.CalculatePlayerSpeed(m_gameManager.GetLevel());
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
                m_vfxManager.Play(VfxManager.VfxType.FINISH_1, collision.gameObject.transform.position + new Vector3(0.0f, 1.0f, 0.0f));
                m_vfxManager.Play(VfxManager.VfxType.FINISH_2, collision.gameObject.transform.position + new Vector3(0.0f, 3.5f, 0.0f));

                m_finishTimer = 2.0f;
                m_body.velocity = new Vector3();
                m_actionSystem.SetWinState(true);

                transform.position = collision.gameObject.transform.position;

                Physics.IgnoreCollision(collision.collider, GetComponent<Collider>());
                Destroy(collision.gameObject);
            }
            else if (collision.gameObject.tag == "Enemy")
            {
                Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();

                if (rb.constraints == RigidbodyConstraints.FreezePosition)
                {
                    m_vfxManager.Play(VfxManager.VfxType.COLLISION_ENEMY, collision.contacts[0].point);

                    if (m_reverseTimer <= 0)
                    {
                        m_reverseTimer = m_reverseTime;

                        Vector3 vec = transform.forward.normalized * -8.0f;
                        vec.y = 3.0f;

                        m_body.velocity = new Vector3();
                        m_body.AddForce(vec, ForceMode.Impulse);

                        m_speed = 0;
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

                    m_enemyPassedTimer = 0.0f;
                    m_rushModeProgress = 0;
                }

                if(m_gameManager.IsVibroEnabled())
                {
                    //Handheld.Vibrate();
                }
            }
            else if(collision.gameObject.tag == "Fall")
            {
                m_isFalling = true;
                m_body.velocity = new Vector3();
            }
            else if (collision.gameObject.tag == "Diamond")
            {
                m_vfxManager.Play(VfxManager.VfxType.COLLISION_DIAMOND, collision.gameObject.transform.position);

                m_gameManager.AddMoney(1);
                Destroy(collision.gameObject);
            }
            else if(collision.gameObject.tag == "EnemyPassed")
            {
                m_enemyPassedTimer = 0.1f;
                Physics.IgnoreCollision(collision.collider, GetComponent<Collider>());
            }
        }
    }

}