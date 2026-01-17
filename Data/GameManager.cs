using TextRPG.Utils;
namespace TextRPG.Data;
using TextRPG.Models;
using TextRPG.Systems;

public class GameManager
{
    //싱글톤 패턴(Singleton Pattern) 구현. 다른클래스에서 손쉽게 접근.

    #region 싱글톤 패턴
    // 싱글톤 인스턴스(내부 접근용 변수:필드)
    private static GameManager instance;
    
    //외부에서 인스턴스에 접근할 수 있는 정책 속성(프로퍼티)
    public static GameManager Instance
    {
        get
        {
            //인스턴스가 없으면 새로 생성
            if (instance == null)
            {
                instance = new GameManager();
            }
            return instance;
        }
    }

    private GameManager()
    {
        //클래스가 생성될때 초기화 작업 수행
        
        //전투 시스템 초기화
        BattleSystem = new BattleSystem();
        
        //상점 시스템 초기화
        ShopSystem = new ShopSystem();
    }
    #endregion

    #region 프로퍼티

    public Player? Player { get; private set; }
    
    //전투 시스템
    public BattleSystem BattleSystem { get; private set; }
    
    //인벤토리 시스템
    public InventorySystem Inventory { get; private set; } 
    
    //상점 시스템
    public ShopSystem ShopSystem { get; private set; }
    
    //게임 실행 여부
    public bool IsRunning { get; private set; } = true;
    

    #endregion

    #region 게임 시작/종료

    public void StartGame(bool loadedGame=false)
    {
        //타이틀 표시
        ConsoleUI.showTitle();
        Console.WriteLine("빡센게임에 오신걸 환영합니다.");
        
        //새로 시작하는 게임에만 새 캐릭터 및 설정을 처리
        if (!loadedGame)
        {

            //캐릭터 생성
            CreateCharacter();

            //테스트 코드.
            //Player.TakeDamage(100);

            //인벤토리 초기화
            Inventory = new InventorySystem();
        }

        //초기 아이템 지급
        SetupInitItems();
        // [추가] 사용자가 지급 메시지를 확인할 수 있도록 잠시 대기
        ConsoleUI.PressAnyKey();
        
        
        //메인 게임 루프
        IsRunning = true;
        while (IsRunning)
        {
            ShowMainMenu();
        }
        
        //게임 종료 처리
        if (!IsRunning)
        {
            ConsoleUI.ShowGameOver();
        }
        //TODO:인벤토리 초기화
        //TODO:초기 아이템 지급
        
        //

    }
    

    #endregion

    #region 캐릭터 생성

    private void CreateCharacter()
    {
        //이름 입력
        Console.Write("캐릭터의 이름을 입력하세요");
        String? name=Console.ReadLine();//nullable 허용

        if (string.IsNullOrWhiteSpace(name))
        {
            name = "무명용사";
            
        }
        Console.WriteLine($"{name}님, 모험을 시작하겠습니다!");
        
        //직업선택
        Console.WriteLine("직업을 선택하세요.");
        Console.WriteLine("1:전사");
        Console.WriteLine("2:궁수");
        Console.WriteLine("3.마법사");

        JobType job = JobType.Warrior;

        while (true)
        {
            Console.WriteLine("선택 (1-3) : ");
            String? input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    job=JobType.Warrior;
                    break;
                case "2":
                    job=JobType.Archer;
                    break;
                case "3":
                    job=JobType.Wizard;
                    break;
                default:
                    Console.WriteLine("잘못된 입력입니다. 다시선택해주세요");
                    continue;
            }
            break;
        }
        //입력한 이름과 선택한 직업으로 플레이어 캐릭터 생성
        Player = new Player(name, job);
        Console.WriteLine($"\n{name}님, {job} 직업으로 캐릭터가 생성되었습니다.");
        
