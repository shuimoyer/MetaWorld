using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

//表示出牌阶段可以出的一手牌, 需要对象池来减少GC
public class AHandOfCards
{
    public CardsTypeEnum CardsType { get; }
    private CardsTypeEnum mCardsEnum = CardsTypeEnum.NONE;
    
    static CardOperator op = new CardOperator();

    public AHandOfCards(int[] cards)
    {
        
    }

    private bool HasKingCard(int[] cards){
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
    private CardsTypeEnum CheckCardsTYPE(int[] cards,bool order=true)
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
                    // else if(){//114
                    //     
                    // }
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
}
