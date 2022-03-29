using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp.Model
{
    public class Combat
    {
        public Player Player { get; set; }
        public Enemy Enemy { get; set; }

        public int KillCount { get; set; }
        public int EarlyDeaths { get; set; }
        public float AvgKills { get; set; }
        public float AvgEarlyDeaths { get; set; }

        public float playerDodge, playerResistance, playerHitChance, playerCritChance, playerDPS;

        public float enemyDodge, enemyResistance, enemyHitChance, enemyCritChance, enemyDPS;        

        System.Random randomAll = new System.Random();

        public Armor Armor { get; set; }
        public Dodge Dodge { get; set; }
        public Resistance Resistance { get; set; }
        public ManaShield ManaShield { get; set; }



        public Combat(Player _player, Enemy _enemy, Armor _armor, Dodge _dodge, Resistance _resistance, ManaShield _manaShield)
        {
            Player = _player;
            Enemy = _enemy; 
            Armor = _armor;
            Dodge = _dodge;
            Resistance = _resistance;
            ManaShield = _manaShield;

            //CalcStats();
        }

        public void Fight(int count)
        {
            if (Player != null && Enemy != null)
            {
                KillCount = 0;
                EarlyDeaths = 0;

                for (int i = 0; i < count; i++)
                {
                    while (Player.CurrentHp > 0)
                    {
                        if (Enemy.CurrentHp > 0)
                        {
                            Player.HitProgress += Player.AttackSpeed;
                            Enemy.HitProgress += Enemy.AttackSpeed;

                            if (Player.HitProgress >= 100)
                            {
                                Hit(Player, Enemy);
                            }

                            if (Enemy.HitProgress >= 100)
                            {
                                Hit(Enemy, Player);
                            }
                        }
                        else
                        {
                            ResetEnemy();
                            KillCount++;
                            Regeneration();
                        }
                    }
                    EarlyDeath();
                    ResetPlayer();
                }

                AvgKills = (float)KillCount / count;
                AvgEarlyDeaths = (float)EarlyDeaths / count;
            }
        }

        public string OneHit(Fighter one, Fighter two)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(Hit(one, two, "attacks:"));
            sb.Append(Hit(two, one, "attacks:"));

            return sb.ToString();
        }
            
        public void CalcStats()
        {
            if (Player != null && Enemy != null)
            {
                playerDodge = CalcDodge(Player.Dodge);
                playerResistance = CalcRes(Player.Resistance, Enemy.Penetration);

                Enemy.CurrentHp = Enemy.Hp;
                enemyDodge = CalcDodge(Enemy.Dodge);
                enemyResistance = CalcRes(Enemy.Resistance, Player.Penetration);

                //playerHitChance = (float)CalcHitChance(player.HitRate, enemy.Armor);
                playerCritChance = CalcCritChance(playerHitChance);
                playerHitChance = (playerHitChance - playerCritChance) * enemyDodge;
                //playerDPS = player.Damage * player.AttackSpeed * playerHitChance * enemyResistance * (1 + playerCritChance * (1 - player.CritDamage));

                enemyHitChance = (float)CalcHitChance(Enemy.HitRate, Player.Armor);
                enemyCritChance = CalcCritChance(enemyHitChance);
                enemyHitChance = (enemyHitChance - enemyCritChance) * playerDodge;
                //enemyDPS = enemy.Damage * enemy.AttackSpeed * enemyHitChance * playerResistance * (1 + enemyCritChance * (1 - enemy.CritDamage));
            }
        }

        float CalcDodge(float dogdeRate)
        {
            return (1 - (dogdeRate / (100 + dogdeRate)));
        }

        float CalcRes(float res, float pen)
        {
            if (res >= pen)
            {
                return 1 - ((res - pen) / (100 + res - pen));
            }
            else
            {
                return 1 + ((res - pen) / 100 * (-1));
            }
        }

        double CalcHitChance(double hitRate, double armor)
        {
            return (5 + Math.Pow(hitRate, (1.0 / 1.8))) / (5 + Math.Pow(armor, (1.0 / 2.0)) * 1.25);
        }

        float CalcCritChance(float hitChance)
        {
            float temp = hitChance - 1;

            if (temp <= 0)
            {
                return 0;
            }
            else
            {
                return temp;
            }
        }

        void Hit(Fighter attacker, Fighter defender)
        {
            if(randomAll.NextDouble() > Dodge.CalcRatio(defender.Dodge))
            {
                attacker.HitProgress -= 100;
            }
            else
            {
                float initDamage = (float) CalcInitDamage(attacker.Damage);

                if (attacker.PhysicalDamage)
                {
                    if (defender.CurrentManaShield > 0)
                    {
                        float manaShieldRed = Resistance.CalcRatio(defender.Resistance - attacker.Penetration);
                        float damageManaShield = initDamage * (ManaShield.AbsorbPhysical / 100);
                        initDamage = HpHit(initDamage * ((100 - ManaShield.AbsorbPhysical) / 100), 1, manaShieldRed);
                        defender.CurrentManaShield -= damageManaShield;
                    }
                    else
                    {
                        float resRed = Resistance.CalcRatio(defender.Resistance - attacker.Penetration);
                        initDamage = HpHit(initDamage, 1, resRed);
                    }

                    if (defender.CurrentArmor > 0)
                    {
                        float armorRed = Armor.CalcReduction(defender.Armor, defender.CurrentArmor);
                        float damageArmor = initDamage;
                        initDamage = HpHit(initDamage, armorRed, 1);
                        defender.CurrentArmor -= damageArmor * 0.5f;
                    }

                    defender.CurrentHp -= initDamage;
                }
                else
                {
                    if (defender.CurrentManaShield > 0)
                    {
                        float manaShieldRed = Resistance.CalcRatio(defender.Resistance * ((100 + ManaShield.ResistanceAmplify) / 100) - attacker.Penetration);
                        float damageManaShield = initDamage * (ManaShield.AbsorbMagical / 100);
                        initDamage = HpHit(initDamage * ((100 - ManaShield.AbsorbMagical) / 100), 1, manaShieldRed);
                        defender.CurrentManaShield -= damageManaShield;
                    }
                    else
                    {
                        float resRed = Resistance.CalcRatio(defender.Resistance - attacker.Penetration);
                        initDamage = HpHit(initDamage, 1, resRed);
                    }

                    if (defender.CurrentArmor > 0)
                    {
                        float armorRed = Armor.CalcReduction(defender.Armor, defender.CurrentArmor);
                        float damageArmor = initDamage;
                        initDamage = HpHit(initDamage, armorRed, 1);
                        defender.CurrentArmor -= damageArmor;
                    }

                    defender.CurrentHp -= initDamage;
                }

                attacker.HitProgress -= 100;
            }
        }

        string Hit(Fighter attacker, Fighter defender, string txt)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append($"{attacker.Name} {txt}\n");

            if (randomAll.NextDouble() > Dodge.CalcRatio(defender.Dodge))
            {
                sb.Append($"{defender.Name} dodges.\n");
                return sb.ToString();
            }
            else
            {
                float initDamage = (float) CalcInitDamage(attacker.Damage);

                sb.Append($"InitDamage: {Math.Round(initDamage, 2)}\n");

                if (attacker.PhysicalDamage)
                {
                    if (defender.CurrentManaShield > 0)
                    {
                        float manaShieldRed = Resistance.CalcRatio(defender.Resistance - attacker.Penetration);
                        float damageManaShield = initDamage * (ManaShield.AbsorbPhysical / 100);
                        initDamage = HpHit(initDamage * ((100 - ManaShield.AbsorbPhysical) / 100), 1, manaShieldRed);
                        defender.CurrentManaShield -= damageManaShield;

                        sb.Append($"Damage to ManaShield: {Math.Round(damageManaShield, 2)} ({Math.Round(defender.CurrentManaShield, 1)} left), ManaShield Red: {Math.Round(manaShieldRed, 2)}\n");
                    }
                    else
                    {
                        float resRed = Resistance.CalcRatio(defender.Resistance - attacker.Penetration);
                        initDamage = HpHit(initDamage, 1, resRed);
                        sb.Append($"Res Red: {Math.Round(resRed, 2)}\n");
                    }

                    if (defender.CurrentArmor > 0)
                    {
                        float armorRed = Armor.CalcReduction(defender.Armor, defender.CurrentArmor);
                        float damageArmor = initDamage;
                        initDamage = HpHit(initDamage, armorRed, 1);
                        defender.CurrentArmor -= damageArmor * 0.5f;

                        sb.Append($"Damage to Armor: {Math.Round(damageArmor * 0.5f, 1)} ({Math.Round(defender.CurrentArmor, 1)} left), Armor Red: {Math.Round(armorRed, 2)}\n");
                    }
                    
                    defender.CurrentHp -= initDamage;

                    sb.Append($"Damage to Hp: {Math.Round(initDamage, 1)} ({Math.Round(defender.CurrentHp, 1)} left)\n");
                    return sb.ToString();
                }
                else
                {                    
                    if (defender.CurrentManaShield > 0)
                    {
                        float manaShieldRed = Resistance.CalcRatio(defender.Resistance * ((100 + ManaShield.ResistanceAmplify) / 100) - attacker.Penetration);
                        float damageManaShield = initDamage * (ManaShield.AbsorbMagical / 100);
                        initDamage = HpHit(initDamage * ((100 - ManaShield.AbsorbMagical) / 100), 1, manaShieldRed);
                        defender.CurrentManaShield -= damageManaShield;

                        sb.Append($"Damage to ManaShield: {Math.Round(damageManaShield, 2)} ({Math.Round(defender.CurrentManaShield, 1)} left), ManaShield Red: {Math.Round(manaShieldRed, 2)}\n");
                    }
                    else
                    {
                        float resRed = Resistance.CalcRatio(defender.Resistance - attacker.Penetration);
                        initDamage = HpHit(initDamage, 1, resRed);
                        sb.Append($"Res Red: {Math.Round(resRed, 2)}\n");
                    }

                    if (defender.CurrentArmor > 0)
                    {
                        float armorRed = Armor.CalcReduction(defender.Armor, defender.CurrentArmor);
                        float damageArmor = initDamage;
                        initDamage = HpHit(initDamage, armorRed, 1);
                        defender.CurrentArmor -= damageArmor;

                        sb.Append($"Damage to Armor: {Math.Round(damageArmor, 1)} ({Math.Round(defender.CurrentArmor, 1)} left), Armor Red: {Math.Round(armorRed, 2)}\n");
                    }

                    defender.CurrentHp -= initDamage;

                    sb.Append($"Damage to Hp: {Math.Round(initDamage, 1)} ({Math.Round(defender.CurrentHp, 1)} left)\n");
                    return sb.ToString();
                }
            }
        }

        double CalcInitDamage(float damage)
        {
            return ((randomAll.NextDouble() * ((damage * 1.2) - (damage * 0.8)) + (damage * 0.8)));
        }        

        float HpHit(float damage, float armor, float res)
        {
            return damage * armor * res;
        }  

        void Regeneration()
        {
            if (Player.CurrentVit > 0)
            {
                if (Player.CurrentVit < (Player.Hp - Player.CurrentHp) * (Player.Regeneration / 100))
                {
                    Player.CurrentHp += Player.CurrentVit;
                    Player.CurrentVit = 0;
                }
                else
                {
                    if ((Player.Hp - Player.CurrentHp) <= Player.Hp * (Player.Regeneration / 100))
                    {
                        Player.CurrentVit -= (Player.Hp - Player.CurrentHp);
                        Player.CurrentHp = Player.Hp;
                    }
                    else
                    {
                        Player.CurrentVit -= Player.Hp * (Player.Regeneration / 100);
                        Player.CurrentHp += Player.Hp * (Player.Regeneration / 100);
                    }
                }
            }
        }  
        
        void ResetPlayer()
        {
            Player.CurrentHp = Player.Hp;
            Player.CurrentVit = Player.Vitality * Player.Hp;
            Player.CurrentArmor = Player.Armor;
            Player.CurrentManaShield = Player.ManaShield;
            Player.HitProgress = 0;
        }

        void ResetEnemy()
        {
            Enemy.CurrentHp = Enemy.Hp;
            Enemy.CurrentArmor = Enemy.Armor;
            Enemy.CurrentManaShield = Enemy.ManaShield;
            Enemy.HitProgress = 0;
        }

        void EarlyDeath()
        {
            if(Player.CurrentVit > 0)
            {
                EarlyDeaths++;
            }
        }
    }
}
