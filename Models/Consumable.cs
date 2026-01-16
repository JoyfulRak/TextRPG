namespace TextRPG.Models;
using System;

public class Consumable : Item
{
    #region 프로퍼티
    //HP회복량
    public int HpAmount { get; private set; }
    
    //MP회복량
    public int MpAmount { get; private set; }
    #endregion
    
    #region 생성자
    public Consumable(
        string name, 
        string description, 
        int price, 
        int hpAmount=0, 
        int mpAmount=0 ) 
        : base(name, description, price, ItemType.Potion)
    {
        HpAmount = hpAmount;
        MpAmount = mpAmount;
    }
    #endregion

    #region 메서드
    public override bool Use(Player player)
    {
        //플레이어의 HP/MP 회복하는 로직
        bool isUsed = false;
        //HP회복
        
        if (HpAmount > 0)
        {
            int healedHp=player.HealHP(HpAmount);
            if (healedHp > 0)
            {
                Console.WriteLine($"{player.Name} 의 체력이 {HpAmount} 만큼 회복되었습니다.");
                isUsed = true;
            }
            else
            {
                Console.WriteLine($"{player.Name} 의 체력이 이미 최대입니다.");
            }
        }
        //MP회복 
        if (MpAmount > 0)
        {
            int healedMp=player.HealMP(MpAmount);
            if (healedMp > 0)
            {
                Console.WriteLine($"{player.Name} 의  {MpAmount} 만큼 회복되었습니다.");
                isUsed = true;
            }
            else
            {
                Console.WriteLine($"{player.Name} 의 마나가 이미 최대입니다.");
            }
        }
        
        
        return isUsed;
    }
    #endregion

    #region 포션 생성 메서드

    public static Consumable CreatePotion(string potionType) => potionType switch
    {

        "체력포션" => new Consumable("체력포션", "체력을 100 회복합니다.", 50, hpAmount: 50),
        "대형체력포션"=> new Consumable("대형체력포션", "체력을 200 회복합니다.", 100, hpAmount: 100),
        "마나포션" => new Consumable("마나포션", "마나를 50 회복합니다.", 50, mpAmount: 50),
        "대형마나포션" => new Consumable("대형마나포션", "마나를 200 회복합니다.", 100, mpAmount: 100),
        _ => null!
       
    };

    #endregion
}
