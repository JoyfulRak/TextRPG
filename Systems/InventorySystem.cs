using System;
using TextRPG.Utils;

namespace TextRPG.Models;
using System.Collections.Generic;

public class InventorySystem
{
    #region 프로퍼티

    //아이템 목록
    private List<Item> Items { get; set; }

    //아이템 갯수(읽기 전용)
    private int Count => Items.Count; //goes to 


    #endregion

    #region 생성자

    public InventorySystem()
    {
        Items = new List<Item>();
    }

    #endregion

    #region 아이템 관리

    //아이템 추가
    public void AddItem(Item item)
    {
        if (item == null)
        {
            return; // 아이템이 없으면 아무것도 하지 않고 그냥 돌아갑니다.
        }

        Items.Add(item);
        Console.WriteLine($"{item.Name} 을 인벤토리에 추가했습니다.");
    }

    //아이템 삭제
    public bool RemoveItem(Item item)
    {
        if (Items.Remove(item))
        {
            Console.WriteLine($"{item.Name} 을 인벤토리에서 제거했습니다.");
            return true;
        }

        return false;
    }

    #endregion

    #region 인벤토리 표시

    public void DisplayInventory()
    {
        Console.Clear();
        Console.WriteLine("\n╔════════════════════════════════╗");
        Console.WriteLine("║         인벤토리               ║");
        Console.WriteLine("╚════════════════════════════════╝");
        if (Items.Count == 0)
        {
            Console.WriteLine("인벤토리가 비어 있습니다.");
            return;
        }

        Console.WriteLine("\n[아이템 목록]");
        for (int i = 0; i < Items.Count; i++)
        {
            Console.Write($"{i + 1}. {Items[i].Name} - {Items[i].Description}");
            Items[i].DisplayInfo();
        }

    }

    public void showInventoryMenu(Player? player)
    {
        while (true)
        {
            DisplayInventory();

            Console.WriteLine("\n 선택하세요.");
            Console.WriteLine("1. 아이템 사용");
            Console.WriteLine("2. 아이템 버리기");
            Console.WriteLine("3. 나가기");
            Console.Write("선택: ");

            string? input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    //아이템 사용로직
                    UseItem(player);
                    break;
                case "2":
                    //아이템 버리기 로직
                    DropItem(player);
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine("잘못된 선택입니다. 다시 선택하세요.");
                    break;
            }

        }
    }

    #endregion

    #region 아이템 사용

    private void UseItem(Player player)
    {
        if (Items.Count == 0)
        {
            Console.WriteLine("인벤토리가 비어 있습니다.");
            return;
        }

        Console.Write("사용할 아이템 번호를 입력하세요: ");
        string? input = Console.ReadLine();
        if (int.TryParse(input, out int index) && index >= 1 && index <= Items.Count)
        {
            Item item = Items[index - 1];
            if (item.Use(player))
            {
                //소모품일 경우 사용후 인벤토리에서 제거
                if (item is Consumable)
                {
                    RemoveItem(item);
                }
            }
        }

        else if (index != 0)
        {
            Console.WriteLine("잘못된 입력입니다.");
        }
    }

    #endregion

    #region 아이템버리기

    private void DropItem(Player player)
    {
        if (Items.Count == 0) return;

        Console.WriteLine("\n버릴 아이템 번호(0:취소): ");

        if (int.TryParse(Console.ReadLine(), out int index) && index >= 1 && index <= Items.Count)
        {
            Item item = Items[index - 1];
            Console.WriteLine($"정말 {item.Name} 을 버리시겠습니까? (y/n): ");

            if (Console.ReadLine().ToLower() == "y")
            {
                //장착 해제 로직
                if (item is Equipment equipment)
                {
                    if (equipment == player.EquippedWeapon)
                    {
                        player.UnequipItem(EquipmentSlot.Weapon);
                    }
                    else if (equipment == player.EquippedArmor)
                    {
                        player.UnequipItem(EquipmentSlot.Armor);
                    }
                }
            }

            RemoveItem(item);
            Console.WriteLine($"{item.Name}을 버렸습니다.");
            ConsoleUI.PressAnyKey();
        }
        //RemoveItem(item);
        //Console.WriteLine($"{item.Name} 을 버렸습니다.");


        else if (index != 0)
        {
            Console.WriteLine("잘못된 입력입니다.");
        }
    }

    #endregion
}