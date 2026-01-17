using System.Text.Encodings.Web;
using TextRPG.Models;

namespace TextRPG.Systems;
using System.Text.Json;
using TextRPG.Data;

public class SaveLoadSystem
{
    //저장 경로 및 파일명
    private const string SaveFilePath = "savegame.json";
    //Json 직렬화 옵션 : 객체 -> 문자열
    private static readonly JsonSerializerOptions jsonOptions = new()
    {
        WriteIndented = true,
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping //한글 지원

    };

    #region 저장 기능

    public static bool SaveGame(Player player, InventorySystem inventory)
    {
        try
        {
            //1.게임 객체(클래스) -> DTO 객체 변환
            var saveData = new GameSaveData
            {
                Player = ConvertToPlayerData(player),
                Inventory = ConvertToItemData(inventory)
            };
            //2.DTO 객체 -> JSON 문자열 변환
            string jsonString = JsonSerializer.Serialize(saveData, jsonOptions);
            //3.JSON 문자열 -> 파일 저장
            File.WriteAllText(SaveFilePath, jsonString);
            Console.WriteLine("게임이 저장되었습니다.");
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }
    }

    //Player -> PlayerData로 변환
    private static PlayerData ConvertToPlayerData(Player player)
    {
        return new PlayerData
        {
            Name = player.Name,
            Job = player.Job.ToString(),
            Level = player.Level,
            CurrentHp = player.CurrentHp,
            MaxHp = player.MaxHp,
            CurrentMp = player.CurrentMp,
            MaxMp = player.MaxMp,
            AttackPower = player.AttackPower,
            Defense = player.Defense,
            Gold = player.Gold,
            EquippedWeaponName = player.EquippedWeapon?.Name,
            EquippedArmorName = player.EquippedArmor?.Name
        };
    }
    
    //InventorySystem -> InventoryData로 변환
    private static List<ItemData> ConvertToItemData(InventorySystem inventory)
    {
        var itemDataList = new List<ItemData>();
        
        for(int i=0;i<inventory.Count;i++)
        {
            var item = inventory.GetItem(i);
            if (item == null) continue;
            var itemData = new ItemData
            {
                Name = item.Name,
            };
            if (item is Equipment equipment)
            {
                itemData.ItemType="Equipment";
                itemData.Slot=equipment.Slot.ToString();
            }
            else if(item is Consumable consumable)
            {
                itemData.ItemType="Consumable";
            }
            itemDataList.Add(itemData);
        }
        return itemDataList;
    }
    
    
    #endregion

}