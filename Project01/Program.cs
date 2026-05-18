//using Project01.Models;
//using Project01.Systems;

//namespace Project01
//{
//    internal class Program
//    {
//        static void Main(string[] args)
//        {
//            Console.WriteLine("Hello, World!");
//        }
//    }
//}




using System;
using System.Collections.Generic;

using Project01.Models;    // Character, Warrior, Monster 등을 가져옴
using Project01.Systems;   // BattleManager를 가져옴
using Project01.Interfaces; // IUsable을 가져옴 (나중에 쓸 경우)

using System;
using System.Collections.Generic;
using Project01.Models;    // Character, Warrior, Monster 등을 가져옴
using Project01.Systems;   // BattleManager를 가져옴

class Program
{
    static void Main(string[] args)
    {
        // 1. 오프닝 및 파티 생성
        Console.Clear();
        Console.WriteLine("==============================================");
        Console.WriteLine("          중세 판타지: 태그 매치 RPG          ");
        Console.WriteLine("==============================================");
        Console.WriteLine("플레이어 파티를 결성합니다...\n");

        List<Character> myParty = new List<Character>
        {
            new Warrior("방구석 여포"),
            new Mage("유리 멘탈"),
            new Rogue("월급 루팡")
        };

        foreach (var member in myParty)
        {
            Console.WriteLine($"▶ {member.Name} ({member.Type})가 파티에 합류했습니다.");
        }
        Console.WriteLine("\n엔터를 누르면 던전으로 진입합니다...");
        Console.ReadLine();

        // 2. 몬스터 리스트 생성 (순차적 진행)
        // 타입 상성 관계: Warrior > Rogue, Rogue > Mage, Mage > Warrior
        List<Monster> dungeonMonsters = new List<Monster>
        {
            new Monster("심술궂은 고블린", 50, 10, JobType.Rogue),    // 전사(Warrior)로 상대 시 유리
            new Monster("훈련된 오크 병사", 80, 15, JobType.Warrior),  // 마법사(Mage)로 상대 시 유리
            new Monster("타락한 마법사", 60, 20, JobType.Mage),       // 도적(Rogue)으로 상대 시 유리
            new Monster("최종 보스: 드래곤", 250, 25, JobType.Warrior)
        };

        // 3. BattleManager 생성
        BattleManager battleManager = new BattleManager(myParty);

        // 4. 순차적으로 몬스터와 대결
        int stage = 1;
        foreach (Monster monster in dungeonMonsters)
        {
            Console.Clear();
            Console.WriteLine("==============================================");
            Console.WriteLine($"       [STAGE {stage}] 던전 깊숙이 진입 중...       ");
            Console.WriteLine("==============================================");
            Console.WriteLine("\n멀리서 강력한 기척이 느껴집니다.");
            Console.WriteLine("엔터를 눌러 조우합니다...");
            Console.ReadLine();

            // 전투 시작 (포켓몬 스타일 메뉴로 진입)
            battleManager.StartBattle(monster);

            // 전투 종료 후 파티 전멸 여부 실시간 확인
            if (myParty.TrueForAll(c => c.IsDead))
            {
                Console.WriteLine("\n[GAME OVER] 모든 용사들이 쓰러졌습니다...");
                Console.WriteLine("프로그램을 종료하려면 엔터를 누르세요.");
                Console.ReadLine();
                return; // 메인 함수 종료 -> 게임 오버
            }

            // 스테이지 클리어 연출 및 정비
            Console.WriteLine($"\n[SUCCESS] 스테이지 {stage}를 무사히 통과했습니다!");
            // RestParty(myParty);
            RestParty(myParty, battleManager);

            Console.WriteLine("\n다음 구역으로 이동하려면 엔터를 누르세요...");
            Console.ReadLine();
            stage++;
        }

        // 5. 게임 클리어 엔딩
        Console.Clear();
        Console.WriteLine("=====================================================");
        Console.WriteLine("축하합니다! 모든 위협을 물리치고 평화를 되찾았습니다!");
        Console.WriteLine("                    - GAME CLEAR -                   ");
        Console.WriteLine("=====================================================");
        Console.ReadLine();
    }

    // 스테이지 간 정비 메서드
    static void RestParty(List<Character> party, BattleManager battleManager)
    {
        Console.WriteLine("\n잠시 캠프를 치고 휴식을 취합니다. 생존자들의 HP가 일부 회복됩니다. (+30 HP)");
        foreach (var member in party)
        {
            if (!member.IsDead)
            {
                member.CurrentHp = Math.Min(member.MaxHp, member.CurrentHp + 30);
                Console.WriteLine($"-> {member.Name}: HP {member.CurrentHp}/{member.MaxHp}");
            }
        }

        // ✨ [추가] 스테이지 클리어 보상으로 포션 1개 지급!
        battleManager.Inventory["체력 포션"]++;
        Console.WriteLine($"보상 획득: 전리품 상자에서 '체력 포션'을 1개 얻었습니다! (현재 보유: {battleManager.Inventory["체력 포션"]}개)");
    }
}