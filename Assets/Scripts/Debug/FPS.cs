using UnityEngine;
using UnityEngine.UI;

namespace Jelly
{
    public class FPS : MonoBehaviour
    {
        Text m_text;

        private void Awake()
        {
            m_text = GetComponent<Text>();
        }

        void Update()
        {
            m_text.text = "";
            m_text.text += "FPS: " + (int)(1f / Time.unscaledDeltaTime);
        }
    }
}
