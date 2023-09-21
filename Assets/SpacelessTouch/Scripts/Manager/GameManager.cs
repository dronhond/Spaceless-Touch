using JKFrame;
using MirMirror;
using SpacelessTouch.Scripts.Common;
using SpacelessTouch.Scripts.Controller;
using SpacelessTouch.Scripts.GameScreen;
using SpacelessTouch.Scripts.System;
using UnityEngine;

namespace SpacelessTouch.Scripts.Manager
{
    public class GameManager : SingletonMono<GameManager>
    {
        [SerializeField] private LineRenderer lineRender;
        private Box.BoxPair pair;
        [Header("箱子连接1")] public Box ClickBox1;
        [Header("箱子连接2")] public Box ClickBox2;

        public static bool isGaming;

        public void Init()
        {
            GameSystem.PlayBGM("LOOP_Let's Go Underwater!.wav");
            FullScreen.Instance.SetActiveMainBtn(true);
        }

        public void Close()
        {
            isGaming = false;
            AudioSystem.StopBGAudio();
            FullScreen.Instance.SetActiveMainBtn(false);
        }
        
        public void Retry()
        {
            Reset();
            GameSystem.ChangeLevel(DataManager.Instance.CurrentLevel);
        }

        public void Reset()
        {
            pair = null;
        }

        public void Update()
        {
            if (!isGaming) return;
            DrawLink();
            if (Input.GetMouseButtonDown(1))
            {
                ClickBox1 = null;
            }
        }

        private void DrawLink()
        {
            Vector3[] points;
            if (ClickBox1 != null && ClickBox2 == null)
            {
                var mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0); //让鼠标的屏幕坐标与对象坐标一致
                points = new Vector3[2];
                points[0] = ClickBox1.transform.position;
                points[1] = CameraController.main.ScreenToWorldPoint(mousePos);
                points[1] = new Vector3(points[1].x, points[1].y, 0);
                lineRender.positionCount = 2;
                lineRender.SetPositions(points);
            }
            else
            {
                lineRender.positionCount = 0;
                //lineRender.SetPositions(points);
            }

            if (pair == null || pair.Box1.ColorType != pair.Box2.ColorType) return;
            points = new Vector3[2];
            points[0] = pair.Box1.transform.position + Vector3.forward * -1;
            points[1] = pair.Box2.transform.position + Vector3.forward * -1;
            pair.LineRender.positionCount = 2;
            pair.LineRender.SetPositions(points);
        }

        public void AddPair(Box box1, Box box2)
        {
            Disconnect(box1, box2);
            box1.Link = box2;
            box2.Link = box1;
            var boxPair = new Box.BoxPair
            {
                Box1 = box1,
                Box2 = box2,
                LineRender = Instantiate(lineRender)
            };
            pair = boxPair;
            ClickBox1 = null;
            ClickBox2 = null;
            GameSystem.PlaySound("GetSBSFX.wav");
        }

        public void Disconnect(Box box1, Box box2)
        {
            if (pair == null) return;
            GameSystem.PlaySound("GetMainWeaponSFX.wav");
            pair.Box1.transform.parent = null;
            pair.Box2.transform.parent = null;
            pair.Box1.Link = null;
            pair.Box2.Link = null;
            var temp = pair.LineRender;
            pair.LineRender = null;
            Destroy(temp.gameObject);
            pair = null;
        }
    }
}