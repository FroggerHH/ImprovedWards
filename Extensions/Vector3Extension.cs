using UnityEngine;

namespace Extensions;

public static class Vector3Extension
{
    public static Vector3 RoundCords(this Vector3 vector3)
    {
        return new Vector3((int)vector3.x, (int)vector3.y, (int)vector3.z);
    }
}