using UnityEngine;

namespace MirMirror
{
    public static class MirMirrorTools
    {
        public static RaycastHit2D RayCast(Vector2 rayOriginPoint, Vector2 rayDirection, float rayDistance,
            LayerMask mask, Color color, bool drawGizmo = true)
        {
            if (drawGizmo)
            {
                Debug.DrawRay(rayOriginPoint, rayDirection * rayDistance, color);
            }

            return Physics2D.Raycast(rayOriginPoint, rayDirection, rayDistance, mask);
        }

        public static RaycastHit2D[] Hits(Vector3 start, Vector3 direction, float length, int mask, Color color,
            int cnt = 1, float rayDis = 0.1f)
        {
            float l = (cnt - 1) * rayDis;
            float startOffset = l / 2;
            Vector3 cross = Vector3.Cross(Vector3.forward, direction);
            RaycastHit2D[] hits = new RaycastHit2D[cnt];
            for (int i = 0; i < cnt; i++)
            {
                hits[i] = RayCast(start + cross * (startOffset - i * rayDis), direction, length, mask, color);
            }

            return hits;
        }

        public static bool CheckState(Animator anim, string stateName, string layerName) //���״̬������״̬
        {
            return anim.GetCurrentAnimatorStateInfo(anim.GetLayerIndex(layerName)).IsName(stateName);
        }

        public static bool CheckStateTag(Animator anim, string tagName, string layerName)
        {
            return anim.GetCurrentAnimatorStateInfo(anim.GetLayerIndex(layerName)).IsTag(tagName);
        }
    }
}