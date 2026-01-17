using TextRPG.Utils;
using TextRPG.Data;
using TextRPG.Systems;

namespace TextRPG;

class Program
{
    static void Main(string[] args)
    {
        
        //콘솔 인코딩 설정(한글 지원)
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        
        //저장된 게임 존재여부 확인
        if (SaveLoadSystem.IsSaveFileExists())
        {
            //메뉴 오픈(새 게임, 이어하기, 종료)
            ShowStartMenu();
        }
        else
        {
            GameManager.Instance.StartGame();
            
        }
        //TODO:게임 로드 및 새 게임 시작
        
    }
    static void ShowStartMenu()
    {
        Console.Clear();
       
       ConsoleUI.showTitle();
        Console.WriteLine("1. 새 게임 시작");
        Console.WriteLine("2. 이어하기");
        Console.WriteLine("0. 종료");
        while (true)
        {
            Console.Write("메뉴 선택: ");
            string? input = Console.ReadLine();
            switch (input)
            {
                case "1":
                    GameManager.Instance.StartGame();
                    return;
                case "2":
                    //이어하기 로직
                    if (GameManager.Instance.LoadGame())
                    {
                        GameManager.Instance.StartGame(true);
                    }
                    return;
                case "0":
                    Console.WriteLine("게임을 종료합니다.");
                    return;
                default:
                    Console.WriteLine("잘못된 입력입니다. 다시 시도하세요.");
                    ConsoleUI.PressAnyKey();
                    break;
            }
        }
    }
}