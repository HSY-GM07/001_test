using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project01.Models
{
    public enum JobType { Warrior, Mage, Rogue }

    public abstract class Character
    {
        public string Name { get; set; }
        public int Level { get; set; } = 1;
        public int MaxHp { get; set; }
        public int CurrentHp { get; set; }
        public int AttackPower { get; set; }
        public JobType Type { get; set; }
        public bool IsDead => CurrentHp <= 0;

        // ✨ [추가] 각 직업의 기술 3개 이름을 담을 배열
        public string[] SkillNames { get; set; } = new string[3];

        public int CalculateDamage(int baseDamage, JobType targetType)
        {
            if ((this.Type == JobType.Warrior && targetType == JobType.Rogue) ||
                (this.Type == JobType.Rogue && targetType == JobType.Mage) ||
                (this.Type == JobType.Mage && targetType == JobType.Warrior))
            {
                Console.WriteLine("상성 우위! 데미지가 2배로 적용됩니다!");
                return baseDamage * 2;
            }
            return baseDamage;
        }

        public abstract void Attack(Monster monster);

        // ✨ [수정] 어떤 기술을 쓸지 번호(index)를 매개변수로 받도록 변경
        public abstract void UseSkill(int skillIndex, Monster monster);

        public void TakeDamage(int damage)
        {
            CurrentHp -= damage;
            if (CurrentHp < 0) CurrentHp = 0;
            Console.WriteLine($"{Name}이(가) {damage}의 피해를 입었습니다! (남은 HP: {CurrentHp})");
        }
    }
}