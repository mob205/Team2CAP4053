using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Helpers
{
    public static bool IsInMask(LayerMask mask, int layer)
    {
        return (mask & (1 << layer)) != 0;
    }
}
