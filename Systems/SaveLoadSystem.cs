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

    #region 불러오기 기능
    
    //저장된 파일 여부 확인
    public static bool IsSaveFileExists()
    {
        return File.Exists(SaveFilePath);
    }

    public static GameSaveData? LoadGame()
    {
        try
        {
            //1.JSON파일에서 문자열 읽기
            string jsonString = File.ReadAllText(SaveFilePath);
            Console.WriteLine(jsonString);
            //2.JSON 문자열 -> DTO 객체 변환
            var saveData = JsonSerializer.Deserialize<GameSaveData>(jsonString);
            Console.WriteLine("게임이 불러와졌습니다.");
            return saveData;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    //PlayerData -> Player 클래스로 변환 메서드
    public static Player LoadPlayer(PlayerData data)
    {
        //JobType을 문자열->열거형(Enum) 변환
        var job = Enum.Parse<JobType>(data.Job);
        
        //Player 객체 생성
        var player = new Player(data.Name, job);
        
        //스텟 설정
        player.Level = data.Level;
        player.CurrentHp = data.CurrentHp;
        player.MaxHp = data.MaxHp;
        player.CurrentMp = data.CurrentMp;
        player.MaxMp = data.MaxMp;
        player.AttackPower = data.AttackPower;
        player.Defense = data.Defense;
        player.Gold = data.Gold;
        
        return player;
    }
    
    //ItemData DTO 를 Inventory 클래스로 변환 메서드
    
    //저장된 장착 아이템을 복원하는 메서드(무기/방어구)
    
    //아이템 생성->Inventory추가 

    #endregion

}