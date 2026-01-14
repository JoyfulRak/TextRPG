using TextRPG.Models;
namespace TextRPG.Systems;

public class BattleSystem
{
    #region 던전입장-전투 실행

    //전투 시작 메서드
    //반환값 : 전투 승리 여부
    public bool StartBattle(Player player, Enemy enemy)
    {
        Console.Clear();

        Console.WriteLine("던전에 입장했습니다!");
        Console.WriteLine("\n╔════════════════════════════════╗");
        Console.WriteLine("║       전투 시작!               ║");
        Console.WriteLine("╚════════════════════════════════╝\n");
        Console.WriteLine("적과 마주쳤습니다!");

        //등장한 적 캐릭터 정보 출력
        enemy.DisplayInfo();
        //턴 변수 정의
        int turn = 1;
        //전투 루프
        while (player.IsAlive && enemy.IsAlive)
        {
            Console.WriteLine($"\n---- {turn} 턴 ----");
            PlayerTurn(player, enemy);

            turn++;
            //TODO : 플레이어의 공격


            int damageToEnemy = Math.Max(0, player.AttackPower - enemy.Defense);
            //enemy.CurrentHp -= damageToEnemy;
            Console.WriteLine($"{player.Name}(이)가 {enemy.Name}(을)를 공격하여 {damageToEnemy}의 피해를 입혔습니다.");
            //TODO:적 사망여부 판단
            if (!enemy.IsAlive)
            {
                Console.WriteLine($"\n{enemy.Name}(이)가 쓰러졌습니다!");
                Console.WriteLine($"{player.Name}(이)가 {enemy.GoldReward} 골드를 획득했습니다!");
                // player.Gold += enemy.GoldReward;
                return true; //전투 승리
            }
            //TODO:적 턴

        }

        //전투 결과 반환
        return player.IsAlive;


        #endregion

        #region 플레이어 턴

        //플레이어 턴(1.공격, 2.스킬 발동, 3.도망)

    }

    private void PlayerTurn(Player player, Enemy enemy)
    {
        Console.WriteLine($"\n{player.Name}의 턴입니다!");
        Console.WriteLine($"HP: {player.CurrentHp}/{player.MaxHp} | MP: {player.CurrentMp}/{player.MaxMp}");
        Console.WriteLine("\n 행동을 선택하세요.");
        Console.WriteLine("1. 공격");
        Console.WriteLine("2. 스킬 발동");
        Console.WriteLine("3. 도망");
        Console.Write("행동 선택 (1-3): ");

        while (true)
        {
            Console.Write("\n 선택 (1-3): ");
            string? input = Console.ReadLine();

            switch (input)
            {
                case "1":
                {
                    //공격
                    int damage = player.Attack(enemy);
                    Console.WriteLine($"{player.Name}(이)가 {enemy.Name}(을)를 공격하여 {damage}의 피해를 입혔습니다.");
                    Console.WriteLine($"{enemy.Name}의 남은 HP: {enemy.CurrentHp}/{enemy.MaxHp}");
                    //int damageToEnemy = Math.Max(0, player.AttackPower - enemy.Defense);
                    //enemy.CurrentHp -= damageToEnemy;

                    break;
                }
                case "2":
                {
                    //스킬 발동
                    //스킬 사용 전에 MP체크
                    if (player.CurrentHp < 15)
                    {
                        Console.WriteLine("MP가 부족합니다.");
                        continue;
                    }
                    //스킬 발동
                    int skillDamage=player.SkillAttack(enemy);
                    Console.WriteLine($"{player.Name}의 스킬 공격! {enemy.Name}에게 {skillDamage}의 피해를 입혔습니다.");
                    Console.WriteLine($"{enemy.Name}의 남은 HP: {enemy.CurrentHp}/{enemy.MaxHp}");
                    break;
                }
                case "3":
                    break;

                default:
                    Console.WriteLine("잘못된 입력입니다. 다시 선택해주세요.");
                    break;


            }
        }
    }

    #endregion

    #region 적 턴



    #endregion
}
