using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jelly
{
    public class VfxManager : MonoBehaviour
    {
        [System.Serializable]
        public enum VfxType
        {
            COLLISION_ENEMY,
            COLLISION_DIAMOND,
            FINISH_1,
            FINISH_2
        }

        [System.Serializable]
        public class Vfx
        {
            public VfxType m_type;
            public GameObject m_prefab;
        }

        [SerializeField] private List<Vfx> m_vfxs;

        void Awake()
        {

        }

        public void Play(VfxType type, Vector3 pos = new Vector3(), Transform transform = null)
        {
            GameObject tmpPrefab = null;
            for (int i = 0; i < m_vfxs.Count; ++i)
            {
                if (type == m_vfxs[i].m_type)
                    tmpPrefab = m_vfxs[i].m_prefab;
            }

            if (tmpPrefab)
            {
                GameObject vfx = Instantiate(tmpPrefab, transform);
                vfx.transform.position = pos;
            }
        }
    }
}
