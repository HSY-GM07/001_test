using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Project01.Models;
using System.Collections.Generic; // List를 쓰기 위해 필요

using System;
using System.Collections.Generic;
using Project01.Models;

namespace Project01.Systems
{







    public class BattleManager
    {
        private List<Character> party;       // 플레이어 파티 (3명)
        private Character currentCharacter; // 현재 필드에 나온 캐릭터
        private Monster currentMonster;     // 현재 상대 중인 몬스터
        public Dictionary<string, int> Inventory { get; set; } // ✨ [추가] 아이템 관리를 위한 딕셔너리

        public BattleManager(List<Character> playerParty)
        {
            party = playerParty;
            currentCharacter = party[0];    // 시작은 언제나 첫 번째 멤버

            // ✨ [추가] 시작할 때 체력 포션 1개 기본 지급
            Inventory = new Dictionary<string, int> {
                { "체력 포션", 1 }
            };
        }

        public void StartBattle(Monster monster)
        {
            currentMonster = monster;

            Console.Clear();
            Console.WriteLine($"\n{currentMonster.Name}(타입: {currentMonster.Type})이(가) 나타났다!");
            Console.WriteLine("엔터를 눌러 전투를 시작하세요...");
            Console.ReadLine();

            // 몬스터가 살아있고, 파티가 전멸하지 않았다면 턴 반복
            while (currentMonster.Hp > 0 && !IsPartyWiped())
            {
                RenderFrame(); // 화면을 깨끗하게 지우고 현재 HP 상태 출력
                PlayerTurn();  // 플레이어의 선택 루프 실행 (행동 완료 시 턴 종료)

                // 플레이어의 행동 후에도 몬스터가 살아있다면 몬스터가 공격
                if (currentMonster.Hp > 0)
                {
                    currentMonster.Attack(currentCharacter);
                    Console.WriteLine("\n엔터를 눌러 다음 턴으로...");
                    Console.ReadLine();

                    CheckCurrentCharacterDown(); // 현재 캐릭터가 죽었는지 확인 및 강제 교체
                }
            }

            if (currentMonster.Hp <= 0)
            {
                Console.WriteLine($"\n{currentMonster.Name}을(를) 물리쳤다!");
            }
        }

        // 포켓몬스터처럼 화면 상단에 항시 현재 전투 상태를 UI 형태로 그려주는 메서드
        private void RenderFrame()
        {
            Console.Clear();
            Console.WriteLine("==================================================");
            Console.WriteLine($"[상대] {currentMonster.Name} (HP: {currentMonster.Hp} | 타입: {currentMonster.Type})");
            Console.WriteLine("--------------------------------------------------");
            Console.WriteLine($"[출전] {currentCharacter.Name} (HP: {currentCharacter.CurrentHp}/{currentCharacter.MaxHp} | 타입: {currentCharacter.Type})");
            Console.WriteLine("==================================================");
        }

        // 플레이어 메인 메뉴 (포켓몬 인터페이스 구현)
        private void PlayerTurn()
        {
            bool isTurnEnded = false;

            // 행동(공격, 스킬, 교대 성공)을 완료하기 전까지는 메뉴가 계속 반복됨 (뒤로가기 지원)
            while (!isTurnEnded)
            {
                RenderFrame();
                Console.WriteLine("\n[ 무엇을 할까? ]");
                Console.WriteLine("1. 싸운다      2. 아이템");
                Console.WriteLine("3. 교대        4. 게임 종료");
                Console.Write("▶ 선택: ");

                string input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        // 싸운다 메뉴 진입 (스킬을 쓰면 true를 반환하여 턴 종료)
                        isTurnEnded = SelectSkillMenu();
                        break;
                    case "2":
                        // 아이템 메뉴 진입
                        isTurnEnded = UseItemMenu();
                        break;
                    case "3":
                        // 교대 메뉴 진입 (정상 교체하면 true를 반환하여 턴 종료)
                        isTurnEnded = SwitchCharacterMenu();
                        break;
                    case "4":
                        Console.WriteLine("\n게임 종료 선택. 프로그램을 종료합니다.");
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("잘못된 입력입니다. 다시 선택하세요.");
                        Console.ReadLine();
                        break;
                }
            }
        }

