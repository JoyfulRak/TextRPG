using TextRPG.Utils;
namespace TextRPG.Data;

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

    #region 게임 시작/종료

    public void StartGame()
    {
        //타이틀 표시
        ConsoleUI.showTitle();
        Console.WriteLine("빡센게임에 오신걸 환영합니다.");
    }

    #endregion
}