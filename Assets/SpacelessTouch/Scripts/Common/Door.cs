using JKFrame;
using UnityEngine;

namespace SpacelessTouch.Scripts.Common
{
    public class Door : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer sr;
        [SerializeField] private Collider2D c2;
        [SerializeField] private Sprite spriteOpen;
        [SerializeField] private Sprite spriteClose;
        [SerializeField] private AudioClip openAudioClip;
        [SerializeField] private AudioClip closeAudioClip;
        
        [ContextMenu("Open")]
        public void Open()
        {
            if (c2.enabled)
            {
                c2.enabled = false;
                AudioSystem.PlayOneShot(openAudioClip);
            }
            sr.sprite = spriteOpen;
        }

        public void Close()
        {
            if (c2 == null) return;
            c2.enabled = true;
            sr.sprite = spriteClose;
            AudioSystem.PlayOneShot(closeAudioClip);
        }
    }

}
