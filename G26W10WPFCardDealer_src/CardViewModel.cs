using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PokerCore;
using System.Collections.ObjectModel;

namespace G26W10WPFCardDealer;

public partial class CardViewModel(
    CardModel model
) : ObservableObject {
    private const string Joker = "Images/black_joker.png";
    private const string FaceSuffix = "2";
    private const string Hint = "카드 분배 버튼을 누르세요";

    [ObservableProperty]
    private ObservableCollection<string> _cardResource = [..
        Enumerable.Repeat(Joker, CardModel.NumCards)
    ];

    [ObservableProperty]
    private string _rankText = Hint;

    [RelayCommand]
    public void DealCards() {
        model.DealCards();
        ShowCards();
        ShowRank();
    }

    private void ShowCards() {
        for (int i = 0; i < model.Cards.Count; i++) {
            CardResource[i] = ImgPath(model.Cards[i]);
        }
    }

    private void ShowRank() {
        if (model.Result is HandRank r)
            RankText = HandText(r);
    }

    // 이미지 파일명 규칙은 화면 쪽 사정이라 Core가 아닌 여기에 둠
    private static string ImgPath(Card c) {
        string suit = SuitName(c.Suit);
        if (c.Rank is Rank.Jack or Rank.Queen or Rank.King)
            suit += FaceSuffix;
        return $"Images/{RankName(c.Rank)}_of_{suit}.png";
    }

    private static string SuitName(Suit s) => s switch {
        Suit.Spade => "spades",
        Suit.Heart => "hearts",
        Suit.Diamond => "diamonds",
        Suit.Club => "clubs",
        _ => throw new ArgumentOutOfRangeException(nameof(s)),
    };

    private static string RankName(Rank r) => r switch {
        Rank.Ace => "ace",
        Rank.Jack => "jack",
        Rank.Queen => "queen",
        Rank.King => "king",
        _ => ((int)r).ToString(),
    };

    private static string HandText(HandRank r) => r switch {
        HandRank.RoyalFlush => "로열 스트레이트 플러시",
        HandRank.StraightFlush => "스트레이트 플러시",
        HandRank.FourKind => "포카드",
        HandRank.FullHouse => "풀하우스",
        HandRank.Flush => "플러시",
        HandRank.Straight => "스트레이트",
        HandRank.ThreeKind => "트리플",
        HandRank.TwoPair => "투페어",
        HandRank.OnePair => "원페어",
        _ => "노페어(하이카드)",
    };
}
