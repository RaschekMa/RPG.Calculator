using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfApp.Model;

namespace WpfApp
{
    public partial class MainWindow : Window
    {
        Player player;
        Enemy enemy;
        Combat combat;

        bool playerPhy;
        bool enemyPhy;

        public MainWindow()
        {
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }

        private void BtnExitClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BtnClearClick(object sender, RoutedEventArgs e)
        {
            AppConsole.Clear();            
        }

        private void BtnCreate(object sender, RoutedEventArgs e)
        {
            player = new Player("Player", Single.Parse(PStr.Text), Single.Parse(PDex.Text), Single.Parse(PInt.Text), Single.Parse(PHp.Text),
                                Single.Parse(PArmor.Text), Single.Parse(PManaShield.Text), Single.Parse(PDodge.Text), Single.Parse(PResistance.Text),
                                Single.Parse(PVitality.Text), Single.Parse(PRegeneration.Text), Single.Parse(PDamage.Text), 
                                Single.Parse(PBaseSpeed.Text), Single.Parse(PAddSpeed.Text), Single.Parse(PPenetration.Text), playerPhy);

            enemy = new Enemy("Enemy", Single.Parse(EHp.Text), Single.Parse(EArmor.Text), Single.Parse(EManaShield.Text), Single.Parse(EDodge.Text), Single.Parse(EResistance.Text), 
                                Single.Parse(EDamage.Text), Single.Parse(ESpeed.Text), Single.Parse(EPenetration.Text), enemyPhy);

            combat = new Combat(player, enemy, new Armor(Single.Parse(Armor1.Text), Single.Parse(Armor2.Text), Single.Parse(Armor3.Text)), new Dodge(Single.Parse(Dodge1.Text), Single.Parse(Dodge2.Text)), 
                     new Resistance(Single.Parse(Res1.Text), Single.Parse(Res2.Text)), new ManaShield(Single.Parse(ManaS1.Text), Single.Parse(ManaS2.Text), Single.Parse(ManaS3.Text)));

            //CHp.Text = player.Hp.ToString();
            //CArmor.Text = player.Armor.ToString();
            //CDodge.Text = player.Dodge.ToString();
            //CResistance.Text = player.Resistance.ToString();
            //CDPS.Text = (player.Damage * player.AttackSpeed).ToString();
            //CCritDamage.Text = player.CritDamage.ToString();
            //CHitRate.Text = player.HitRate.ToString();
            //CPenetration.Text = player.Penetration.ToString();

            AppConsole.Text += $"Created... \n";
            AppConsole.Text += $"Player: \n";
            AppConsole.Text += $"Hp: {player.Hp} Armor: {player.Armor} ManaShield: {player.ManaShield} Dodge: {player.Dodge} Resistance: {player.Resistance} Damage: {player.Damage} AttackSpeed: {player.AttackSpeed} Penetration: {player.Penetration}\n";
            AppConsole.Text += $"Enemy: \n";
            AppConsole.Text += $"Hp: {enemy.Hp} Armor: {enemy.Armor} ManaShield: {enemy.ManaShield} Dodge: {enemy.Dodge} Resistance: {enemy.Resistance} Damage: {enemy.Damage} AttackSpeed: {enemy.AttackSpeed} Penetration: {enemy.Penetration}\n\n";
        }

        private void BtnBattle(object sender, RoutedEventArgs e)
        {
            combat.Fight(Int32.Parse(BattleCount.Text));
            AppConsole.Text += $"AvgKills: {combat.AvgKills}\n";
            AppConsole.Text += $"AvgEarlyDeaths: {combat.AvgEarlyDeaths}\n\n";
        }

        private void BtnHit(object sender, RoutedEventArgs e)
        {            
            AppConsole.Text += combat.OneHit(combat.Player, combat.Enemy);
        }

        private void CheckEnemyPhysical(object sender, RoutedEventArgs e)
        {
            enemyPhy = true;
        }
        private void UnCheckEnemyPhysical(object sender, RoutedEventArgs e)
        {
            enemyPhy = false;
        }

        private void CheckPlayerPhysical(object sender, RoutedEventArgs e)
        {
            playerPhy = true;
        }

        private void UnCheckPlayerPhysical(object sender, RoutedEventArgs e)
        {
            playerPhy = false;
        }
    }
}
