using SpacelessTouch.Scripts.Common;
using UnityEngine;

namespace MirMirror
{
    public class TouchButtom : MonoBehaviour
    {
        public Door m_Door;
        public Sprite m_Up;
        public Sprite m_Down;

        public void OnTriggerStay2D(Collider2D col)
        {
            if (col.CompareTag("Box") || col.CompareTag("Player"))
            {
                m_Door.Open();
            }

            this.GetComponent<SpriteRenderer>().sprite = m_Down;
        }

        public void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Box") || other.CompareTag("Player"))
            {
                m_Door.Close();
            }

            this.GetComponent<SpriteRenderer>().sprite = m_Up;
        }
    }
}