        // //적 캐릭터 생성
        // Enemy enemy=Enemy.CreateEnemy(Player.Level);
        // Console.WriteLine($"적 {enemy.Name}(이)가 나타났습니다!");
        // enemy.DisplayInfo();
        //
        // //전투 테스트
        // BattleSystem battleSystem=new BattleSystem();
        // bool PlayerWin=battleSystem.StartBattle(Player,enemy);
        
        
        ConsoleUI.PressAnyKey();
    }
    
    //0:전사, 1:마법사, 2:궁수 - 열거형 변수를 선언
    
    //초기 아이템 지급
    private void SetupInitItems()
    {
        //Inventory.AddItem(Equipment.CreateWeapon("목검"));
        //Inventory.AddItem(Equipment.CreateArmor("천갑옷"));
        //Console.WriteLine("\n초기 장비 아이템을 지급했습니다.");
        
        //기본장비
        var weapon = Equipment.CreateWeapon("목검");
        var armor = Equipment.CreateArmor("천갑옷");
        Inventory.AddItem(weapon);
        Inventory.AddItem(armor);
        
        //기본 장비 착용
        Player.EquipItem(weapon);
        Player.EquipItem(armor);
        Console.WriteLine("\n기본 장비를 지급하고 착용했습니다.");
        
        //포션 지급
        Inventory.AddItem(Consumable.CreatePotion("체력포션"));
        Inventory.AddItem(Consumable.CreatePotion("체력포션"));
        Inventory.AddItem(Consumable.CreatePotion("마나포션"));

        
    }

    #endregion

    #region 메인 메뉴
    public void ShowMainMenu()
    {
            Console.Clear();
            Console.WriteLine("╔══════════════════════════════════════════╗");
            Console.WriteLine("║                                          ║");
            Console.WriteLine("║            메인 메뉴                     ║");
            Console.WriteLine("║                                          ║");
            Console.WriteLine("╚══════════════════════════════════════════╝\n");
            Console.WriteLine("1. 상태보기");
            Console.WriteLine("2. 인벤토리");
            Console.WriteLine("3. 상점 방문");
            Console.WriteLine("4. 던전 입장(전투)");
            Console.WriteLine("5. 휴식(체력/마나 회복)");
            Console.WriteLine("6. 게임 저장");
            Console.WriteLine("0. 게임 종료");
            Console.WriteLine("=================");
            Console.Write("선택 (1-6): ");
            
            String? input = Console.ReadLine();
            switch (input)
            { 
                case "1":
                    //모험 시작
                    Player.DisplayInfo();
                    ConsoleUI.PressAnyKey();
                    break;
                case "2":
                    //인벤토리 방문
                    Inventory.showInventoryMenu(Player);
                    break;
                case "3":
                    //상점 방문
                    ShopSystem.ShowShopMenu(Player, Inventory);
                    break;
                case "4":
                    //던전 입장 및 전투 기능 구현
                    EnterDungeon();
                    break;
                case "5":
                    //휴식 기능 구현
                    Rest();
                    
                    break;
                case "6":
                    //저장 기능 구현
                    SaveGame();
                    
                    break;
                case "0":
                    IsRunning = false;
                    Console.WriteLine("게임을 종료합니다. 이용해주셔서 감사합니다!");
                    break;
                default:
                    Console.WriteLine("잘못된 입력입니다. 다시 선택해주세요.");
                    ConsoleUI.PressAnyKey();
                    break;
            }
    }
    #endregion

    #region 메뉴 기능
    //던전 입장 
    public void EnterDungeon()
    {
        Console.Clear();
        Console.WriteLine("던전에 입장합니다...");
        
        //적 캐릭터 생성
        Enemy enemy=Enemy.CreateEnemy(Player.Level);
        //Console.WriteLine($"적 {enemy.Name}(이)가 나타났습니다!");
        ConsoleUI.PressAnyKey();

        BattleSystem.StartBattle(Player, enemy);
        Console.WriteLine("던전에서 나왔습니다.");
        ConsoleUI.PressAnyKey();
    }
    
    //휴식 (체력/마나 회복)
    public void Rest()
    {
        //상수(Constant)
        const int restCost = 50;
        
        Console.Clear();
        Console.WriteLine("여관에서 휴식을 취합니다.");
        Console.WriteLine($"\n휴식 비용: {restCost} 골드");
        
        if (Player.Gold < restCost)
        {
            Console.WriteLine("골드가 부족합니다. 휴식을 취할 수 없습니다.");
            ConsoleUI.PressAnyKey();
            return;
        }
        
        Console.Write("휴식을 취하시겠습니까? (Y/N): ");
        if (Console.ReadKey().Key == ConsoleKey.Y)
        {
            Player.SpendGold(restCost);
            Player.HealHP(Player.MaxHp);
            Player.HealMP(Player.MaxMp);
            Console.WriteLine("\n휴식을 취했습니다. 체력과 마나가 모두 회복되었습니다.");
            ConsoleUI.PressAnyKey();
        }
    }

    #endregion

    #region 저장/로드 기능

    //게임저장
    public void SaveGame()
    {
        if (Player == null || Inventory == null)
        {
            Console.WriteLine("저장할 게임 데이터가 없습니다.");
            return;
        }

        if (SaveLoadSystem.SaveGame(Player, Inventory))
        {
            Console.WriteLine("게임이 성공적으로 저장되었습니다.");
            ConsoleUI.PressAnyKey();
            
        }
    }
    
    //게임 로드
    public bool LoadGame()
    {
        var saveDate = SaveLoadSystem.LoadGame();
        if (saveDate == null) return false;
        
        //1.플레이어 데이터 복원
        Player = SaveLoadSystem.LoadPlayer(saveDate.Player);
        
        //2.인벤토리 데이터 복원
        
        //3.장착 아이템 복원
        Console.WriteLine("게임이 성공적으로 로드되었습니다.");
        ConsoleUI.PressAnyKey();
        return true;
    }

    #endregion
    
}