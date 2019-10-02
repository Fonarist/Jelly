using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jelly
{
    public class Field : MonoBehaviour
    {

        enum TurnType
        {
            NONE = 0,
            LEFT,
            RIGHT
        }

        private Player m_player;
        private GameManager m_gameManager;

        [SerializeField] private List<GameObject> m_blocksPref;
        [SerializeField] private GameObject m_blockLeftPref;
        [SerializeField] private GameObject m_blockLeftAfterPref;
        [SerializeField] private GameObject m_blockRightPref;
        [SerializeField] private GameObject m_blockRightAfterPref;
        [SerializeField] private GameObject m_blockFinishPref;

        [SerializeField] private Vector3 m_offsetBlock;
        [SerializeField] private float m_offsetTurnBlock;

        private List<GameObject> m_objects;

        // Start is called before the first frame update
        void Awake()
        {
            m_player = FindObjectOfType<Player>();
            m_gameManager = FindObjectOfType<GameManager>();

            m_objects = new List<GameObject>();
        }

        void Start()
        {
            Generate();
        }

        public void Reset()
        {
            for(int i = 0; i < m_objects.Count; ++i)
            {
                Destroy(m_objects[i]);
            }
            m_objects.Clear();

            Generate();
        }

        public void Generate()
        {
            int count = Formulas.CalculateCountBlocks(m_gameManager.GetLevel());

            m_objects.Clear();

            int randF = Random.Range(0, m_blocksPref.Count);
            GameObject blockFirst = Instantiate(m_blocksPref[randF], transform);
            blockFirst.transform.position = new Vector3(0.0f, m_offsetBlock.y, m_offsetBlock.z);

            m_objects.Add(blockFirst);

            TurnType prevTurn = TurnType.NONE;

            for (int i = 0; i < count; ++i)
            {
                Vector3 prevBlockPos = m_objects[m_objects.Count - 1].transform.position;

                if (i == count - 1) //create finish
                {
                    GameObject finish = Instantiate(m_blockFinishPref, transform);

                    finish.transform.position = new Vector3(prevBlockPos.x, prevBlockPos.y, prevBlockPos.z + m_offsetBlock.z + m_offsetTurnBlock);

                    m_player.AddDestination(finish.transform.position);

                    m_objects.Add(finish);
                }
                else //turn
                {
                    TurnType newTurn = TurnType.NONE;
                    if(prevTurn == TurnType.NONE)
                    {
                        int r = Random.Range(0, 2);
                        newTurn = r == 0 ? TurnType.LEFT : TurnType.RIGHT;
                    }
                    else
                    {
                        newTurn = prevTurn == TurnType.LEFT ? TurnType.RIGHT : TurnType.LEFT;
                    }

                    if (newTurn == TurnType.LEFT)
                    {
                        GameObject left;

                        int rand = Random.Range(0, m_blocksPref.Count);
                        GameObject block = Instantiate(m_blocksPref[randF], transform);

                        if (prevTurn == TurnType.NONE)
                        {
                            left = Instantiate(m_blockLeftPref, transform);
                            left.transform.position = new Vector3(prevBlockPos.x, left.transform.position.y, prevBlockPos.z + m_offsetBlock.z + m_offsetTurnBlock);

                            block.transform.position = new Vector3(
                                prevBlockPos.x - m_offsetBlock.z - m_offsetTurnBlock, prevBlockPos.y, prevBlockPos.z + m_offsetBlock.z + m_offsetTurnBlock);

                            block.transform.Rotate(new Vector3(0f, -1.0f, 0.0f), 90);
                        }
                        else
                        {
                            left = Instantiate(m_blockLeftAfterPref, transform); 
                            left.transform.position = new Vector3(prevBlockPos.x + m_offsetBlock.z + m_offsetTurnBlock, left.transform.position.y, prevBlockPos.z);

                            block.transform.position = new Vector3(
                                prevBlockPos.x + m_offsetBlock.z + m_offsetTurnBlock, prevBlockPos.y, prevBlockPos.z + m_offsetBlock.z + m_offsetTurnBlock);
                        }

                        prevTurn = prevTurn == TurnType.NONE ? TurnType.LEFT : TurnType.NONE;

                        m_objects.Add(left);
                        m_objects.Add(block);

                        m_player.AddDestination(left.transform.position);
                    }
                    else
                    {
                        GameObject right;

                        int rand = Random.Range(0, m_blocksPref.Count);
                        GameObject block = Instantiate(m_blocksPref[randF], transform);

                        if (prevTurn == TurnType.NONE)
                        {
                            right = Instantiate(m_blockRightPref, transform);
                            right.transform.position = new Vector3(prevBlockPos.x, right.transform.position.y, prevBlockPos.z + m_offsetBlock.z + m_offsetTurnBlock);

                            block.transform.position = new Vector3(
                                prevBlockPos.x + m_offsetBlock.z + m_offsetTurnBlock, prevBlockPos.y, prevBlockPos.z + m_offsetBlock.z + m_offsetTurnBlock);

                            block.transform.Rotate(new Vector3(0f, 1.0f, 0.0f), 90);
                        }
                        else
                        {
                            right = Instantiate(m_blockRightAfterPref, transform);
                            right.transform.position = new Vector3(prevBlockPos.x - m_offsetBlock.z - m_offsetTurnBlock, right.transform.position.y, prevBlockPos.z);

                            block.transform.position = new Vector3(
                                prevBlockPos.x - m_offsetBlock.z - m_offsetTurnBlock, prevBlockPos.y, prevBlockPos.z + m_offsetBlock.z + m_offsetTurnBlock);
                        }

                        prevTurn = prevTurn == TurnType.NONE ? TurnType.RIGHT : TurnType.NONE;

                        m_objects.Add(right);
                        m_objects.Add(block);

                        m_player.AddDestination(right.transform.position);
                    }
                }
            }
        }
    }
}
