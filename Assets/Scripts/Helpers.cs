using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Helpers
{
    public static bool IsInMask(LayerMask mask, int layer)
    {
        return (mask & (1 << layer)) != 0;
    }

    public static T GetClosest<T>(Transform src, List<T> dests, Func<T, bool> predicate) where T : MonoBehaviour
    {
        T best = null;
        float bestDist = Mathf.Infinity;

        foreach(var obj in dests)
        {
            var diff = (src.position - obj.transform.position).sqrMagnitude;

            if(diff < bestDist && predicate(obj))
            {
                best = obj;
                bestDist = diff;
            }
        }
        return best;
    }

    public static bool HasAny<T>(List<T> objs, Func<T, bool> predicate) where T : MonoBehaviour
    {
        foreach(var obj in objs)
        {
            if(predicate(obj)) { return true; }
        }
        return false;
    }
}
