using TextRPG.Utils;
namespace TextRPG.Data;
using TextRPG.Models;

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
    }
    #endregion

    #region 프로퍼티

    public Player? Player { get; private set; }
    

    #endregion

    #region 게임 시작/종료

    public void StartGame()
    {
        //타이틀 표시
        ConsoleUI.showTitle();
        Console.WriteLine("빡센게임에 오신걸 환영합니다.");
        
        //TODO:캐릭터 생성
        CreateCharacter();
        //TODO:인벤토리 초기화
        //TODO:초기 아이템 지급

    }
    

    #endregion

    #region 캐릭터 생성

    private void CreateCharacter()
    {
        //이름 입력
        Console.WriteLine("캐릭터의 이름을 입력하세요");
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
    }
    
    //0:전사, 1:마법사, 2:궁수 - 열거형 변수를 선언

    #endregion
}