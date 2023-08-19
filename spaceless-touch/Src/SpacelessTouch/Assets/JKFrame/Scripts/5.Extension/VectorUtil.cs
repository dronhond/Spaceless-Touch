using UnityEngine;

public static class VectorUtil
{
    /// <summary>
    /// 返回平面向量在坐标系中与Y轴夹角(正负)
    /// </summary>
    public static float GetAngleInCoordinateSystem(this Vector2 Vec)
    {
        float angle;
        angle = Vector2.Angle(new Vector2(0, 1), Vec);
        if (Vec.x < 0)
        {
            angle = -angle;
        }
        return angle;
    }
}
