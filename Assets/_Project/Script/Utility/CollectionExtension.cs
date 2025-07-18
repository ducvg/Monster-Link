
using System;
using System.Collections.Generic;

public static class CollectionExtension
{
    private static Random random = new Random();

    public static T GetRandomElement<T>(this List<T> source)
    {
        int index = random.Next(source.Count);
        return source[index];
    }

    public static void Shuffle<T>(this List<T> list)  
{  
    int n = list.Count;  
    while (n > 1) {  
        n--;  
        int k = random.Next(n + 1);  
        T value = list[k];  
        list[k] = list[n];  
        list[n] = value;  
    }  
}
}
