using System;
using System.Linq;
using MirMirror;
using Sirenix.OdinInspector;
using SpacelessTouch.Scripts.Manager;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SpacelessTouch.Scripts.Common
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Box : MonoBehaviour, IPointerClickHandler
    {
        [OnValueChanged("BoxChanged")] 
        [SerializeField] private BoxType boxType;

        public class BoxPair
        {
            public LineRenderer LineRender;
            public Box Box1;
            public Box Box2;
        }

        [OnValueChanged("ColorChanged")] public BoxColorType ColorType;

        /// <summary>
        /// 被连接的另外一个箱子
        /// </summary>
        public Box Link;

        private RaycastHit2D[] _groundDetector;

        [Header("站立姿态射线")] [Range(0.05f, 1)] [SerializeField]
        private float m_GroundDetectorRayDis = 0.1f;

        [Range(1, 10)] public int m_GroundDetectorRayCnt = 1;
        public float m_GroundRayLength = 0.3f;
        [Header("当前着陆状态")] [SerializeField] private ElementType elementType;

        [SerializeField] public SpriteRenderer sr;
        [SerializeField] private Rigidbody2D r2;

        private bool _needToStop;
        [SerializeField] private bool isGrounded;
        public float v2PosX;

        private void Awake()
        {
            v2PosX = transform.position.x;
        }

        private void ColorChanged()
        {
            sr.color = ColorType == BoxColorType.黄色 ? Color.yellow : Color.red;
        }

        private void BoxChanged()
        {
            
        }

        private void Update()
        {
            var mPosX = transform.position.x;
            if (Link != null)
            {
                if (Math.Abs(transform.position.x - v2PosX) > 0.0001f && Link.ColorType == ColorType)
                {
                    var target = Link.transform;
                    var pos = target.position;

                    pos = new Vector2(mPosX - v2PosX + pos.x, pos.y);
                    v2PosX = mPosX;
                    Link.v2PosX = pos.x;
                    target.position = pos;
                }
				else if (Link.ColorType != ColorType){
					GameManager.Instance.Disconnect(Link, Link.Link);
				}
            }
            else if (Math.Abs(v2PosX - mPosX) > 0.0001f) v2PosX = transform.position.x;
            /* if (_needToStop)
            {
                r2.velocity =
                    Vector3.Lerp(r2.velocity,
                        Vector3.zero, 0.5f);
            }

            if (r2.velocity.x <= 0.01f)
            {
                r2.velocity = Vector2.zero;
                _needToStop = false;
            }
             */

            GroundDetector();
        }

        private void GroundDetector()
        {
            _groundDetector = new RaycastHit2D[m_GroundDetectorRayCnt];
            if(GetComponent<Rigidbody2D>().gravityScale>=0) 
				_groundDetector = MirMirrorTools.Hits(transform.position + Vector3.down * 0.51f, Vector3.down,
                m_GroundRayLength,
                LayerMask.GetMask("Ground"), Color.green, m_GroundDetectorRayCnt, m_GroundDetectorRayDis);
			else
                _groundDetector = MirMirrorTools.Hits(transform.position + Vector3.up * 0.51f, Vector3.up,
                m_GroundRayLength,
                LayerMask.GetMask("Ground"), Color.green, m_GroundDetectorRayCnt, m_GroundDetectorRayDis);				
            isGrounded = _groundDetector.Any(raycast => raycast.collider != null);
            if (isGrounded) elementType = ElementType.Ground;
            else
            {
				if (Link != null)
                {
                    /*m_Link.m_Link = null;*/
                    GameManager.Instance.Disconnect(Link, Link.Link);
                }

                Link = null;
                transform.parent = null;
                elementType = ElementType.None;
            }
        }

        public void Push()
        {
            var position = transform.position;
            var collider2DLeft = Physics2D.Raycast(position, Vector2.left, 0.6f, ~LayerMask.GetMask("TouchOrGround"))
                .collider;
            var collider2DRight = Physics2D.Raycast(position, Vector2.right, 0.6f, ~LayerMask.GetMask("TouchOrGround"))
                .collider;
            Debug.DrawLine(position,
                Physics2D.Raycast(position, Vector2.left, 0.6f, ~LayerMask.GetMask("TouchOrGround")).point, Color.red);
            Debug.DrawLine(position,
                Physics2D.Raycast(position, Vector2.right, 0.6f, ~LayerMask.GetMask("TouchOrGround")).point, Color.red);
            if (Link != null && !(collider2DLeft != null && collider2DLeft.CompareTag("Ground")) &&
                !(collider2DRight != null && collider2DRight.CompareTag("Ground")))
            {
                var target = PlayerController.Instance.transform;
                Link.transform.parent = target;
                transform.parent = target;
            }
            else
                transform.parent = PlayerController.Instance.transform;
        }

        public void ChangeColor(BoxColorType type)
        {
            if (ColorType != type) GameManager.Instance.Disconnect(this, null);
            ColorType = type;
            ColorChanged();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
			bool flg = false;
			if (LayBox.Instance != null) if (LayBox.Instance.InLayMode) flg = true; 
            if (eventData.button == PointerEventData.InputButton.Left && !flg)
            {
                if (GameManager.Instance.ClickBox1 != null && GameManager.Instance.ClickBox1 != this)
                {
                    if (GameManager.Instance.ClickBox1.ColorType == ColorType)
                    {
                        GameManager.Instance.ClickBox2 = this;
                        GameManager.Instance.AddPair(GameManager.Instance.ClickBox1,
                            GameManager.Instance.ClickBox2);
                    }
                    else
                        GameManager.Instance.ClickBox1 = null;
                }
                else if (GameManager.Instance.ClickBox1 == null) GameManager.Instance.ClickBox1 = this;
            }
            else if (eventData.button == PointerEventData.InputButton.Right)
                GameManager.Instance.Disconnect(this, null);
        }
    }
}