using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GlobalUtils
{
    //洗牌
    public static void Shuffle(int[] cards)
    {
        int n = cards.Length;
        while(n>0)
        {
            int index = UnityEngine.Random.Range(0,n);
            int temp = cards[n-1];
            cards[n-1] = cards[index];
            cards[index] = temp;
            n--;
        }
    }
}