        // 1. 싸운다 (하위 기술 메뉴)
        private bool SelectSkillMenu()
        {
            RenderFrame();
            Console.WriteLine($"\n[ {currentCharacter.Name}의 기술 목록 ]");

            // 앞서 1단계에서 세팅한 기술 3개 이름을 출력
            for (int i = 0; i < currentCharacter.SkillNames.Length; i++)
            {
                Console.WriteLine($"{i + 1}. {currentCharacter.SkillNames[i]}");
            }
            Console.WriteLine("4. 뒤로가기");
            Console.Write("▶ 선택: ");

            string input = Console.ReadLine();

            if (input == "4") return false; // 뒤로가기를 누르면 false를 반환하여 메인 메뉴로 복귀

            if (int.TryParse(input, out int choice) && choice >= 1 && choice <= 3)
            {
                // 선택한 기술 사용 (배열 인덱스이므로 -1 해줌)
                currentCharacter.UseSkill(choice - 1, currentMonster);
                Console.WriteLine("\n엔터를 누르면 몬스터의 턴으로 넘어갑니다...");
                Console.ReadLine();
                return true; // 행동 완료, 턴 종료
            }

            Console.WriteLine("X 올바른 번호를 선택하세요.");
            Console.ReadLine();
            return false;
        }

        // 2. 아이템 메뉴
        private bool UseItemMenu()
        {
            RenderFrame();

            // 딕셔너리에서 현재 포션 개수 가져오기
            int potionCount = Inventory["체력 포션"];

            Console.WriteLine("\n[ 가방 안의 아이템 ]");
            Console.WriteLine($"1. 체력 포션 (HP +50) - 보유 개수: {potionCount}개");
            Console.WriteLine("2. 뒤로가기");
            Console.Write("▶ 선택: ");

            string input = Console.ReadLine();
            if (input == "2") return false; // 뒤로가기

            if (input == "1")
            {
                if (potionCount > 0)
                {
                    // 포션 사용 로직
                    Inventory["체력 포션"]--; // 개수 1개 감소

                    int healAmount = 50;
                    currentCharacter.CurrentHp = Math.Min(currentCharacter.MaxHp, currentCharacter.CurrentHp + healAmount);

                    Console.WriteLine($"\n포션을 사용했습니다! {currentCharacter.Name}의 HP가 {healAmount} 회복되었습니다.");
                    Console.WriteLine($"-> 현재 HP: {currentCharacter.CurrentHp}/{currentCharacter.MaxHp}");
                    Console.WriteLine("\n엔터를 누르면 몬스터의 턴으로 넘어갑니다...");
                    Console.ReadLine();
                    return true; // 턴 종료 성공!
                }
                else
                {
                    Console.WriteLine("\n포션이 부족하여 사용할 수 없습니다!");
                    Console.ReadLine();
                }
            }
            return false; // 아이템 사용 실패 시 메인 메뉴로 복귀
        }

        // 3. 교대 메뉴
        private bool SwitchCharacterMenu()
        {
            RenderFrame();
            Console.WriteLine("\n[ 교체할 파티원을 선택하세요 ]");

            for (int i = 0; i < party.Count; i++)
            {
                string status = party[i].IsDead ? "[사망]" : (party[i] == currentCharacter ? "[출전중]" : "[대기]");
                Console.WriteLine($"{i + 1}. {party[i].Name} ({party[i].Type}) {status}");
            }
            Console.WriteLine($"{party.Count + 1}. 뒤로가기");
            Console.Write("▶ 선택: ");

            string input = Console.ReadLine();

            // 뒤로가기 번호 처리
            if (input == (party.Count + 1).ToString()) return false;

            if (int.TryParse(input, out int choice) && choice >= 1 && choice <= party.Count)
            {
                Character selected = party[choice - 1];

                if (selected.IsDead)
                {
                    Console.WriteLine("사망한 캐릭터로는 교체할 수 없습니다!");
                    Console.ReadLine();
                    return false;
                }
                if (selected == currentCharacter)
                {
                    Console.WriteLine("이미 출전 중인 캐릭터입니다.");
                    Console.ReadLine();
                    return false;
                }

                // 정상 교체 진행
                currentCharacter = selected;
                Console.WriteLine($"\n{currentCharacter.Name}(으)로 교체되었습니다! 다음 몬스터 턴이 시작됩니다.");
                Console.ReadLine();
                return true; // 교체 성공 시 턴 종료 (포켓몬 룰 적용)
            }

            Console.WriteLine("올바른 번호를 선택하세요.");
            Console.ReadLine();
            return false;
        }

        private bool IsPartyWiped() => party.TrueForAll(c => c.IsDead);

        // 전투 중 현재 캐릭터가 사망했을 때 처리
        private void CheckCurrentCharacterDown()
        {
            if (currentCharacter.IsDead)
            {
                Console.WriteLine($"\n❗ {currentCharacter.Name}이(가) 쓰러졌습니다!");

                if (!IsPartyWiped())
                {
                    Console.WriteLine("다음 출전할 캐릭터를 강제로 교체해야 합니다.");
                    Console.ReadLine();

                    // 강제 교체할 때는 메뉴에서 취소(뒤로가기)를 못 하도록 반복 유도
                    while (true)
                    {
                        if (SwitchCharacterMenu()) break;
                    }
                }
            }
        }
    }
}