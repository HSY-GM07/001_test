using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project01.Models
{

    // Models/Monster.cs
    public class Monster
    {
        public string Name { get; set; }
        public int Hp { get; set; }
        public int AttackPower { get; set; }
        public JobType Type { get; set; } // 몬스터도 Warrior, Mage, Rogue 중 하나의 상성을 가짐

        public Monster(string name, int hp, int attack, JobType type)
        {
            Name = name;
            Hp = hp;
            AttackPower = attack;
            Type = type;
        }

        public void TakeDamage(int damage)
        {
            Hp -= damage;
            if (Hp < 0) Hp = 0;
            Console.WriteLine($"{Name}이(가) {damage}의 피해를 입었습니다! (남은 HP: {Hp})");
        }

        // 몬스터의 공격 (플레이어 파티 중 현재 나와있는 캐릭터를 공격함)
        public void Attack(Character target)
        {
            Console.WriteLine($"\n{Name}의 공격!");
            target.TakeDamage(AttackPower);
        }
    }

}
