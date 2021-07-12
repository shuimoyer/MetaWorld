using System.Collections;
using System.Collections.Generic;
using System;
using System.Text;
using UnityEngine;

public enum CardsTypeEnum
{
    NONE, //非法
    SINGLE,  //单张         1
    DOUBLE, //对子          2
    KING_BOMB,//王炸        2
    TRIPLE, //三条          3
    TRIPLE_SINGLE, //三带一     4
    BOMB, //炸弹        4
    TRIPLE_DOUBLE, //三带二     5
    ORDER,//顺子 大于等于连续的五张   >=5
    BOMB_SINGLE,//四张+两个单张     6
    LINK_DOUBLE,//连对 三对及以上   2N  N>=3
    PLANE,// 飞机， 两个或以上的连续三张   3N  N>=2
    BOMB_DOUBLE,//四张+两个对子     8

    PLANE_WING_SINGLE,//飞机带翅膀，飞机加同等数量的单张  4N N>=2
    PLANE_WING_DOUBLE,//飞机带翅膀，飞机加同等数量的对子  5N N>=2
}

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
        ThreePlayerDDZManager.Shuffle(cards);
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
//不包括大小王
public class CardOperator{
    int[] tempArray = new int[13];  //index表示card，存储的值表示相同card的数量
    int[] sortedArray = new int[13]; //存储的值表示card，index越小表示 card重复数越大，card值越大
    public int Length = 0;
    public void Init(int[] cards){
        for(int i =0;i<tempArray.Length;i++){
            tempArray[i]=0;
        }
        for(int i=0;i<cards.Length;i++){
            tempArray[cards[i]/4] += 1;
        }
        for(int i =0;i<tempArray.Length;i++){
            if(tempArray[i]>0){Length++;}
        }
    }
    //lenth必须小于 Length   从1开始, you 
    public int GetCardByLength(int length){
        int curLen = 0;
        for(int i =0;i<tempArray.Length;i++){
            if(tempArray[i]>0){
                curLen++;
                if(curLen==length){
                    return i;
                }
            }
        }
        return -1;
    }
    public int GetCardCountByCard(int cardIndex){
        return tempArray[cardIndex];
    }
    public int GetCardCountByLength(int length){
        return GetCardCountByCard(GetCardByLength(length));
    }
}
//模拟服务器，处理客户端的请求
public class ThreePlayerDDZManager
{
    static CardOperator op = new CardOperator();

    public static bool HasKingCard(int[] cards){
        bool hasKing = false;
        for(int i=0;i<cards.Length;i++){
            if(cards[i]>51){
                hasKing = true;
                break;
            }
        }
        return hasKing;
    }

    //检测出的牌是什么类型,同时默认进行排序，便于后续做大小比较
    public static CardsTypeEnum CheckCardsTYPE(int[] cards,bool order=true)
    {
        if(order){
            Array.Sort(cards);
        }

        if(cards.Length==1){
            return CardsTypeEnum.SINGLE;
        }

        CardsTypeEnum type = CardsTypeEnum.NONE;

        if(HasKingCard(cards)){
            if(cards.Length==2){
                if(cards[0]>51 && cards[1]>51 && cards[0]!=cards[1]){
                    type = CardsTypeEnum.KING_BOMB;
                }
            }
        }
        else{
            op.Init(cards);
            if(op.Length==1){
                if(cards.Length==2){type = CardsTypeEnum.DOUBLE;}
                else if(cards.Length==3){type = CardsTypeEnum.TRIPLE;}
                else if(cards.Length==4){type = CardsTypeEnum.BOMB;}
            }
            //11 21 22 31 32 33 41 42 43 44  
            //合法的只有 31 32 33 42
            else if(op.Length==2){
                if(cards.Length==4){ //22 31
                    if(op.GetCardCountByLength(1)!=2){
                        type = CardsTypeEnum.TRIPLE_SINGLE;
                    }
                }
                else if(cards.Length==5){ //32 41
                    if(op.GetCardCountByLength(1)==3 || op.GetCardCountByLength(1)==2){
                        type = CardsTypeEnum.TRIPLE_DOUBLE;
                    }
                }
                else if(cards.Length==6){ //33 42
                    if(op.GetCardCountByLength(1)==3){
                        if(op.GetCardByLength(2)-op.GetCardByLength(1)==1){
                            type = CardsTypeEnum.PLANE;
                        }
                    }
                    else{
                        type = CardsTypeEnum.BOMB_SINGLE;
                    }
                }
            }
            //111 112 122 222 113 123 223 333 114 124 224 234...
            //合法的只有 222 333 114 224  
            else if(op.Length==3){
                if(cards.Length==6){ //222
                    if(op.GetCardCountByLength(1)==2 && op.GetCardCountByLength(2)==2 && op.GetCardCountByLength(3)==2){
                        if(op.GetCardByLength(2)-op.GetCardByLength(1)==1 && op.GetCardByLength(3)-op.GetCardByLength(2)==1){
                            type = CardsTypeEnum.LINK_DOUBLE;
                        }
                    }
                    else if(){//114

                    }
                }
                else if(cards.Length==8){

                }
                else if(cards.Length==9){

                }
            }
            else if(op.Length==4)
            {
                
            }   
        }
        return type;
    }

    public static int CompareCards(int tgtCard,int yourCard)
    {
        
        return 0;
    }

    //比较出牌的大小
    //1表示大，0表示相等，-1表示小于  -2表示非法
    public static int CompareCards(int[] tgtCards,int[] yourCards)
    {
        CardsTypeEnum tgtType = CheckCardsTYPE(tgtCards);
        CardsTypeEnum yourType = CheckCardsTYPE(yourCards);
        if(yourType==CardsTypeEnum.KING_BOMB){
            return 1;
        }
        if(tgtType==CardsTypeEnum.KING_BOMB){
            return -1;
        }
        if(yourType==CardsTypeEnum.BOMB){
            if(tgtType==CardsTypeEnum.BOMB){
                return CompareCards(tgtCards[0],yourCards[0]);
            }
            return 1;
        }
        else{
            if(tgtType==CardsTypeEnum.BOMB){
                return -1;
            }
            if(tgtType==yourType && tgtCards.Length==yourCards.Length){
                return CompareCards(tgtCards[0],yourCards[0]);
            }
        }
        return -2;
    }

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

    //初始化一副牌 
    //洗牌
    //发牌给3个人
    //展示牌
    //出牌 刷新
    //比大小

}
