using SpacelessTouch.Scripts.Manager;
using UnityEngine;


namespace SpacelessTouch.Scripts.Common
{
    /// <summary>
    /// <para>传送方块</para>
    /// </summary>
    public class DeliverBox : Box 
    {
        /// <summary>
        /// <para>传入角色游戏对象</para>
        /// </summary>
        public GameObject Player; //传送对象
        [SerializeField] private bool IsEnterDoor; //是否靠近门/墙
        [SerializeField] private bool HaveSomeone; //是否
        private Vector3 upPosition;

        /// <summary>
        /// <para>终点周围物体判断</para>
        /// </summary>
        /// <param name="col">控制对象，这里是传送终点对象的周围物体判断</param>
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
        /// <summary>
        /// <para>自定义方块颜色，这里是传送方块自身颜色</para>
        /// </summary>
        private void ColorChanged()
        {
            sr.color = ColorType == BoxColorType.白色 ? Color.white : Color.red;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type">游戏场景类有写染色的设计，这里是改变颜色接口</param>
        public new void ChangeColor(BoxColorType type)
        {
            if (ColorType != type) GameManager.Instance.Disconnect(this, null);
            ColorType = type;
            ColorChanged();
        }

        /// <summary>
        /// <para>传送主体逻辑，当靠近时按F即可传送</para>
        /// </summary>
        public void FixedUpdate()
        {
            if (Link != null)
            {
                if (IsEnterDoor && Input.GetKeyDown(KeyCode.F))
                {
                    if (HaveSomeone)
                    {
                        var position = Link.transform.position;
                        upPosition = new Vector3(position.x, position.y + 2, position.z); //定义终点坐标
                        Player.transform.position = upPosition;
                    }
                    else
                    {
                        Player.transform.position = Link.transform.position;
                    }
                }
            }
        }
        /// <summary>
        /// <para>判断是否传送过去了</para>
        /// </summary>
        /// <param name="col">当前触碰的物体</param>

        private void OnCollisionExit2D(Collision2D col)
        {
            if (col.gameObject == Player)
            {
                IsEnterDoor = false;
            }
        }
    }

}