using JKFrame;
using Sirenix.OdinInspector;
using UnityEngine;

namespace SpacelessTouch.Scripts.Common
{
    public class Paint : MonoBehaviour
    {
        [OnValueChanged("ColorChanged")] public BoxColorType ColorType;
        [SerializeField] private SpriteRenderer sr;
        [SerializeField] private AudioClip touchAudioClip;

        private void ColorChanged()
        {
            sr.color = ColorType == BoxColorType.红色 ? Color.red : Color.yellow;
        }

        public void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Box")) return;
            AudioSystem.PlayOneShot(touchAudioClip);
            other.GetComponent<Box>().ChangeColor(ColorType);
        }
    }
}