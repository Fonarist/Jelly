
using UnityEngine;
using UnityEngine.UI;

namespace Jelly
{
    public class FloatingText : MonoBehaviour
    {
        [SerializeField] private Animator m_animator;
        [SerializeField] private Text m_text;

        void OnEnable()
        {
            AnimatorClipInfo[] animClips = m_animator.GetCurrentAnimatorClipInfo(0);
            Destroy(transform.gameObject, animClips[0].clip.length * 0.9f);
        }

        public void SetText(string text)
        {
            m_text.text = text;
        }
    }
}
