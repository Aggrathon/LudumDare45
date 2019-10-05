
using UnityEngine;
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
}
