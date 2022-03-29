using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp.Model
{
    public abstract class Fighter
    {
        public string Name { get; set; }

        private float
        strength,
        dexterity,
        intelligence,
        armor,
        currentArmor,
        dodge,
        resistance,
        hp,
        currentHp,
        manaShield,
        currentManaShield,
        damage,
        vitality,
        currentVit,
        regeneration,
        critDamage,
        baseAttackSpeed,
        addAttackSpeed,
        attackSpeed,
        hitRate,
        penetration;

        public float Strength { get { return strength; } set { strength = value; } }
        public float Dexterity { get { return dexterity; } set { dexterity = value; } }
        public float Intelligence { get { return intelligence; } set { intelligence = value; } }
        public float Hp { get { return hp; } set { hp = value + Strength; } }
        public float Armor { get { return armor; } set { armor = value + Strength; } }
        public float ManaShield { get { return manaShield; } set { manaShield = value + Intelligence; } }
        public float Dodge { get { return dodge; } set { dodge = value + Dexterity; } }
        public float Resistance { get { return resistance; } set { resistance = value + Intelligence; } }
        public float Vitality { get { return vitality; } set { if (value == 0) { vitality = 2; } else { vitality = value; } } }
        public float CurrentVit { get { return currentVit; } set { currentVit = value; } }
        public float Regeneration { get { return regeneration; } set { if (value == 0) { regeneration = 50; } else { regeneration = value; } } }
        public float BaseAttackSpeed { get { return baseAttackSpeed; } set { if (value == 0) { baseAttackSpeed = 0.3f; } else { baseAttackSpeed = value; } } }
        public float AddAttackSpeed { get { return addAttackSpeed; } set { addAttackSpeed = value + Strength * 0.5f; } }
        public float Penetration { get { return penetration; } set { penetration = value + Intelligence; } }

        
        public float CurrentHp { get { return currentHp; } set { currentHp = value; } }        
        public float CurrentArmor { get { return currentArmor; } set { currentArmor = value; } }        
        public float CurrentManaShield { get { return currentManaShield; } set { currentManaShield = value; } }        
        
        public float Damage { get { return damage; } set { if (value == 0) { damage = 2; } else { damage = value; } } }
        public float CritDamage { get { return critDamage; } set { if (value == 0) { critDamage = 1.5f; } else { critDamage = value; } } }
        public float AttackSpeed { get { return attackSpeed; } set { attackSpeed = value; } }
        public float HitRate { get { return hitRate; } set { hitRate = value; } }        
        public bool PhysicalDamage { get; set; }

        public float HitProgress { get; set; }
    }
}
