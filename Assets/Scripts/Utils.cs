
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class Utils
{

    public static void ShuffleList<A>(List<A> list)
    {
        for (int i = 0; i < list.Count - 1; i++)
        {
            SwapList(list, i, UnityEngine.Random.Range(i, list.Count));
        }
    }

    public static void SwapList<A>(List<A> list, int a, int b)
    {
        if (a != b)
        {
            var tmp = list[a];
            list[a] = list[b];
            list[b] = tmp;
        }
    }
    
    public static Vector3 GetGroundIntersect(Ray ray, float height) {
        if (height > ray.origin.y)
            return ray.origin;
        height =  - (ray.origin.y - height) / ray.direction.y;
        return ray.origin + ray.direction * height;
    }

    public static IEnumerator LerpMoveTo(Transform mover, Vector3 position, float time=0.5f) {
        Vector3 vel = Vector3.zero;
        while ((mover.position - position).sqrMagnitude > 0.01f) {
            mover.position = Vector3.SmoothDamp(mover.position, position, ref vel, time);
            yield return null;
        }
        mover.position = position;
    }
}
