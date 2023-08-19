using SpacelessTouch.Scripts.Manager;
using UnityEngine;


namespace SpacelessTouch.Scripts.Common
{
    public class DeliverBox : Box 
    {
        public GameObject Player;
        [SerializeField] private bool IsEnterDoor;
        [SerializeField] private bool HaveSomeone;
        private Vector3 upPosition;

        private void OnCollisionEnter2D (Collision2D col)
        {
            if (col.gameObject == Player)
            {
                IsEnterDoor = true;
            }
            else
            {
                HaveSomeone = Mathf.Abs(col.gameObject.transform.position.x - transform.position.x) < 2;
            }
        }
        
        //定义颜色
        private void ColorChanged()
        {
            sr.color = ColorType == BoxColorType.白色 ? Color.white : Color.red;
        }
        
        public new void ChangeColor(BoxColorType type)
        {
            if (ColorType != type) GameManager.Instance.Disconnect(this, null);
            ColorType = type;
            ColorChanged();
        }

        public void FixedUpdate()
        {
            if (Link != null)
            {
                if (IsEnterDoor && Input.GetKeyDown(KeyCode.F))
                {
                    if (HaveSomeone)
                    {
                        var position = Link.transform.position;
                        upPosition = new Vector3(position.x, position.y + 2, position.z);
                        Player.transform.position = upPosition;
                    }
                    else
                    {
                        Player.transform.position = Link.transform.position;
                    }
                }
            }
        }

        private void OnCollisionExit2D(Collision2D col)
        {
            if (col.gameObject == Player)
            {
                IsEnterDoor = false;
            }
        }
    }

}