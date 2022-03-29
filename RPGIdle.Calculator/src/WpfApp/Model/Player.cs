using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp.Model
{
    public class Player : Fighter
    {      
        public Player(string _name, float _str, float _dex, float _int, float _hp, float _armor, float _manaShield, float _dodge, float _resistance, float _vitality, 
                        float _regeneration, float _damage, float _baseAttackSpeed, float _addAttackSpeed, float _penetration, bool _physical)
        {
            Name = _name;
            Strength = _str;
            Dexterity = _dex;
            Intelligence = _int;
            Hp = _hp;
            CurrentHp = Hp;
            Armor = _armor;
            CurrentArmor = Armor;
            ManaShield = _manaShield;
            CurrentManaShield = ManaShield;
            Dodge = _dodge;
            Resistance = _resistance;
            Vitality = _vitality;
            CurrentVit = Vitality * Hp;
            Regeneration = _regeneration;
            Damage = _damage;
            //CritDamage = _critDamage;
            BaseAttackSpeed = _baseAttackSpeed;
            AddAttackSpeed = _addAttackSpeed;
            AttackSpeed = BaseAttackSpeed * (1 + (AddAttackSpeed / 100));
            //HitRate = _hitRate;
            Penetration = _penetration;
            PhysicalDamage = _physical;
            HitProgress = 0;
        }
    }
}
