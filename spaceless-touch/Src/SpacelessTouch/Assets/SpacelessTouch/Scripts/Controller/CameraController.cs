using Cinemachine;
using SpacelessTouch.Scripts.Common;
using UnityEngine;

namespace SpacelessTouch.Scripts.Controller
{
    public class CameraController : MonoBehaviour
    {
        public static CameraController Instance;
        public static Camera main;
        public CinemachineVirtualCamera cinemachine;
        public CinemachineConfiner cinemachineConfiner;

        private void Awake()
        {
            Instance = this;
            main = GetComponent<Camera>();
        }

        // private void Start()
        // {
        //     if (PlayerController.Instance != null)
        //         cinemachine.Follow = PlayerController.Instance.transform;
        // }

        public void SetFollowPlayer(bool isFollowPlayer) //创建角色以后让摄像机跟随的方法
        {
            //CM相机直接给follow
            cinemachine.Follow = isFollowPlayer ? PlayerController.Instance.transform : null;
        }

        public void InitCMvcam1(GameObject CMvcam1)
        {
            cinemachine = CMvcam1.GetComponent<CinemachineVirtualCamera>();
            cinemachineConfiner = CMvcam1.GetComponent<CinemachineConfiner>();
        }

        public void SetFollowTarget(Transform target)
        {
            cinemachine.Follow = target;
        }

        public void CameraLimit(PolygonCollider2D mapConfine)
        {
            cinemachineConfiner.m_BoundingShape2D = mapConfine;
        }
    }
}