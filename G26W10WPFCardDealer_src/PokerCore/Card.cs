namespace PokerCore;

public enum Suit { Spade, Heart, Diamond, Club }

public enum Rank
{
    Two = 2, Three, Four, Five, Six, Seven, Eight, Nine, Ten,
    Jack, Queen, King, Ace
}

public readonly record struct Card(Suit Suit, Rank Rank)
{
    public override string ToString() => $"{Suit}-{Rank}";
}
