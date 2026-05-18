using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project01.Models
{
    public class Mage : Character
    {
        public Mage(string name)
        {
            Name = name;
            MaxHp = 100;
            CurrentHp = MaxHp;
            AttackPower = 10;
            Type = JobType.Mage;

            // ✨ [추가] 마법사의 기술 3개 설정
            SkillNames[0] = "비전 화살";
            SkillNames[1] = "파이어볼";
            SkillNames[2] = "메테오 스트라이크";
        }

        public override void Attack(Monster monster)
        {
            int finalDamage = CalculateDamage(AttackPower, monster.Type);
            Console.WriteLine($"{Name}의 신비한 마법 공격!");
            monster.TakeDamage(finalDamage);
        }

        // ✨ [수정]
        public override void UseSkill(int skillIndex, Monster monster)
        {
            int baseDamage = 0;

            switch (skillIndex)
            {
                case 0:
                    Console.WriteLine($"\n{Name}의 기술: [{SkillNames[0]}]! 마법 화살을 연속 발사합니다.");
                    baseDamage = AttackPower + 8;
                    break;
                case 1:
                    Console.WriteLine($"\n{Name}의 기술: [{SkillNames[1]}]! 거대한 화염구를 던집니다.");
                    baseDamage = AttackPower + 16;
                    break;
                case 2:
                    Console.WriteLine($"\n{Name}의 필살기: [{SkillNames[2]}]! 하늘에서 운석을 소환합니다!");
                    baseDamage = AttackPower + 30;
                    break;
            }

            int finalDamage = CalculateDamage(baseDamage, monster.Type);
            monster.TakeDamage(finalDamage);
        }
    }
}