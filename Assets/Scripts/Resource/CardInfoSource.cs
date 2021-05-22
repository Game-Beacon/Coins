using System.Collections.Generic;
using Tools;

public static class CardInfoSource
{
    public static List<Card> cards { get; private set; }
    public static void Intialize()
    {
        cards= FileHandler.ReadCardsFile();
    }
}