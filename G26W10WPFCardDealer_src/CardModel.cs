using PokerCore;

namespace G26W10WPFCardDealer;

// PokerCore(Card/Deck/HandEvaluator)를 그대로 가져다 쓰는 얇은 Model.
public class CardModel {
    public const int NumCards = Deck.HandSize;

    public IReadOnlyList<Card> Cards { get; private set; } = [];
    public HandRank? Result { get; private set; }

    public void DealCards() {
        List<Card> hand = new Deck().Deal(NumCards);
        Cards = [.. hand.OrderBy(c => c.Rank).ThenBy(c => c.Suit)];
        Result = HandEvaluator.Evaluate(Cards);
    }
}
