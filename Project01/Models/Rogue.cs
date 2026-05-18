using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Project01.Models
{
    public class Rogue : Character
    {
        public Rogue(string name)
        {
            Name = name;
            MaxHp = 120;
            CurrentHp = MaxHp;
            AttackPower = 20;
            Type = JobType.Rogue;

            // ✨ [추가] 도적의 기술 3개 설정
            SkillNames[0] = "기습 공격";
            SkillNames[1] = "독 바른 표창";
            SkillNames[2] = "그림자 일격";
        }

        public override void Attack(Monster monster)
        {
            int finalDamage = CalculateDamage(AttackPower, monster.Type);
            Console.WriteLine($"{Name}의 신속한 단검 공격!");
            monster.TakeDamage(finalDamage);
        }

        // ✨ [수정]
        public override void UseSkill(int skillIndex, Monster monster)
        {
            int baseDamage = 0;

            switch (skillIndex)
            {
                case 0:
                    Console.WriteLine($"\n{Name}의 기술: [{SkillNames[0]}]! 사각지대에서 찌릅니다.");
                    baseDamage = AttackPower + 6;
                    break;
                case 1:
                    Console.WriteLine($"\n{Name}의 기술: [{SkillNames[1]}]! 치명적인 표창을 날립니다.");
                    baseDamage = AttackPower + 12;
                    break;
                case 2:
                    Console.WriteLine($"\n{Name}의 필살기: [{SkillNames[2]}]! 분신과 함께 적을 벱니다!");
                    baseDamage = AttackPower + 22;
                    break;
            }

            int finalDamage = CalculateDamage(baseDamage, monster.Type);
            monster.TakeDamage(finalDamage);
        }
    }
}