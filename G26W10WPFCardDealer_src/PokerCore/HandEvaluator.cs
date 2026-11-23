namespace PokerCore;

public enum HandRank
{
    HighCard, OnePair, TwoPair, ThreeKind, Straight, Flush,
    FullHouse, FourKind, StraightFlush, RoyalFlush
}

public static class HandEvaluator
{
    private const int FourCnt = 4;
    private const int ThreeCnt = 3;
    private const int PairCnt = 2;
    private const int StraightSpan = Deck.HandSize - 1;

    private static readonly HashSet<Rank> Wheel =
        [Rank.Ace, Rank.Two, Rank.Three, Rank.Four, Rank.Five];

    public static HandRank Evaluate(IReadOnlyList<Card> hand)
    {
        Validate(hand);
        bool flush = IsFlush(hand);
        bool straight = IsStraight(hand);
        if (flush && straight)
            return IsRoyal(hand) ? HandRank.RoyalFlush : HandRank.StraightFlush;

        int[] cnt = CountPattern(hand);
        int top = cnt[0];
        int second = cnt.Length > 1 ? cnt[1] : 0;

        if (top == FourCnt) return HandRank.FourKind;
        if (top == ThreeCnt && second == PairCnt) return HandRank.FullHouse;
        if (flush) return HandRank.Flush;
        if (straight) return HandRank.Straight;
        if (top == ThreeCnt) return HandRank.ThreeKind;
        if (top == PairCnt)
            return second == PairCnt ? HandRank.TwoPair : HandRank.OnePair;
        return HandRank.HighCard;
    }

    private static void Validate(IReadOnlyList<Card> hand)
    {
        if (hand.Count != Deck.HandSize)
            throw new ArgumentException($"hand must have {Deck.HandSize} cards");
        if (hand.Distinct().Count() != Deck.HandSize)
            throw new ArgumentException("duplicate cards in hand");
    }

    private static int[] CountPattern(IReadOnlyList<Card> hand) =>
        hand.GroupBy(c => c.Rank)
            .Select(g => g.Count())
            .OrderByDescending(n => n)
            .ToArray();

    private static bool IsFlush(IReadOnlyList<Card> hand) =>
        hand.Select(c => c.Suit).Distinct().Count() == 1;

    private static bool IsStraight(IReadOnlyList<Card> hand)
    {
        var ranks = hand.Select(c => c.Rank).ToHashSet();
        if (ranks.SetEquals(Wheel)) return true;
        return ranks.Count == Deck.HandSize
            && ranks.Max() - ranks.Min() == StraightSpan;
    }

    private static bool IsRoyal(IReadOnlyList<Card> hand) =>
        hand.Min(c => c.Rank) == Rank.Ten;
}
