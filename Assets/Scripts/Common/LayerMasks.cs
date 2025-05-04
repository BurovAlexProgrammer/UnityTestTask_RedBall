using UnityEngine;

namespace Common
{
    public static class LayerMasks
    {
        public static int Ground => LayerMask.NameToLayer("Ground");
        public static int Damage => LayerMask.NameToLayer("Damage");
        public static int Dead => LayerMask.NameToLayer("Dead");
        public static int Player => LayerMask.NameToLayer("Player");

    }
}