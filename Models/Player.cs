namespace TextRPG.Models;
using System;


public class Player : Character
{
    #region 프로퍼티

    //직업
    

    public JobType Job{get; private set;}
    
    //골드
    public int Gold {get; private set;}
    //TODO:장착 무기
    public Equipment? EquippedWeapon {get; private set;}
    
    //TODO:장착 방어구
    public Equipment? EquippedArmor {get; private set;}
    
    #endregion

    #region 생성자

    public Player(string name, JobType job) : base(
        name:name, 
        maxHp:GetInitHp(job), 
        maxMp:GetInitMp(job), 
        attackPower:GetInitAttack(job), 
        defense:GetInitDefense(job), 
        level:1)
    {
        Job = job;
        Gold = 1000;
    }

    private static int GetInitHp(JobType job)
    {
        switch (job)
        {
            case JobType.Warrior: return 50;
            case JobType.Archer: return 100;
            case JobType.Wizard: return 80;
            default: return 100;
        }
    }

    private static int GetInitMp(JobType job)
    {
        switch (job)
        {
            case JobType.Warrior: return 30;
            case JobType.Archer: return 50;
            case JobType.Wizard: return 100;
            default: return 100;
        }
    }

    private static int GetInitAttack(JobType job) =>
        job switch
        {
            JobType.Warrior => 20,
            JobType.Archer => 30,
            JobType.Wizard => 40,
            _ => 20
        };

    private static int GetInitDefense(JobType job) =>
        job switch
        {
            JobType.Warrior => 15,
            JobType.Archer => 10,
            JobType.Wizard => 5,
            _ => 15

        };

    #endregion

    #region 메서드

    //플레이어 정보 출력(오버라이드)
    public override void DisplayInfo()
    {
        base.DisplayInfo(); //기본 정보 출력
        Console.WriteLine($"직업: {Job}");
        Console.WriteLine($"골드: {Gold}");
        Console.WriteLine("===================");
    }

    //기본공격메서드(override)
    public override int Attack(Character target)
    {
        //TODO:장착 무기에 따른 추가 데미지
        int attackDamage = AttackPower;
        return target.TakeDamage(attackDamage);

    }
    //스킬 공격(MP소모) : Player 전용 메소드
    
    public int SkillAttack(Character target)
    {
        int mpCost = 15;
        //스킬 공격=기본공격 1.5 데미지
        int totalDamage = AttackPower;
        totalDamage = (int)(totalDamage * 1.5f);
        
        //mp소모
        CurrentMp-=mpCost;
        
        //데미지 전달
        return target.TakeDamage(totalDamage);
    }

    //골드 획득 메서드
    public void GainGold(int amount)
    {
        Gold+=amount;
        Console.WriteLine($"골드 + {amount} 획득! 현재 골드: {Gold}");
    }
    
    //장비 착용
    public void EquipItem(Equipment newEquipment)
    {
        Equipment? prevEquipment = null;
        switch (newEquipment.Slot)
        {
            case EquipmentSlot.Weapon:
                prevEquipment = EquippedWeapon;
                EquippedWeapon = newEquipment;
                break;
            case EquipmentSlot.Armor:
                prevEquipment = EquippedArmor;
                EquippedArmor = newEquipment;
                break;
        }
        
        //이전 장비 해제 메세지
        if (prevEquipment != null)
        {
            Console.WriteLine($"{prevEquipment} 장착 해제");
        }
        Console.WriteLine($"{newEquipment.Name} 장착 완료");
    }
    //장비 해제
    public Equipment? UnequipItem(EquipmentSlot slot)
    {
        Equipment? equipment = null;
        switch (slot)
        {
            case EquipmentSlot.Weapon:
                equipment = EquippedWeapon;
                EquippedWeapon = null;
                break;
            case EquipmentSlot.Armor:
                equipment = EquippedArmor;
                EquippedArmor = null;
                break;
        }

        if (equipment != null)
        {
            Console.WriteLine($"{equipment.Name} 장착 해제");
        }
        return equipment;
    }
    #endregion
}