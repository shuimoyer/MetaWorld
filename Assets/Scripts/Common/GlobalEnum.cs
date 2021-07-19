using System.Collections;
using System.Collections.Generic;
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
