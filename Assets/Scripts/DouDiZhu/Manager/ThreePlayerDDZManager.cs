using System.Collections.Generic;
using System;
using System.Text;

public class CardPlayer
{
    public bool autuPlay = true;  //是否自动玩耍
    public bool ready = false; //是否准备好了

    private List<int> cardsInHand;

    public CardPlayer(int[] cards)
    {
        cardsInHand = new List<int>(cards);
        cardsInHand.Sort();
    }

    //出牌
    public void ShowCards()
    {

    }
}

//负责
public class ThreePlayerDDZ
{
    int[] cards;
    public ThreePlayerDDZ()
    {
        //0:3 1:4 2:5 3:6 ...7:10 8:J 9:Q 10:K 11:A 12:2  52小王  53大王
        //0,1,2,3 方块、梅花、红桃、黑桃  AAAA和四种花色  除4余数为花色    
        cards = new int[54];
        for(int i=0;i<54;i++)
        {
            cards[i]=i;
        }
        GlobalUtils.Shuffle(cards);
    }

    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        for(int i=0;i<cards.Length;i++)
        {
            sb.Append(cards[i]).Append(',');
        }
        return sb.ToString();
    }
}

//模拟服务器，处理客户端的请求
public class ThreePlayerDDZManager
{
    public static AHandOfCards CreateAHandOfCards(int[] cards)
    {
        return new AHandOfCards(cards);
    }
    public static int CompareCards(int tgtCard,int yourCard)
    {
        return 0;
    }

    //比较出牌的大小
    //1表示大，0表示相等，-1表示小于  -2表示非法
    public static int CompareCards(int[] tgtCards,int[] yourCards)
    {
        AHandOfCards tgtHand = CreateAHandOfCards(tgtCards);
        AHandOfCards yourHand = CreateAHandOfCards(yourCards);
        
        if(yourHand.CardsType==CardsTypeEnum.KING_BOMB){
            return 1;
        }
        if(tgtHand.CardsType==CardsTypeEnum.KING_BOMB){
            return -1;
        }
        if(yourHand.CardsType==CardsTypeEnum.BOMB){
            if(tgtHand.CardsType==CardsTypeEnum.BOMB){
                return CompareCards(tgtCards[0],yourCards[0]);
            }
            return 1;
        }
        else{
            if(tgtHand.CardsType==CardsTypeEnum.BOMB){
                return -1;
            }
            if(tgtHand.CardsType==yourHand.CardsType && tgtCards.Length==yourCards.Length){
                return CompareCards(tgtCards[0],yourCards[0]);
            }
        }
        return -2;
    }
    

    //初始化一副牌 
    //洗牌
    //发牌给3个人
    //展示牌
    //出牌 刷新
    //比大小

}
