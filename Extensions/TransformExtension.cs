using UnityEngine;

namespace Extensions;

public static class TransformExtension
{
    public static Transform FindChildByName(this Transform transform, string name)
    {
        return Utils.FindChild(transform, name);
    }
}