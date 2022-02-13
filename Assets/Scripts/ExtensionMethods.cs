using System.Collections.Generic;
using UnityEngine;

public static class ExtensionMethods
{
    public static CardSuit DetermineCardSuit(string cardName)
    {
        //Covert to upper case to make handling the text easier
        string upperName = cardName.ToUpper();

        if (upperName.Contains("DIAMOND"))
            return CardSuit.Diamond;
        else if (upperName.Contains("SPADE"))
            return CardSuit.Spade;
        else if (upperName.Contains("CLUB"))
            return CardSuit.Club;
        else if (upperName.Contains("HEART"))
            return CardSuit.Heart;
        else
        {
            Debug.LogWarning("Could not determine card suit for " + cardName);
            return CardSuit.Undefined;
        }
    }

    public static int DetermineCardNumber(string cardName)
    {
        //Covert to upper case to make handling the text easier
        string upperName = cardName.ToUpper();

        if ((upperName.Contains("1") || upperName.Contains("ACE")) && !upperName.Contains("10") && !upperName.Contains("11") && !upperName.Contains("12") && !upperName.Contains("13"))
            return 1;
        else if (upperName.Contains("2"))
            return 2;
        else if (upperName.Contains("3"))
            return 3;
        else if (upperName.Contains("4"))
            return 4;
        else if (upperName.Contains("5"))
            return 5;
        else if (upperName.Contains("6"))
            return 6;
        else if (upperName.Contains("7"))
            return 7;
        else if (upperName.Contains("8"))
            return 8;
        else if (upperName.Contains("9"))
            return 9;
        else if (upperName.Contains("10"))
            return 10;
        else if (upperName.Contains("11") || upperName.Contains("JACK"))
            return 11;
        else if (upperName.Contains("12") || upperName.Contains("QUEEN"))
            return 12;
        else if (upperName.Contains("13") || upperName.Contains("KING"))
            return 13;
        else
        {
            Debug.LogWarning("Could not find matching card number for " + cardName + ". Setting to 0");
            return 0;
        }
    }

    public static void Shuffle<T>(this IList<T> list)
    {
        System.Random rnd = new System.Random();

        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rnd.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}
