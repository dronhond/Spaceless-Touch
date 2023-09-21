using System.Linq;
using JKFrame;
using MirMirror;
using SpacelessTouch.Scripts.Manager;
using SpacelessTouch.Scripts.Model;
using Unity.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SpacelessTouch.Scripts.Common
{
    public class PlayerController : SingletonMono<PlayerController>
    {
        [SerializeField] private Rigidbody2D r2;

        [Header("速度因子")] [Range(0.01f, 10)] [SerializeField]
        private float moveSpeed;

        [Header("跳跃因子")] [Range(0.1f, 20f)] [SerializeField]
        private float jumpForce;

        private RaycastHit2D[] _groundDetector;

        [Header("站立射线检测距离")] [Range(0.05f, 1)] [SerializeField]
        private float groundDetectorRayDis;

        [Header("站立检测射线数量")] [Range(1, 10)] [SerializeField] private int groundDetectorRayCnt;
        [SerializeField] private float groundRayLength;
        [SerializeField] private float groundNowLength;
        
        [Header("当前着陆状态")] [ReadOnly] [SerializeField]
		private ElementType elementType;
        private float _moveMultiple;
		private bool isContraryGravity = false; // 是否反重力
		private bool IsContraryGravity{
			get{ return isContraryGravity;}
			set{
				if (isContraryGravity == value) return;
				else {
					isContraryGravity = value;
					if (isContraryGravity) transform.rotation = Quaternion.Euler(new Vector3(0f, 180f, 180f));
					else transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
				}
				
			}
		}

        private void Start()
        {
#if UNITY_EDITOR
            int level;
            try
            {
                level = int.Parse(SceneManager.GetActiveScene().name[5..]);
            }
            catch
            {
                level = 1;
            }

            DataManager.Instance.CurrentLevel = level;
            DataManager.Instance.DataInfo ??= new DataInfo(level);
            GameScreen.FullScreen.Instance.SetActiveMainBtn(true);
#endif
            GameManager.isGaming = true;
            groundNowLength = groundRayLength;
            GameScreen.FullScreen.Instance.UpdateShowLevel();
        }

        private void Update()
        {
            if (!GameManager.isGaming) return;
            //反重力检测
			if(r2.gravityScale >= 0) IsContraryGravity = false;
			else IsContraryGravity = true;
            //角色跳跃
			if (elementType != ElementType.None && Input.GetButtonDown("Jump"))
			{
				if(r2.gravityScale >= 0)
				{
					r2.velocity = new Vector2(r2.velocity.x, jumpForce);
				}
				else 
				{	
					r2.velocity = new Vector2(r2.velocity.x, jumpForce * -1); // 反重力跳跃
				}
			}
            GroundDetector();
        }

        private void FixedUpdate()
        {
            if (!GameManager.isGaming) return;
            Move();
        }

        private void Move()
        {
            _moveMultiple = Input.GetAxisRaw("Horizontal");
            //角色左右移动
            if (_moveMultiple != 0)
            {
                transform.Translate(new Vector2(_moveMultiple * moveSpeed * Time.fixedDeltaTime, 0));
            }

            //角色朝向修改
            if (_moveMultiple != 0) transform.localScale = new Vector2(_moveMultiple, 1);
        }

        /// <summary>
        /// 玩家接触地面判断
        /// </summary>
        private void GroundDetector()
        {
			if(!isContraryGravity){
				_groundDetector = new RaycastHit2D[groundDetectorRayCnt];
				_groundDetector = MirMirrorTools.Hits(transform.position, Vector3.down, groundNowLength,
					~LayerMask.GetMask("Ignore Raycast"), Color.green, groundDetectorRayCnt, groundDetectorRayDis);
				var onGround = _groundDetector.Any(raycast => raycast.collider != null);
				elementType = onGround ? ElementType.Ground : ElementType.None;
			}
			else{ // 反重力情况
				_groundDetector = new RaycastHit2D[groundDetectorRayCnt];
				_groundDetector = MirMirrorTools.Hits(transform.position, Vector3.up, groundNowLength,
					~LayerMask.GetMask("Ignore Raycast"), Color.green, groundDetectorRayCnt, groundDetectorRayDis);
				var onGround = _groundDetector.Any(raycast => raycast.collider != null);
				elementType = onGround ? ElementType.Ground : ElementType.None;
			}
		}
    }
}