using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp.Model
{
    public class Enemy : Fighter
    {       
        public Enemy(string _name, float _hp, float _armor, float _manaShield, float _dodge, float _resistance, float _damage, float _attackSpeed, float _penetration, bool _physical)
        {     
            Name = _name;
            Hp = _hp;
            CurrentHp = Hp;
            Armor = _armor;
            CurrentArmor = Armor;
            ManaShield = _manaShield;
            CurrentManaShield = ManaShield;
            Dodge = _dodge;
            Resistance = _resistance;            
            Damage = _damage;
            //CritDamage = _critDamage;
            AttackSpeed = _attackSpeed;
            //HitRate = _hitRate;
            Penetration = _penetration;
            PhysicalDamage = _physical;
            HitProgress = 0;
        }
    }
}
