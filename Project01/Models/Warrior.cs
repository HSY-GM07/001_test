using Project01.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Project01.Models
{
    public class Warrior : Character
    {
        public Warrior(string name)
        {
            Name = name;
            MaxHp = 150;
            CurrentHp = MaxHp;
            AttackPower = 15;
            Type = JobType.Warrior;

            // ✨ [추가] 전사의 기술 3개 설정
            SkillNames[0] = "강타 (일반 기술)";
            SkillNames[1] = "방패 가격 (강한 기술)";
            SkillNames[2] = "지각 변동 (필살기)";
        }

        public override void Attack(Monster monster)
        {
            int finalDamage = CalculateDamage(AttackPower, monster.Type);
            Console.WriteLine($"{Name}의 묵직한 검 공격!");
            monster.TakeDamage(finalDamage);
        }

        // ✨ [수정] 기술 번호에 따라 데미지와 연출이 다르게 출력되도록 변경
        public override void UseSkill(int skillIndex, Monster monster)
        {
            int baseDamage = 0;

            switch (skillIndex)
            {
                case 0:
                    Console.WriteLine($"\n{Name}의 기술: [{SkillNames[0]}]!");
                    baseDamage = AttackPower + 5;
                    break;
                case 1:
                    Console.WriteLine($"\n{Name}의 기술: [{SkillNames[1]}]! 적의 자세를 무너뜨립니다.");
                    baseDamage = AttackPower + 12;
                    break;
                case 2:
                    Console.WriteLine($"\n{Name}의 필살기: [{SkillNames[2]}]! 대지를 내리칩니다!");
                    baseDamage = AttackPower + 25;
                    break;
            }

            int finalDamage = CalculateDamage(baseDamage, monster.Type);
            monster.TakeDamage(finalDamage);
        }
    }
}