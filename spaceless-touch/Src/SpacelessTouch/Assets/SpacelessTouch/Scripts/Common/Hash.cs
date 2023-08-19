using UnityEngine;

namespace SpacelessTouch.Scripts.Common
{
    public static class AnimatorHash
    {
        public static readonly int Forward = Animator.StringToHash("Forward");
        public static readonly int Fall = Animator.StringToHash("Fall");
        public static readonly int Land = Animator.StringToHash("Land");
        public static readonly int Jump = Animator.StringToHash("Jump");
    }

    // public static class ShaderHash
    // {
    //     public static readonly int Value = Shader.PropertyToID("_Value");
    // }
}
