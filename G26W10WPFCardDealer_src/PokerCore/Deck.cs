namespace PokerCore;

public class Deck
{
    public const int HandSize = 5;

    private readonly List<Card> _cards = [];
    private readonly Random _rng;

    public Deck(Random? rng = null)
    {
        _rng = rng ?? new Random();
        Fill();
        Shuffle();
    }

    private void Fill()
    {
        foreach (Suit s in Enum.GetValues<Suit>())
            foreach (Rank r in Enum.GetValues<Rank>())
                _cards.Add(new Card(s, r));
    }

    private void Shuffle()
    {
        for (int i = _cards.Count - 1; i > 0; i--)
        {
            int j = _rng.Next(i + 1);
            (_cards[i], _cards[j]) = (_cards[j], _cards[i]);
        }
    }

    public List<Card> Deal(int n = HandSize)
    {
        if (n > _cards.Count)
            throw new InvalidOperationException("not enough cards");
        var hand = _cards.GetRange(0, n);
        _cards.RemoveRange(0, n);
        return hand;
    }
}
