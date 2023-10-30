using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace A105
{
    internal class Program
    {
        static int currentRowEnd;
        static string[] inventory = { "Dagger", "Empty", "Old Tattered Clothing", "Items", "0 gold" };
        static int gold = 0;
        static string[] potions = new string[10];
        static int[] playerStats = { 15, 4, 45 };
        static bool[] status = { false, false, false, false, false, false, false, false };
        static bool magic = false;
        static void Fight(string[] enemies)
        {
            Random rnd = new Random();
            int playerInitiative;
            int[] initiativeOrder = new int[enemies.Length + 1];
            int rowChoice;
            int roll;
            Console.Clear();
            Console.Write("You entered a battle with " + enemies[0]);
            for (int i = 1; i < enemies.Length; i++) { Console.Write(" and " + enemies[i]); }
            Console.WriteLine();
            Console.ReadKey(true);
            Console.Clear();
            int[] enemy1 = EnemyStats(enemies[0]);
            int[] enemy2 = { 0, 0, 0, 0 };
            int[] enemy3 = { 0, 0, 0, 0 };
            if (enemies.Length > 2) { enemy2 = EnemyStats(enemies[1]); enemy3 = EnemyStats(enemies[2]); }
            else if (enemies.Length == 2) { enemy2 = EnemyStats(enemies[1]); }

            //setting initiative
            if (status[7]) { playerInitiative = 20; }
            else if (status[6]) { playerInitiative = rnd.Next(15, 21); }
            else { playerInitiative = rnd.Next(1, 21); }

            initiativeOrder[0] = playerInitiative; initiativeOrder[1] = enemy1[3];
            if (initiativeOrder.Length == 4) { initiativeOrder[2] = enemy2[3]; initiativeOrder[3] = enemy3[3]; }
            else if (initiativeOrder.Length == 3) { initiativeOrder[2] = enemy2[3]; }
            Array.Sort(initiativeOrder); Array.Reverse(initiativeOrder);

            //setting turn order
            string[] turnOrder = new string[initiativeOrder.Length];
            bool[] characterFound = { false, false, false, false };
            for (int i = 0; i < turnOrder.Length; i++)
            {
                if (initiativeOrder[i] == playerInitiative && !characterFound[0]) { turnOrder[i] = "player"; characterFound[0] = true; }
                else if (initiativeOrder[i] == enemy1[3] && !characterFound[1]) { turnOrder[i] = "enemy1"; characterFound[1] = true; }
                else if (initiativeOrder[i] == enemy2[3] && !characterFound[2]) { turnOrder[i] = "enemy2"; characterFound[2] = true; }
                else if (initiativeOrder[i] == enemy3[3] && !characterFound[3]) { turnOrder[i] = "enemy3"; characterFound[3] = true; }
            }   

            //fighting
            bool defend = false;

            int[] enemy1Turn = new int[2];
            int[] enemy2Turn = new int[2];
            int[] enemy3Turn = new int[2];

            while (enemy1[2] > 0 || enemy2[2] > 0 || enemy3[2] > 0)
            {
                defend = false;
                if (status[6]) { roll = rnd.Next(15, 21); } else { roll = rnd.Next(1, 21); }
                Console.WriteLine("What would you like to do:");
                Console.WriteLine("  Attack");
                Console.WriteLine("  Defend");
                Console.WriteLine("  Inventory");
                currentRowEnd = 4;
                rowChoice = Menu(1, false);
                if (rowChoice == 1)
                {
                    Console.Clear();
                    Console.WriteLine("Enemies:");
                    currentRowEnd = 1;
                    for (int i = 0; i < enemies.Length; i++)
                    {
                        Console.WriteLine("  " + enemies[i]);
                        currentRowEnd += 1;
                    }
                    rowChoice = Menu(1, true);
                    if (rowChoice == -1) { Console.Clear(); continue; }
                    else if ((rowChoice == 1 && enemy1[2] < 1) || (rowChoice == 2 && enemy2[2] < 1) || (rowChoice == 3 && enemy3[2] < 1))
                    { Console.Clear(); Console.WriteLine(enemies[rowChoice - 1] + " is dead."); Console.ReadKey(true); Console.Clear(); continue; }
                }
                else if (rowChoice == 2)
                {
                    int defence;
                    Console.Clear();
                    switch (inventory[1])
                    {
                        case "Empty":
                            Console.WriteLine("You have no shield.");
                            Console.ReadKey(true);
                            Console.Clear();
                            continue;
                        default:
                            if (roll == 20 || status[7] || status[5])
                            {
                                Console.WriteLine("You readied your shield with all your might, ready to parry any attack that comes your way.");
                                break;
                            }
                            else if (roll > 14 || status[4])
                            {
                                Console.WriteLine("You readied your shield, strong enough to hold off an onslaught.");
                                break;
                            }
                            else if (roll > 9)
                            {
                                Console.WriteLine("You readied your shield.");
                                break;
                            }
                            else if (roll > 4)
                            {
                                Console.WriteLine("You readied your shield, not fully ready for an attack.");
                                break;
                            }
                            else if (roll == 1)
                            {
                                Console.WriteLine("You dropped your shield...");
                                break;
                            }
                            else
                            {
                                Console.WriteLine("You put your shield up, though your not ready for an attack.");
                                break;
                            }
                    }
                    defence = roll;
                    defend = true;
                }
                else { InventoryMenu(); continue; }

                int[] playerTurn = BattleTurn("player", playerStats);
                if (enemy1[2] > 0) { enemy1Turn = BattleTurn(enemies[0], enemy1); }
                if (enemy2[2] > 0) { enemy2Turn = BattleTurn(enemies[0], enemy2); }
                if (enemy3[2] > 0) { enemy3Turn = BattleTurn(enemies[0], enemy3); }

                Console.Clear();
                for (int i = 0; i < turnOrder.Length; i++)
                {
                    if (turnOrder[i] == "player")
                    {
                        if (!defend)
                        {
                            if (rowChoice == 1)
                            {
                                if (playerTurn[1] >= enemy1[1])
                                {
                                    Console.WriteLine("You dealt " + playerTurn[2] + " damage to " + enemies[0] + ".");
                                    enemy1[2] -= playerTurn[2];
                                    if (enemy1[2] < 1) { Console.WriteLine(enemies[0] + " died."); }
                                    else { Console.WriteLine(enemies[0] + " hp: " + enemy1[2]); }
                                }
                                else { Console.WriteLine(enemies[0] + " blocked your attack."); }
                            }
                            else if (rowChoice == 2)
                            {
                                if (playerTurn[1] >= enemy2[1])
                                {
                                    Console.WriteLine("You dealt " + playerTurn[2] + " damage to " + enemies[1] + ".");
                                    enemy2[2] -= playerTurn[2];
                                    if (enemy2[2] < 1) { Console.WriteLine(enemies[1] + " died."); }
                                    else { Console.WriteLine(enemies[1] + " hp: " + enemy2[2]); }
                                }
                                else { Console.WriteLine(enemies[0] + " blocked your attack."); }
                            }
                            else
                            {
                                if (playerTurn[1] >= enemy3[1])
                                {
                                    Console.WriteLine("You dealt " + playerTurn[2] + " damage to " + enemies[2] + ".");
                                    enemy3[2] -= playerTurn[2];
                                    if (enemy3[2] < 1) { Console.WriteLine(enemies[2] + " died."); }
                                    else { Console.WriteLine(enemies[2] + " hp: " + enemy3[2]); }
                                }
                                else { Console.WriteLine(enemies[0] + " blocked your attack."); }
                            }
                            Console.ReadKey(true);
                        }
                    }
                    else if (turnOrder[i] == "enemy1" && enemy1[2] > 0)
                    {
                        if (enemy1[0] != 3)
                        {
                            if (!defend)
                            {
                                if (enemy1Turn[1] >= playerStats[1])
                                {
                                    Console.WriteLine(enemies[0] + " dealt " + enemy1Turn[2] + " damage.");
                                    playerStats[2] -= enemy1Turn[2];
                                    if (playerStats[2] < 1) { Death(); }
                                    Console.WriteLine("hp: " + playerStats[2]);
                                }
                                else { Console.WriteLine("You blocked " + enemies[0] + "s attack."); }
                            }
                            else
                            {
                                if (enemy1Turn[1] >= playerTurn[1])
                                {
                                    Console.WriteLine(enemies[0] + " dealt " + enemy1Turn[2] + " damage.");
                                    playerStats[2] -= enemy1Turn[2];
                                    if (playerStats[2] < 1) { Death(); }
                                    Console.WriteLine("hp: " + playerStats[2]);
                                }
                                else { Console.WriteLine("You blocked " + enemies[0] + "s attack."); }
                            }
                            Console.ReadKey(true);
                        }
                    }
                    else if (turnOrder[i] == "enemy2" && enemy2[2] > 0)
                    {
                        if (enemy2[0] != 3)
                        {
                            if (!defend)
                            {
                                if (enemy2Turn[1] >= playerStats[1])
                                {
                                    Console.WriteLine(enemies[1] + " dealt " + enemy2Turn[2] + " damage.");
                                    playerStats[2] -= enemy2Turn[2];
                                    if (playerStats[2] < 1) { Death(); }
                                    Console.WriteLine("hp: " + playerStats[2]);
                                }
                                else { Console.WriteLine("You blocked " + enemies[0] + "s attack."); }
                            }
                            else
                            {
                                if (enemy2Turn[1] >= playerTurn[1])
                                {
                                    Console.WriteLine(enemies[1] + " dealt " + enemy2Turn[2] + " damage.");
                                    playerStats[2] -= enemy2Turn[2];
                                    if (playerStats[2] < 1) { Death(); }
                                    Console.WriteLine("hp: " + playerStats[2]);
                                }
                                else { Console.WriteLine("You blocked " + enemies[0] + "s attack."); }
                            }
                            Console.ReadKey(true);
                        }
                    }
                    else if (enemy3[2] > 0)
                    {
                        if (enemy3[0] != 3)
                        {
                            if (!defend)
                            {
                                if (enemy3Turn[1] >= playerStats[1])
                                {
                                    Console.WriteLine(enemies[2] + " dealt " + enemy3Turn[2] + " damage.");
                                    playerStats[2] -= enemy3Turn[2];
                                    if (playerStats[2] < 1) { Death(); }
                                    Console.WriteLine("hp: " + playerStats[2]);
                                }
                                else { Console.WriteLine("You blocked " + enemies[0] + "s attack."); }
                            }
                            else
                            {
                                if (enemy2Turn[1] >= playerTurn[1])
                                {
                                    Console.WriteLine(enemies[1] + " dealt " + enemy2Turn[2] + " damage.");
                                    playerStats[2] -= enemy2Turn[2];
                                    if (playerStats[2] < 1) { Death(); }
                                    Console.WriteLine("hp: " + playerStats[2]);
                                }
                                else { Console.WriteLine("You blocked " + enemies[0] + "s attack."); }
                            }
                            Console.ReadKey(true);
                        }
                    }
                }
                Console.Clear();
                ImportantActionPlayer();
                if (playerStats[2] < 1) { Death(); }

            }
            Console.WriteLine("You won!");
            Console.ReadKey(true);
            Console.Clear();


        }
        static int[] EnemyStats(string monster)
        {
            Random rnd = new Random();
            int[] enemyStats = new int[4];
            //string[] enemyCatalogue = { "Goblin", "Slime", "Eldritch King" };
            switch (monster)
            {
                case "Goblin":
                    enemyStats[0] = rnd.Next(4, 9);
                    enemyStats[1] = rnd.Next(2, 7);
                    enemyStats[2] = rnd.Next(10, 16);
                    enemyStats[3] = rnd.Next(1, 21);
                    break;
                case "Slime":
                    enemyStats[0] = rnd.Next(3, 8);
                    enemyStats[1] = rnd.Next(0, 5);
                    enemyStats[2] = rnd.Next(5, 7);
                    enemyStats[3] = rnd.Next(1, 21);
                    break;
                case "Eldritch King":
                    enemyStats[0] = rnd.Next(10, 12);
                    enemyStats[1] = rnd.Next(1, 4);
                    enemyStats[2] = rnd.Next(50, 60);
                    enemyStats[3] = rnd.Next(1, 21);
                    break;
                case "Bear":
                    enemyStats[0] = rnd.Next(12, 16);
                    enemyStats[1] = rnd.Next(1, 4);
                    enemyStats[2] = rnd.Next(15, 26);
                    enemyStats[3] = rnd.Next(1, 21);
                    break;
            }
            return enemyStats;
        }
        static int[] BattleTurn(string character, int[] stats)
        {
            Random rnd = new Random();
            int roll = rnd.Next(1, 21);
            int[] attack = new int[3];

            //string[] enemyCatalogue = { "Goblin", "Slime", "Eldritch King" };
            switch (character)
            {
                case "player":
                    if (roll > 10) { attack[0] = 1; } else { attack[0] = 2; }
                    roll = rnd.Next(1, 21);
                    if (roll == 20) { attack[1] = (int)Math.Ceiling(stats[0] * 1.5); }
                    else if (roll > 14) { attack[1] = (int)Math.Ceiling(stats[0] * 1.2); }
                    else if (roll > 4) { attack[1] = stats[0]; }
                    else if (roll == 1) { attack[1] = (int)Math.Floor((double)(stats[0] / 2)); }
                    else { attack[1] = (int)Math.Floor((double)(stats[0] * 0.8)); }
                    attack[2] = WeaponList(inventory[0], false) + rnd.Next(-1, 2); 
                    break;
                case "Goblin":
                    if (roll > 10) { attack[0] = 1; } else { attack[0] = 2; }
                    roll = rnd.Next(1, 21);
                    if (roll == 20) { attack[1] = (int)Math.Ceiling(stats[0] * 1.5); }
                    else if (roll > 14) { attack[1] = (int)Math.Ceiling(stats[0] * 1.2); }
                    else if (roll > 4) { attack[1] = stats[0]; }
                    else if (roll == 1) { attack[1] = (int)Math.Floor((double)(stats[0] / 2)); }
                    else { attack[1] = (int)Math.Floor((double)(stats[0] * 0.8)); }
                    attack[2] = rnd.Next(3, 6);
                    break;
                case "Slime":
                    if (roll > 10)
                    {
                        attack[0] = 1;
                        if (roll > 10) { attack[0] = 1; } else { attack[0] = 2; }
                        roll = rnd.Next(1, 21);
                        if (roll == 20) { attack[1] = (int)Math.Ceiling(stats[0] * 1.5); }
                        else if (roll > 14) { attack[1] = (int)Math.Ceiling(stats[0] * 1.2); }
                        else if (roll > 4) { attack[1] = stats[0]; }
                        else if (roll == 1) { attack[1] = (int)Math.Floor((double)(stats[0] / 2)); }
                        else { attack[1] = (int)Math.Floor((double)(stats[0] * 0.8)); }
                        attack[2] = rnd.Next(5, 8);
                    }
                    else
                    {
                        attack[0] = 2;
                        if (roll > 10) { attack[0] = 1; } else { attack[0] = 2; }
                        roll = rnd.Next(1, 21);
                        if (roll == 20) { attack[1] = (int)Math.Ceiling(stats[0] * 1.5); }
                        else if (roll > 14) { attack[1] = (int)Math.Ceiling(stats[0] * 1.2); }
                        else if (roll > 4) { attack[1] = stats[0]; }
                        else if (roll == 1) { attack[1] = (int)Math.Floor((double)(stats[0] / 2)); }
                        else { attack[1] = (int)Math.Floor((double)(stats[0] * 0.8)); }
                        attack[2] = rnd.Next(1, 4);
                    }
                    break;
                case "Eldritch King":
                    attack[0] = 1;
                    attack[1] = rnd.Next(1, 21);
                    attack[2] = rnd.Next(7, 9);
                    break;
                case "Bear":
                    attack[0] = 1;
                    attack[1] = rnd.Next(1, 21);
                    attack[2] = rnd.Next(5, 9);
                    break;
            }
            return attack;
        }
        static void EnemyAttack(int damage, bool physical)
        {
            //goblin is throwing gold
        }
        static int Menu(int min, bool backPossible)
        {
            int rowPos = min;
            Console.SetCursorPosition(0, rowPos);
            Console.Write(">");
            while (true)
            {
                ConsoleKeyInfo key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.DownArrow)
                {
                    Console.SetCursorPosition(0, rowPos);
                    Console.Write(" ");
                    if (rowPos > currentRowEnd - 2) { rowPos = min; }
                    else { rowPos++; }
                    Console.SetCursorPosition(0, rowPos);
                    Console.Write(">");
                }
                else if (key.Key == ConsoleKey.UpArrow)
                {
                    Console.SetCursorPosition(0, rowPos);
                    Console.Write(" ");
                    if (rowPos < min + 1) { rowPos = currentRowEnd - 1; }
                    else { rowPos--; }
                    Console.SetCursorPosition(0, rowPos);
                    Console.Write(">");
                }
                else if (key.Key == ConsoleKey.LeftArrow && backPossible)
                {
                    return -1;
                }
                else if (key.Key == ConsoleKey.Enter) { break; }
            }
            return rowPos;
        }
        static void InventoryMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Inventory:");
                currentRowEnd = inventory.Length + 1;

                Console.WriteLine("  Weapon:    " + inventory[0]);
                Console.WriteLine("  Shield:    " + inventory[1]);
                Console.WriteLine("  Armor:     " + inventory[2]);
                Console.WriteLine("  Items:     " + inventory[3]);
                Console.WriteLine("  Gold:      " + inventory[4]);



                int rowPos = Menu(1, true);
                if (rowPos == 1) { WeaponList(inventory[0], true); }
                else if (rowPos == 2) { }
                else if (rowPos == 4) { ItemMenu(); }
                if (rowPos == -1) { Console.Clear(); currentRowEnd = 0; break; }
            }
        }
        static string GeneratePotion(string[] options)
        {
            Random rnd = new Random();
            return options[rnd.Next(0, options.Length)];
        }
        static void ItemMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Potions:");
                currentRowEnd = potions.Length + 1;
                for (int i = 0; i < potions.Length; i++)
                {
                    Console.WriteLine("  " + potions[i]);
                }
                int rowPos = Menu(1, true);
                for (int i = 0; i < potions.Length; i++)
                {
                    if (rowPos == i + 1 && potions[i] != "Empty")
                    {
                        potions[i] = UsePotion(potions[i]);
                        if (potions[i] == "Empty") { for (int j = i; j < potions.Length - 1; j++) { potions[j] = potions[j + 1]; } potions[9] = "Empty"; }
                        break;
                    }
                }
                if (rowPos == -1) { break; }
            }
        }
        static string UsePotion(string potion)
        {
            Random rnd = new Random();
            int roll;
            if (status[6]) { roll = rnd.Next(15, 21); }
            else { roll = rnd.Next(1, 21); }
            string potionType;
            Console.Clear();
            //string[] potionCatalogue = { "Health Elixir", "Venomous Vial", "Strength Tonic", "Shadow Essence", "Explosive Flask", "Fortune Elixer" };
            if (potion[0] == 'S') { potionType = potion.Substring(7); }
            else { potionType = potion.Substring(5); }
            switch (potionType[0].ToString() + potionType[1])
            {
                case "He": Console.WriteLine("Restores a portion of the consumers health and cures all poisons, useful in a pinch."); break;
                case "Ve": Console.WriteLine("Unleashes a poison that slowly weakens and kills someone at just the touch of the venom, useful to kill enemies without begining a deadly duel."); break;
                case "St": Console.WriteLine("Temporarily enhances the consumers raw strength, useful against strong enemies."); break;
                case "Sh": Console.WriteLine("Temporarily decreases all noise emitted around the consumer, useful for sneaking around."); break;
                case "Ex": Console.WriteLine("Upon breaking, releases a small yet strong explosion, useful for \"ranged\" warriors."); break;
                case "Fo": Console.WriteLine("Temporarily increases the chance of success in a fight, according to the d20 theory."); break;
            }
            if (potion[0] == 'S') { Console.WriteLine("This one seems to have a rather strong effect"); }
            else { Console.WriteLine("This one seems to have a rather weak effect"); }
            Console.WriteLine("Would you like to use it? (y/n)");
            if (YesOrNo() == 'y')
            {
                switch (potionType[0].ToString() + potionType[1])
                {
                    case "He":
                        if (potion[0] == 'S')
                        {
                            if (roll == 20 || status[7]) { playerStats[2] += 25; Console.WriteLine("You healed 25 hp."); }
                            else if (roll == 1) { Console.WriteLine("You dropped the potion..."); }
                            else { playerStats[2] += 20; Console.WriteLine("You healed 20 hp."); }
                        }
                        else
                        {
                            if (roll == 20 || status[7]) { playerStats[2] += 15; Console.WriteLine("You healed 15 hp."); }
                            else if (roll == 1) { Console.WriteLine("You dropped the potion..."); }
                            else { playerStats[2] += 10; Console.WriteLine("You healed 10 hp."); }
                        }
                        if (roll != 1 && status[0]) { Console.WriteLine("You were cured of the poison."); status[0] = false; status[1] = false; }
                        if (playerStats[2] > 30) { playerStats[2] = 30; }
                        Console.WriteLine("hp: " + playerStats[2]);
                        break;
                    case "Ve":
                        Console.WriteLine("You went to drink the poison...");
                        if (roll == 20 || status[7]) { Console.WriteLine("Lukily, you dropped the potion."); break; }
                        else if (potion[0] == 'S')
                        {
                            if (roll == 1) { Console.WriteLine("It had a very strong effect!"); playerStats[2] -= 5; Console.WriteLine("hp: " + playerStats[2]); }
                            else { Console.WriteLine("It had a strong effect."); }
                            status[1] = true;
                        }
                        else
                        {
                            if (roll == 1) { Console.WriteLine("It had a strong effect!"); status[1] = true; }
                            else { Console.WriteLine("It had a weak effect."); }
                        }
                        status[0] = true;
                        break;
                    case "St": Console.WriteLine("You feel significantly stronger."); status[4] = true; status[5] = true; break;
                    case "Sh": Console.WriteLine("You feel significantly sneakier."); status[2] = true; status[3] = true; break;
                    case "Ex": Console.WriteLine("You can't use that yet."); break;
                    case "Fo": Console.WriteLine("You feel significantly luckier."); status[6] = true; status[7] = true; break;
                }

                Console.ReadKey(true);
                return "Empty";
            }
            else { return potion; }
        }
        static int RepeatedOption(int numOfOption, string option)
        {
            Console.Clear();

            Console.WriteLine(option + "s:");
            for (int i = 0; i < numOfOption; i++)
            {
                Console.WriteLine("  " + option + " " + (i + 1));
            }
            currentRowEnd = numOfOption + 1;
            return Menu(1, true);
        }
        static bool Chest(string item, string type)
        {
            Console.Clear();
            Console.WriteLine("You opened the chest and found... " + item);
            switch (type)
            {
                case "weapon": return AddWeapon(item);
                case "shield": return AddShield(item);
                case "potion": return AddItem(item);
                case "armor": return AddArmor(item);
                case "gold": return AddGold(item);
            }
            Console.Clear();
            Console.WriteLine("something went wrong in Chest()");
            return false;
        }
        static bool AddWeapon(string weapon)
        {
            Console.WriteLine("Would you like to equip " + weapon + "? (y/n)");
            if (YesOrNo() == 'y') { Console.WriteLine("You equipped " + weapon + ", and threw away " + inventory[0]); inventory[0] = weapon; Console.ReadKey(true); Console.Clear(); return true; }
            else { Console.WriteLine("You left " + weapon + " in the chest."); Console.ReadKey(true); Console.Clear(); return false; }
        }
        static bool AddShield(string shield)
        {
            Console.WriteLine("Would you like to equip " + shield + "? (y/n)");
            if (YesOrNo() == 'y') 
            {
                if (inventory[1] == "Empty") { Console.WriteLine("You equipped " + shield); }
                else { Console.WriteLine("You equipped " + shield + ", and threw away " + inventory[1]); }
                inventory[1] = shield; Console.ReadKey(true); 
                Console.Clear(); 
                currentRowEnd = 0; 
                return true; 
            }
            else { Console.WriteLine("You left " + shield + "."); Console.ReadKey(true); Console.Clear(); currentRowEnd = 0; return false; }
        }
        static bool AddItem(string potion)
        {
            Console.WriteLine("You added " + potion + " to item inventory.");
            for (int i = 0; i < potions.Length; i++)
            {
                if (potions[i] == "Empty") { potions[i] = potion; break; }
            }
            Console.ReadKey(true); Console.Clear(); currentRowEnd = 0;
            return true;
        }
        static bool AddArmor(string armor)
        {
            Console.WriteLine("Would you like to equip " + armor + "? (y/n)");
            if (YesOrNo() == 'y') { Console.WriteLine("You equipped " + armor + ", and threw away " + inventory[2]); inventory[2] = armor; Console.ReadKey(true); Console.Clear(); return true; }
            else { Console.WriteLine("You left " + armor + " in the chest."); Console.ReadKey(true); Console.Clear(); return false; }
        }
        static bool AddGold(string money)
        {
            Console.WriteLine("You added " + money + " gold to your inventory");
            gold += int.Parse(money);
            inventory[4] = gold.ToString();
            Console.ReadKey(true);
            return true;
        }
        static char YesOrNo()
        {
            ConsoleKeyInfo option = Console.ReadKey(true);
            if (option.Key == ConsoleKey.Y) { return 'y'; }
            return 'n';
        }
        static string RoomFetcher(string[] route, int currentStep)
        {
            switch (route[0])
            {
                case "0": return Room0().ToString();
                case "1":
                    switch (route[1])
                    {
                        case "0": return Room1().ToString();
                        case "1": currentStep -= 1; return Room11(route).ToString();
                        default:
                            switch (route[2])
                            {
                                case "0": return Room12().ToString();
                                case "1":
                                    switch (route[3])
                                    {
                                        case "0": return Room121().ToString();
                                        default: return Room1211().ToString();
                                    }
                                default:
                                    switch (route[3])
                                    {
                                        case "0": return Room122().ToString();
                                        default:
                                            switch (route[4])
                                            {
                                                case "1": return Room1211().ToString();
                                                default: return Room1221().ToString();
                                            }

                                    }
                            }
                    }
                case "2": return Room2().ToString();
                default:
                    switch (route[1])
                    {
                        case "0": return Room3().ToString();
                        default:
                            switch (route[2])
                            {
                                case "0": return Room31().ToString();
                                case "1": 
                                    switch (route[3])
                                    {
                                        case "0": return Room311().ToString();
                                        default: 
                                            switch (route[4])
                                            {
                                                case "0": return Room3111().ToString();
                                                default: currentStep -= 1; return Room31111(route).ToString();
                                            }
                                    }
                                case "2":
                                    switch (route[3])
                                    {
                                        case "0": return Room312().ToString();
                                        case "1": return Room3121().ToString();
                                        default:
                                            switch (route[4])
                                            {
                                                case "0": return Room3122().ToString();
                                                default: 
                                                    switch (route[4])
                                                    {
                                                        case "0": return Room31222().ToString();
                                                        default: currentStep = 0; return Room312221(route).ToString();
                                                    }
                                            }
                                    }
                                default: return "0";
                            }
                    }
            }
        }
        static int Room0()
        {
            bool chestOpen = false;
            int rowChoice;
            while (true)
            {
                if (!chestOpen) { Console.WriteLine("You can see 3 paths each with an image carved into the enourmous vines surrounding them, a chest, and a bright light coming from behind you."); }
                else { Console.WriteLine("You can see 3 paths each with an image carved into the enourmous vines surrounding them, an opened chest, and a bright light coming from behind you."); }
                Console.WriteLine("What would you like to inspect?");
                Console.WriteLine("  Paths");
                if (!chestOpen) { Console.WriteLine("  Chest"); }
                Console.WriteLine("  Bright Light");
                Console.WriteLine("  Inventory");
                if (!chestOpen) { currentRowEnd += 6; rowChoice = Menu(currentRowEnd - 4, false); }
                else { currentRowEnd += 5; rowChoice = Menu(currentRowEnd - 3, false); }
                if (!chestOpen)
                {
                    if (rowChoice == currentRowEnd - 4)
                    {
                        rowChoice = RepeatedOption(3, "Path");
                        if (rowChoice != -1) { return rowChoice; }
                        Console.Clear();
                        currentRowEnd = 0;
                    }
                    else if (rowChoice == currentRowEnd - 3) { chestOpen = Chest("Weak Health Elixir", "potion"); }
                    else if (rowChoice == currentRowEnd - 2) { Console.Clear(); Console.WriteLine("You decided to go back, after all the journeys better than the destination."); Console.ReadKey(true); System.Environment.Exit(0); return 4; }
                    else { InventoryMenu(); }
                }
                else
                {
                    if (rowChoice == currentRowEnd - 3)
                    {
                        rowChoice = RepeatedOption(3, "Path");
                        if (rowChoice != -1) { return rowChoice; }
                    }
                    else if (rowChoice == currentRowEnd - 2) { Console.Clear(); Console.WriteLine("You decided to go back, after all the journeys better than the destination."); Console.ReadKey(true); System.Environment.Exit(0); return 4; }
                    else { InventoryMenu(); }
                }
            }

        }
        static int Room1()
        {
            int rowChoice;
            bool bodyLooted = false;
            while (true)
            {
                Console.WriteLine("You find yourself in a small area, likely an old guards post, considering the skeleton in the corner.");
                Console.WriteLine("There are two paths, one which leads into a small building, and another which continues through the grove.");
                Console.WriteLine("  Paths");
                if (!bodyLooted) { Console.WriteLine("  Dead body"); currentRowEnd += 5; }
                else { currentRowEnd += 4; }
                Console.WriteLine("  Inventory");
                rowChoice = Menu(2, false);
                if (rowChoice == 2) { rowChoice = RepeatedOption(2, "Path"); if (rowChoice != -1) { return rowChoice; } }
                else if (!bodyLooted)
                {
                    if (rowChoice == 3) { bodyLooted = Chest("Old Wooden Shield", "shield"); }
                    else if (rowChoice == 4) { InventoryMenu(); }
                }
                else { InventoryMenu(); }
            }
        }
        static int Room11(string[] route)
        {
            int rowChoice;
            bool gotBottle = false;
            Random rnd = new Random();
            int roll;
            if (status[6]) { roll = rnd.Next(15, 21); } else { roll = rnd.Next(1, 21); }
            Console.WriteLine("You walked into the building, its pitch black...");
            Console.WriteLine("*SHHHHRRRRKKK*");
            Console.Write("Several spikes sprung up from the ground! ");
            Console.ReadKey(true);
            if (roll < 15) { Console.WriteLine("Unfortunately, one went right through your heart."); Console.WriteLine("Within seconds, You died."); Console.ReadKey(true); System.Environment.Exit(0); }
            else if (roll == 20 || status[7]) { Console.WriteLine("Somehow, you were abe to jump out of the way unscathed."); }
            else if (roll > 14)
            {
                Console.WriteLine("Luckily, the spikes were so old they had turned blunt, dealing minimal damage.");
                playerStats[2] -= 5;
                Console.WriteLine("hp: " + playerStats[2]);
            }
            Console.ReadKey(true);
            string potion = GeneratePotion(new string[] { "Strong Strength Tonic", "Strong Shadow Essence", "Strong Explosive flask", "Strong Fortune Elixer" });
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Of what you can make out, there is a bottle on the ground close to you and an exit to the room.");
                Console.WriteLine("  Exit");
                if (!gotBottle) { Console.WriteLine("  Bottle"); currentRowEnd = 5; }
                else { currentRowEnd = 4; }
                Console.WriteLine("  Explore dark room further");
                Console.WriteLine("  Inventory");
                rowChoice = Menu(1, false);
                if (rowChoice == 1) { route[1] = "2"; return 2; }
                else if (!gotBottle)
                {
                    if (rowChoice == 2) { Console.Clear(); AddItem(potion); gotBottle = true; }
                    else if (rowChoice == 3)
                    {
                        Console.Clear();
                        Console.WriteLine("While exploring the trapped room further, another trap went off! More spikes came up from the ground and pierced your heart");
                        Console.WriteLine("You died.");
                        Console.ReadKey(true);
                        System.Environment.Exit(0);
                    }
                    else { InventoryMenu(); }
                }
                else if (rowChoice == 2)
                {
                    Console.Clear();
                    Console.WriteLine("While exploring the trapped room further, another trap went off! More spikes came up from the ground and pierced your heart");
                    Console.WriteLine("You died.");
                    Console.ReadKey(true);
                    System.Environment.Exit(0);
                }
                else { InventoryMenu(); }

            }
        }
        static int Room12()
        {
            bool fought = false;
            bool bodyLooted = false;
            int rowChoice;
            Random rnd = new Random();
            Console.WriteLine("As you walk forward, you can hear faint footsteps ahead.");
            Console.WriteLine("It looks like its time for some action.");
            Console.ReadKey(true);
            Console.Clear();
            while (!fought)
            {
                Console.WriteLine("In the centre of the room you can see a small goblin. It looks relatively weak, but fast.");
                Console.WriteLine("You can also see two paths behind it, you can probably take the second without being spotted, though the other seems harder.");
                Console.WriteLine("  Fight");
                Console.WriteLine("  Sneak past");
                Console.WriteLine("  Inventory");
                currentRowEnd = 5;
                rowChoice = Menu(2, false);

                if (rowChoice == 2) { Fight(new string[] { "Goblin" }); fought = true; }
                else if (rowChoice == 3)
                {
                    int roll = rnd.Next(1, 21);
                    rowChoice = RepeatedOption(2, "Path");
                    Console.Clear();
                    if (rowChoice == 1)
                    {
                        if (roll > 14 || status[6] || status[2]) { Console.WriteLine("You snuk past the goblin and went through the first path."); Console.ReadKey(true); return rowChoice; }
                        else { Console.WriteLine("You got caught!"); Console.ReadKey(true); Console.Clear(); Fight(new string[] { "Goblin" }); fought = true; }
                    }
                    else if (rowChoice == 2)
                    {
                        if (roll > 7 || status[6] || status[2]) { Console.WriteLine("You snuk past the goblin and went through the second path."); Console.ReadKey(true); return rowChoice; }
                        else { Console.WriteLine("You tripped and got caught!"); Console.ReadKey(true); Console.Clear(); Fight(new string[] { "Goblin" }); fought = true; }
                    }
                }
                else { InventoryMenu(); }
            }
            while (true)
            {
                Console.Clear();
                Console.WriteLine("In the centre of the room lies the dead goblin, behind it are two paths.");
                Console.WriteLine("  Paths");
                currentRowEnd = 3;
                if (!bodyLooted) { Console.WriteLine("  Goblin"); currentRowEnd = 4; }
                Console.WriteLine("  Inventory");
                rowChoice = Menu(1, false);

                if (rowChoice == 1) { rowChoice = RepeatedOption(2, "Path"); if (rowChoice != -1) { return rowChoice; } }
                else if (rowChoice == 2 && !bodyLooted)
                {
                    Console.Clear();
                    Console.WriteLine("You inspected the body and found 10 gold.");
                    Console.ReadKey(true);
                    inventory[4] = "10";
                    bodyLooted = true;
                }
                else { InventoryMenu(); }


            }
        }
        static int Room121()
        {
            Random rnd = new Random();
            int rowChoice;
            bool fought = false;
            bool bodyLooted = false;
            bool[] chestsOpened = { false, false, false };
            bool allChestsOpened = false;
            int clawsClipped = 0;
            Console.WriteLine("You slowly approached a sound of rumblind thunder, mixed with the rhythmic vibrations of a distant jackhammer.");
            Console.WriteLine("Its hard to say what lies ahead, but it cannot be good.");
            Console.Clear();
            Console.Write("As you arrive at the end of the passageway, y");
            currentRowEnd = 1;
            while (!fought)
            {
                if (currentRowEnd == 0) { Console.Write("Y"); }
                Console.WriteLine("ou see a giant sleeping bear-like creature, guarding at least two golden chests. There is only one exit.");
                Console.WriteLine("You might be able to sneak past but that creature would likely hear the chests opening.");
                Console.WriteLine("  Fight");
                Console.WriteLine("  Sneak past");
                Console.WriteLine("  Inventory");
                currentRowEnd = 5;
                rowChoice = Menu(2, false);

                if (rowChoice == 2) 
                {
                    Fight(new string[] { "Bear" });
                    fought = true; 
                }
                else if (rowChoice == 3)
                {
                    Console.Clear();
                    int roll = rnd.Next(1, 21);
                    if (roll == 15 || status[6] || status[2])
                    {
                        Console.WriteLine("You managed to sneak past the bear-like creature, and went through the exit.");
                        Console.ReadKey(true);
                        return 1;
                    }
                    else if (roll > 5)
                    {
                        Console.WriteLine("Despite sneaking well, the creature awoke!");
                        fought = true;
                    }
                    else { Console.WriteLine("You tripped and fell on the creature..."); Console.WriteLine("Unsurprisingly it awoke."); fought = true; }
                }
                else { InventoryMenu(); }
                Console.Clear();
                currentRowEnd = 0;
            }
            while (true)
            {
                currentRowEnd = 0;
                Console.WriteLine("On the right of the room, lies the slain creature.");
                Console.WriteLine("There are the two chests you saw before, and another the creature was covering, there is one exit.");
                Console.WriteLine("  Exit");
                if (!allChestsOpened) { Console.WriteLine("  Chests"); currentRowEnd += 6; }
                else { currentRowEnd += 5; }
                if (!bodyLooted) { Console.WriteLine("  Slain beast"); }
                else { currentRowEnd -= 1; }
                Console.WriteLine("  Inventory");
                rowChoice = Menu(2, false);

                if (rowChoice == 2) { return 1; }
                else if (!allChestsOpened)
                {
                    if (rowChoice == 3)
                    {
                        Console.Clear();
                        Console.WriteLine("Chests:");
                        for (int i = 0; i < chestsOpened.Length; i++) { Console.WriteLine("  Chest " + (i + 1)); }
                        currentRowEnd = 4;
                        rowChoice = Menu(1, true);
                        if (rowChoice == 1 && !chestsOpened[0]) { if (Chest("Silver Blade of the Eldritch", "weapon")) { chestsOpened[0] = true; } }
                        else if (rowChoice == 2 && !chestsOpened[1]) { if (Chest("Golden Royal Eldritch Armor", "armor")) { chestsOpened[1] = true; } }
                        else if (rowChoice == 3 && !chestsOpened[2]) { if (Chest("Strong Strength Tonic", "potion")) { chestsOpened[2] = true; } }
                        else if (rowChoice == -1) { Console.Clear(); }
                        else { Console.Clear(); Console.WriteLine("This chest has already been opened."); Console.ReadKey(true); Console.Clear(); }
                        if (chestsOpened[0] && chestsOpened[1] && chestsOpened[2]) { allChestsOpened = true; }
                    }
                    else if (rowChoice == 4 && !bodyLooted)
                    {
                        Console.Clear();
                        Console.WriteLine("You inspected the body, you could probably cut its claws off.");
                        Console.WriteLine("Would you like to cut its claws off (y/n)");
                        if (YesOrNo() == 'y')
                        {
                            AddItem("Giant Claw");
                            clawsClipped += 1;
                        }
                        else { Console.Clear(); }
                    }
                    else { InventoryMenu(); }
                }
                else if (rowChoice == 3 && !bodyLooted)
                {
                    Console.Clear();
                    if (clawsClipped < 10)
                    {
                        Console.WriteLine("You inspected the body, you could probably cut its claws off.");
                        Console.WriteLine("Would you like to cut its claws off (y/n)");
                        if (YesOrNo() == 'y')
                        {
                            AddItem("Giant Claw");
                            clawsClipped += 1;
                        }
                        else { Console.Clear(); }
                    }
                    else { Console.WriteLine("You inspected the body, there's nothing helpful you can get from it."); }
                }
                else { InventoryMenu(); }
            }
        }
        static int Room1211()
        {
            Console.WriteLine("As you approach the thunderous noise you heard earlier, a strange feeling overcomes you.");
            Console.WriteLine("\"This is it.\" After searching for so long, you've finally reached what must be the end.");
            Console.ReadKey(true);
            Console.WriteLine();
            Console.WriteLine("You entered a large, old stone building. You can see the silouette of a man, sitting on a throne.");
            Console.WriteLine("\"You who have entered, and slaughtered many, I assure you that I am stronger than you can comprehend.\"");
            Console.WriteLine("\"Leave now and I will spare you your life\"");
            Console.ReadKey(true);
            Console.WriteLine();
            Console.WriteLine("But you know you cannot leave now, right on the cusp of the ancient eldritch magic you so desire.");
            Console.ReadKey();
            Console.Clear();
            Fight(new string[] { "Eldritch King" });
            Console.WriteLine("You win. Ending 1/6");
            Console.ReadKey(true);
            System.Environment.Exit(0);
            return 0;
        }
        static int Room122()
        {
            bool[] chestsOpened = { false, false };
            Random rnd = new Random();
            while (true)
            {
                Console.WriteLine("You walk across a narrow path, with massive vines as walls to either side.");
                Console.WriteLine("Just above the exit is a picture of a skull like at the beginning, but bloodied.");
                Console.WriteLine("You see two old copper chests along the way.");
                Console.WriteLine("  Exit");
                if (!chestsOpened[0] || !chestsOpened[1]) { Console.WriteLine("  Chests"); currentRowEnd = 6; }
                else { currentRowEnd = 5; }
                Console.WriteLine("  Inventory");
                int rowChoice = Menu(3, false);
                if (rowChoice == 3) { return 1; }
                else if (rowChoice == 4 && !chestsOpened[0] || !chestsOpened[1])
                {
                    Console.Clear();
                    rowChoice = RepeatedOption(2, "Chest");
                    if (rowChoice == 1 && !chestsOpened[0]) { if (Chest("Weak Explosive Flask", "potion")) { chestsOpened[0] = true; } }
                    else if (rowChoice == 2 && !chestsOpened[1])
                    {
                        Console.Clear();
                        Console.WriteLine("You opened the chest and found... An explosion!");
                        int roll;
                        if (status[6]) { roll = rnd.Next(15, 21); }
                        else { roll = rnd.Next(1, 21); }
                        if (roll == 20 || status[7])
                        {
                            Console.WriteLine("But it dealt no damage...");
                            Console.WriteLine("Actually, it seemed to heal you");
                            Console.ReadKey(true);
                            playerStats[2] += 5;
                            Console.WriteLine("hp: " + playerStats[2]);
                        }
                        else if (roll > 14)
                        {
                            Console.WriteLine("It dealt little damage");
                            Console.ReadKey(true);
                            playerStats[2] -= 5;
                            if (playerStats[2] < 1) { Death(); }
                            Console.WriteLine("hp: " + playerStats[2]);
                        }
                        else if (roll > 9)
                        {
                            Console.WriteLine("It dealt a bit of damage.");
                            Console.ReadKey(true);
                            playerStats[2] -= 8;
                            if (playerStats[2] < 1) { Death(); }
                            Console.WriteLine("hp: " + playerStats[2]);
                        }
                        else if (roll > 4)
                        {
                            Console.WriteLine("It dealt some of damage.");
                            Console.ReadKey(true);
                            playerStats[2] -= 10;
                            if (playerStats[2] < 1) { Death(); }
                            Console.WriteLine("hp: " + playerStats[2]);
                        }
                        else if (roll == 1)
                        {
                            Console.WriteLine("It dealt massive damage!");
                            Console.ReadKey(true);
                            playerStats[2] -= 18;
                            if (playerStats[2] < 1) { Death(); }
                            Console.WriteLine("hp: " + playerStats[2]);
                        }
                        else
                        {
                            Console.WriteLine("It dealt a lot of damage!");
                            Console.ReadKey(true);
                            playerStats[2] -= 12;
                            if (playerStats[2] < 1) { Death(); }
                            Console.WriteLine("hp: " + playerStats[2]);
                        }
                        chestsOpened[1] = true;
                    }
                    else if (rowChoice == -1) { Console.Clear(); }
                    else { Console.Clear(); Console.WriteLine("This chest has already been opened."); Console.ReadKey(true); Console.Clear(); }
                }
                else { InventoryMenu(); }
            }

        }
        static int Room1221()
        {
            Random rnd = new Random();
            int roll;
            int rowChoice = 0;
            bool enterFight = false;
            bool fought = false;
            Console.WriteLine("You approach the outside of some kind of old but grand castle.");
            Console.WriteLine("There doesn't seem to be anything beyond it, at least from what you can tell at this angle.");
            Console.WriteLine("This must be the end.");
            Console.ReadKey(true);
            Console.Clear();
            while (true)
            {
                if (!enterFight)
                {
                    Console.WriteLine("The entrance appears to be guarded by three lifeless slimy creatures.");
                    Console.WriteLine("  Fight");
                    Console.WriteLine("  Sneak past");
                    Console.WriteLine("  Inventory");
                    currentRowEnd = 4;
                    rowChoice = Menu(1, false);
                }
                if (rowChoice == 1 || enterFight)
                {
                    Console.Clear();
                    Fight(new string[] { "Slime", "Slime", "Slime" });
                    Console.WriteLine("You defeated them.");
                    AddWeapon("Bronze Blade of the Eldritch");
                    AddArmor("Silver Guards Eldritch Armor");
                    AddShield("Silver Guards Edlritch Shield");
                    AddItem("Slime");
                    enterFight = false;
                    fought = true;
                }
                else if (rowChoice == 2)
                {
                    Console.Clear();
                    if (status[6]) { roll = rnd.Next(15, 21); } else { roll = rnd.Next(1, 21); }
                    if (roll == 20 || status[7] || status[3]) { Console.WriteLine("You managed to sneak past and entere the castle"); return 1; }
                    else { Console.WriteLine("You got caught!"); Console.ReadKey(true); enterFight = true; }
                }
                else { InventoryMenu(); }
                while (fought)
                {
                    Console.Clear();
                    Console.WriteLine("More seem to be appearing from the ground, if you dont go ahead now you may have to fight more!");
                    Console.WriteLine("  Enter");
                    Console.WriteLine("  Wait");
                    Console.WriteLine("  Inventory");
                    currentRowEnd = 4;
                    rowChoice = Menu(1, false);
                    if (rowChoice == 1) { return 1; }
                    else if (rowChoice == 2) { fought = false; Console.Clear(); }
                    else { InventoryMenu(); }
                }
            }
        }
        static int Room2()
        {
            Console.WriteLine("As you walked forward a storm suddenly appeared and zapped you with lightning.");
            Console.ReadKey(true);
            Console.WriteLine("You died.");
            Console.ReadKey(true);
            System.Environment.Exit(0);
            return 2;
        }
        static int Room3()
        {
            int rowChoice;
            bool goldLooted = false;
            while (true)
            {
                Console.WriteLine("You find yourself in a desolate area, no life or shrubbery, besides the giant vines to the sides acting as walls.");
                Console.WriteLine("In the corner furthest from you, you can see something shiny. There is also an exit.");
                Console.WriteLine("  Exit");
                if (!goldLooted) { Console.WriteLine("  Shiny corner"); currentRowEnd = 5; } else { currentRowEnd = 4; }
                Console.WriteLine("  Inventory");
                rowChoice = Menu(2, false);

                if (rowChoice == 2) { return 1; }
                else if (!goldLooted)
                {
                    Console.Clear();
                    Console.Write("You checked in the corner and found... ");
                    Console.ReadKey(true);
                    Console.WriteLine("20 gold.");
                    AddGold("20");
                    Console.ReadKey(true);
                    Console.Clear();
                    goldLooted = true;
                }
                else { InventoryMenu(); }
            }
        }
        static int Room31()
        {
            Random rnd = new Random();
            int rowChoice;
            bool chestOpened = false;
            while (true)
            {
                Console.WriteLine("You walk into a large room with a large golden chest in the centre, with a light shining on it from the roof and red arrows pointing at it on the floor.");
                Console.WriteLine("There are three paths you can take to continue.");
                Console.WriteLine("  Paths");
                if (!chestOpened) { Console.WriteLine("  Chest"); currentRowEnd = 5; } else { currentRowEnd = 4; }
                Console.WriteLine("  Inventory");
                rowChoice = Menu(2, false);

                if (rowChoice == 2) { rowChoice = RepeatedOption(3, "Path"); if (rowChoice != -1) { return rowChoice; } Console.Clear(); }
                else if (!chestOpened)
                {
                    if (rowChoice == 3)
                    {
                        Console.Clear();
                        Console.Write("You opened the chest and found... ");
                        Console.ReadKey(true);
                        Console.WriteLine("An explosion!");
                        Console.ReadKey(true);
                        if (rnd.Next(1, 3) == 1)
                        {
                            playerStats[2] -= 15;
                            if (playerStats[2] < 1) { playerStats[2] = 0; }
                            Console.WriteLine("It dealt a lot of damage, hp: " + playerStats[2]);
                            Death();
                        }
                        else
                        {
                            Console.WriteLine("It dealt a lot of damage, hp: 0");
                            playerStats[2] = 0;
                            Death();
                        }
                    }
                    else { InventoryMenu(); }
                }
                else { InventoryMenu(); }
            }
        }
        static int Room311()
        {
            int rowChoice;
            bool[] bottlesTaken = { false, false, false };
            bool allBottlesTaken = false;
            while (true)
            {
                Console.WriteLine("While walking along a path, you see a small pile of bottles in a small hole, and an Old Staff on the ground.");
                Console.WriteLine("  Continue");
                if (!allBottlesTaken) { Console.WriteLine("  Bottle pile"); currentRowEnd = 5; } else { currentRowEnd = 4; }
                if (inventory[0] != "Old Staff") { Console.WriteLine("  Old Staff"); } else { currentRowEnd -= 1; }
                Console.WriteLine("  Inventory");
                rowChoice = Menu(1, false);

                if (rowChoice == 1) { return 1; }
                else if (!allBottlesTaken)
                {
                    if (rowChoice == 2)
                    {
                        rowChoice = RepeatedOption(3, "Potion");
                        Console.Clear();
                        if (rowChoice == 1) 
                        { 
                            if (!bottlesTaken[0]) { bottlesTaken[0] = AddItem("Weak Health Elixer"); } 
                            else { Console.Clear(); Console.WriteLine("You already have this bottle."); Console.ReadKey(true); } 
                        }
                        else if (rowChoice == 2) 
                        { 
                            if (!bottlesTaken[1]) { bottlesTaken[1] = AddItem("Weak Explosive Flask"); }
                            else { Console.Clear(); Console.WriteLine("You already have this bottle."); Console.ReadKey(true); }
                        }
                        else if (rowChoice == -1) { Console.Clear(); continue; }
                        else
                        {
                            if (!bottlesTaken[2]) { bottlesTaken[2] = AddItem("Strong Venemous Vial"); }
                            else { Console.Clear(); Console.WriteLine("You already have this bottle."); Console.ReadKey(true); }
                        }
                        if (bottlesTaken[0] && bottlesTaken[1] && bottlesTaken[2]) { allBottlesTaken = true; }
                        Console.Clear();
                    }
                    else if (inventory[0] != "Old Staff" && rowChoice == 3) { Console.Clear(); AddWeapon("Old Staff"); }
                    else { InventoryMenu(); }
                }
                else if (inventory[0] != "Old Staff") { if (rowChoice == 2) { AddItem("Old Staff"); } else {  InventoryMenu(); } }
                else { InventoryMenu(); }
            }
        }
        static int Room3111()
        {
            Random rnd = new Random();
            int rowChoice;
            bool fought = false;
            while (!fought)
            {
                Console.WriteLine("As you walk a the path meets a stone road.");
                Console.WriteLine("Following it, you find two goblins guarding nothing but the rest of the path. It looks tough to sneak past.");
                Console.WriteLine("  Fight");
                Console.WriteLine("  Sneak past");
                Console.WriteLine("  Inventory");
                currentRowEnd = 5;
                rowChoice = Menu(2, false);

                if (rowChoice == 2)
                {
                    Fight(new string[] { "Goblin", "Goblin" });
                    fought = true;
                }
                else if (rowChoice == 3)
                {
                    int roll;
                    if (status[6] || status[2]) { roll = rnd.Next(15, 21); }
                    else { roll = rnd.Next(1, 21); }
                    Console.Clear();
                    if (roll > 17 || status[3] || status[7]) { Console.WriteLine("You got past, and continued on the stone road."); return 1; }
                    else { Fight(new string[] { "Goblin", "Goblin" }); fought = true; }
                }
                else { InventoryMenu(); }
            }
            bool[] goblinsLooted = { false, false };
            while (true)
            {
                Console.WriteLine("In the room are the two dead Goblins.");
                Console.WriteLine("  Continue");
                if (!goblinsLooted[0] || !goblinsLooted[1]) { Console.WriteLine("  Goblins"); currentRowEnd = 4; } else { currentRowEnd = 3; }
                Console.WriteLine("  Inventory");
                rowChoice = Menu(1, false);

                if (rowChoice == 1) { return 1; }
                else if (rowChoice == 2) 
                {
                    rowChoice = RepeatedOption(2, "Goblin");
                    Console.Clear();
                    if (rowChoice == 1) { if (!goblinsLooted[0]) { Console.WriteLine("You found 20 gold."); goblinsLooted[0] = true; AddGold("20"); } else { Console.WriteLine("You already looted this body."); } }
                    else { if (!goblinsLooted[1]) { Console.WriteLine("You found 15 gold."); goblinsLooted[1] = true; AddGold("15"); } else { Console.WriteLine("You already looted this body."); } }
                    Console.ReadKey(true);
                    Console.Clear();
                }
                else { InventoryMenu(); }
            }
        }
        static int Room31111(string[] route)
        {
            Random rnd = new Random();
            int rowChoice;
            bool fought = false;
            while (!fought)
            {
                Console.WriteLine("Following the path, you find three goblins guarding the path, it feels almost like a trial remembering the last room.");
                Console.WriteLine("It looks almost impossible to sneak past.");
                Console.WriteLine("  Fight");
                Console.WriteLine("  Sneak past");
                Console.WriteLine("  Inventory");
                currentRowEnd = 5;
                rowChoice = Menu(2, false);

                if (rowChoice == 2)
                {
                    Fight(new string[] { "Goblin", "Goblin", "Goblin" });
                    fought = true;
                }
                else if (rowChoice == 3)
                {
                    int roll;
                    if (status[6] || status[2]) { roll = rnd.Next(15, 21); }
                    else { roll = rnd.Next(1, 21); }
                    Console.Clear();
                    if (roll > 18 || status[3] || status[7]) { Console.WriteLine("You got past, and continued on the stone road."); route[1] = "2"; route[2] = "2" ; return 1; }
                    else { Console.WriteLine("You got caught."); Console.ReadKey(true); Console.Clear(); Fight(new string[] { "Goblin", "Goblin", "Goblin" }); fought = true; }
                }
                else { InventoryMenu(); }
            }
            bool[] goblinsLooted = { false, false, false };
            while (true)
            {
                Console.WriteLine("In the room are the three dead Goblins.");
                Console.WriteLine("  Continue");
                if (!goblinsLooted[0] || !goblinsLooted[1] || !goblinsLooted[2]) { Console.WriteLine("  Goblins"); currentRowEnd = 4; } else { currentRowEnd = 3; }
                Console.WriteLine("  Inventory");
                rowChoice = Menu(1, false);

                if (rowChoice == 1) { route[1] = "2"; route[2] = "2"; return 1; }
                else if (rowChoice == 2)
                {
                    rowChoice = RepeatedOption(3, "Goblin");
                    Console.Clear();
                    if (rowChoice == 1) { if (!goblinsLooted[0]) { Console.WriteLine("You found 15 gold."); goblinsLooted[0] = true; inventory[4] += 15; } else { Console.WriteLine("You already looted this body."); } }
                    else if (rowChoice == 2) { if (!goblinsLooted[1]) { Console.WriteLine("You found 25 gold."); goblinsLooted[1] = true; inventory[4] += 25; } else { Console.WriteLine("You already looted this body."); } }
                    else { 
                        if (!goblinsLooted[2]) 
                        { 
                            Console.Write("You found 2 gold and a note that reads... ");
                            Console.ReadKey(true);
                            AddGold("2");
                            Console.WriteLine("\"NO REFUNDS\"");
                            goblinsLooted[2] = true; 
                        } 
                        else { Console.WriteLine("You already looted this body."); } 
                    }
                    Console.ReadKey(true);
                    Console.Clear();
                }
                else { InventoryMenu(); }
            }
        }
        static int Room312()
        {
            int rowChoice;
            bool fought = false;
            while (!fought)
            {
                Console.WriteLine("Following the desolate path, you see a goblin, it seems to be searching around frantically for something.");
                Console.WriteLine("It noticed you but doesn't seem to care about your presence.");
                Console.WriteLine("You can see some kind of stick poking out of some sand.");
                Console.ReadKey(true);
                Console.WriteLine("  Paths");
                Console.WriteLine("  Stick like object");
                Console.WriteLine("  Fight");
                Console.WriteLine("  Inventory");
                currentRowEnd = 7;
                rowChoice = Menu(3, false);

                if (rowChoice == 3) { rowChoice = RepeatedOption(2, "Path"); if (rowChoice != -1) {Console.Clear(); return rowChoice; } }
                else if (rowChoice == 4)
                {
                    Console.Clear();
                    Console.WriteLine("As you reach for the object, the goblin sees you and tries to attack.");
                    Console.ReadKey(true);
                    Fight(new string[] { "Goblin" });
                    fought = true;
                }
                else if (rowChoice == 5)
                {
                    Console.Clear();
                    Fight(new string[] { "Goblin" });
                    fought = true;
                }
                else { InventoryMenu(); }
            }
            bool bodyLooted = false;
            bool swordFound = false;
            while (true)
            {
                Console.Clear();
                Console.WriteLine("You can see the dead goblin and two paths forward.");
                Console.WriteLine("  Paths");
                if (!swordFound) { Console.WriteLine("  Stick like object"); currentRowEnd = 5; } else { currentRowEnd = 4; }
                if (!bodyLooted) { Console.WriteLine("  Goblin body"); } else { currentRowEnd -= 1; }
                Console.WriteLine("  Inventory");
                rowChoice = Menu(1, false);
                Console.Clear();
                if (rowChoice == 1) { rowChoice = RepeatedOption(2, "Path"); if (rowChoice != -1) { return rowChoice; } }
                else if (!swordFound)
                {
                    if (rowChoice == 2) { swordFound = AddWeapon("Silver Blade of the Eldritch"); }
                    else if (rowChoice == 3 && !bodyLooted) 
                    {
                        Console.WriteLine("You found an old wooden shield");
                        AddShield("Old Wooden Shield");
                        Console.Write("You found 2 gold and a note that reads... ");
                        Console.ReadKey(true);
                        Console.WriteLine("\"NO REFUNDS\"");
                        Console.ReadKey(true);
                        AddGold("2");
                        bodyLooted = true;
                    }
                    else { InventoryMenu(); }
                }
                else if (!bodyLooted && rowChoice == 2)
                {
                    Console.Write("You found 2 gold and a note that reads... ");
                    Console.ReadKey(true);
                    Console.WriteLine("\"NO REFUNDS\"");
                    Console.ReadKey(true);
                    AddGold("2");
                    bodyLooted = true;
                }
                else { InventoryMenu(); }

            }
        }
        static int Room3121()
        {
            Console.WriteLine("You walked through a very narrow path, and suddenly... you hear a sound, something mechanical.");
            Console.ReadKey(true);
            Console.WriteLine("You felt your heart get peirced, and died.");
            Console.ReadKey(true);
            System.Environment.Exit(0);
            return 0;
        }
        static int Room3122()
        {
            Random rnd = new Random();
            int rowChoice;
            bool fought = false;
            while (!fought)
            {
                Console.WriteLine("You find yourself in a large open area, with three goblins, guarding 2 chests (or arguing other them)");
                Console.WriteLine("They seem relatively distracted, by the chests and you can probably sneak past.");
                Console.WriteLine("  Fight");
                Console.WriteLine("  Sneak past");
                Console.WriteLine("  Inventory");
                currentRowEnd = 5;
                rowChoice = Menu(2, false);

                if (rowChoice == 2) { Fight(new string[] { "Goblin", "Goblin", "Goblin" }); fought = true; }
                else if (rowChoice == 3)
                {
                    rowChoice = RepeatedOption(2, "Path"); 
                    if (rowChoice == -1) { Console.Clear(); continue; }
                    int roll = rnd.Next(1, 21);
                    Console.Clear();
                    if (roll > 5 || status[2] || status[6]) { Console.WriteLine("You got past.");  return rowChoice; }
                    else { Console.WriteLine("You tripped and got caught..."); Console.ReadKey(true); Console.Clear(); Fight(new string[] { "Goblin", "Goblin", "Goblin" }); fought = true; }
                }
                else { InventoryMenu(); }
            }
            bool[] bodiesLooted = { false, false, false };
            bool[] chestsOpened = { false, false };
            while (true)
            {
                Console.Clear();
                Console.WriteLine("You can see the dead goblins, two silver chests, and two paths to continue forward.");
                Console.WriteLine("  Paths");
                if (!chestsOpened[0] || !chestsOpened[1]) { Console.WriteLine("  Chests"); currentRowEnd = 5; } else { currentRowEnd = 4; }
                if (!bodiesLooted[0] || !bodiesLooted[1] || !bodiesLooted[2]) { Console.WriteLine("  Goblins"); } else { currentRowEnd -= 1; }
                Console.WriteLine("  Inventory");
                rowChoice = Menu(1, false);
                Console.Clear();
                if (rowChoice == 1) { rowChoice = RepeatedOption(2, "Path"); if (rowChoice == -1) { Console.Clear(); continue; } return rowChoice; }
                else if (!chestsOpened[0] || !chestsOpened[1])
                {
                    if (rowChoice == 2)
                    {
                        rowChoice = RepeatedOption(2, "Chest");
                        if (rowChoice == 1) { chestsOpened[0] = Chest("50", "gold"); }
                        else if (rowChoice == 2) { chestsOpened[1] = Chest("Strong Health Elixer", "potion"); }
                    }
                    else if (rowChoice == 3 && (!bodiesLooted[0] || !bodiesLooted[1] || !bodiesLooted[2]))
                    {
                        rowChoice = RepeatedOption(3, "Goblin");
                        if (rowChoice == 1 && !bodiesLooted[0])
                        {
                            Console.WriteLine("You found 20 gold.");
                            AddGold("20");
                            bodiesLooted[0] = true;
                        }
                        else if (rowChoice == 2 && !bodiesLooted[1])
                        {
                            bodiesLooted[1] = AddItem("Weak Shadow Essence");
                            Console.Write("You found a note that says... ");
                            Console.ReadKey(true);
                            Console.WriteLine("\"NO REFUNDS\"");
                        }
                        else if (rowChoice == 3 && !bodiesLooted[2])
                        {
                            Console.WriteLine("You found 15 gold.");
                            AddGold("15");
                            bodiesLooted[0] = true;
                        }
                        else { Console.WriteLine("You already looted this body."); Console.ReadKey(true); }
                    }
                    else { InventoryMenu(); }
                }
                else if (rowChoice == 2 && (!bodiesLooted[0] || !bodiesLooted[1] || !bodiesLooted[2]))
                {
                    rowChoice = RepeatedOption(3, "Goblin");
                    if (rowChoice == 1 && !bodiesLooted[0])
                    {
                        Console.WriteLine("You found 20 gold.");
                        AddGold("20");
                        bodiesLooted[0] = true;
                    }
                    else if (rowChoice == 2 && !bodiesLooted[1])
                    {
                        bodiesLooted[1] = AddItem("Weak Shadow Essence");
                        Console.Write("You found a note that says... ");
                        Console.ReadKey(true);
                        Console.WriteLine("\"NO REFUNDS\"");
                    }
                    else if (rowChoice == 3 && !bodiesLooted[2])
                    {
                        Console.WriteLine("You found 15 gold.");
                        AddGold("15");
                        bodiesLooted[0] = true;
                    }
                    else { Console.WriteLine("You already looted this body."); Console.ReadKey(true); }
                }
                else { InventoryMenu(); }
            }
        }
        static int Room31221()
        {
            return 9;
        }
        static int Room31222()
        {
            int rowChoice;
            bool chestOpened = false;
            while (!chestOpened)
            {
                Console.WriteLine("You end up in front of a giant vault, with a locked door.");
                Console.WriteLine("You can see two chests, one with a skull symbol, one with a key symbol.");
                Console.WriteLine("  Skull chest");
                Console.WriteLine("  Key chest");
                Console.WriteLine("  Inventory");
                currentRowEnd = 5;
                rowChoice = Menu(2, false);
                Console.Clear();
                if (rowChoice == 1)
                {
                    Console.Write("You opened the skull chest and... ");
                    Console.ReadKey(true);
                    Console.WriteLine("it exploded.");
                    Console.ReadKey(true);
                    Console.WriteLine("You died");
                    System.Environment.Exit(0);
                }
                else if (rowChoice == 2)
                {
                    Console.WriteLine("You opened the key chest and... ");
                    Console.ReadKey(true);
                    Console.WriteLine("found a key inside.");
                    Console.ReadKey(true);
                }
            }
            while (true)
            {
                Console.Clear();
                Console.WriteLine("  Try key");
                Console.WriteLine("  Skull chest");
                Console.WriteLine("  Inventory");
                currentRowEnd = 3;
                rowChoice = Menu(1, false);

                if (rowChoice == 1)
                {
                    Console.Write("You tried the key and... ");
                    Console.ReadKey(true);
                    Console.WriteLine("the door opened.");
                    Console.ReadKey(true);
                    Console.WriteLine("(You left the key in the lock)");
                    Console.ReadKey(true);
                    return 1;
                }
                else if (rowChoice == 2)
                {
                    Console.Write("You opened the skull chest and... ");
                    Console.ReadKey(true);
                    Console.WriteLine("it exploded.");
                    Console.ReadKey(true);
                    Console.WriteLine("You died.");
                    Console.ReadKey(true);
                    System.Environment.Exit(0);
                }
                else { InventoryMenu(); }
            }
        }
        static int Room312221(string[] route)
        {
            int rowChoice;
            bool swordBought = false;
            bool potionBought = false;
            Console.WriteLine("You entered the giant vault.");
            Console.WriteLine("You see an old Goblin at what seems to be some kind of shop stand.");
            Console.WriteLine("\"Hmm, a human, don't see many of those now a days.\"");
            Console.ReadKey(true);
            Console.WriteLine("\"Don't be threatened, unlike the others I don't see you as an enemy, but a customer\"");
            Console.WriteLine("\"I presume you're here for ancient artifacts, well I have quite the stock... as long as you have the gold that is.\"");
            Console.ReadKey(true);
            while (true)
            {
                Console.Clear();
                Console.WriteLine("\"Unfortunately I have quite the small stock at the moment, the 'Revolution' has been purchasing a lot lately.\"");
                Console.WriteLine("  Strange dice - 50 gold");
                if (!swordBought) { Console.WriteLine("  Silver Blade of the Eldritch - 20 gold"); currentRowEnd = 5; } else { currentRowEnd = 4; }
                if (!potionBought) { Console.WriteLine("  Weak Health Elixer - 10 gold"); } else { currentRowEnd -= 1; }
                Console.WriteLine("  Inventory");
                rowChoice = Menu(1, false);
                Console.Clear();
                if (rowChoice == 1)
                {
                    Console.WriteLine("\"Ah, you have a good eye, that there is an incredibly powerful item, that even the king would dribble over\"");
                    Console.WriteLine("\"No ones able to make up the money for it though, and of course if someone tries to steal it, well, I have my ways.\"");
                    Console.WriteLine("\"That being said, do you have the money for it?\"");
                    Console.ReadKey(true);
                    Console.Clear();
                    if (magic) { Console.WriteLine("Ending (6/6)"); }
                    else if (gold > 49)
                    {
                        Console.WriteLine("\"You do? Ha, I guess I shouldn't be surprised, you must've killed many to get here.\"");
                        Console.WriteLine("He gave you the magical dice, as you held it you felt a sense of divinity, as if you now had the power to do whatever you wanted.");
                        Console.ReadKey(true);
                        Console.WriteLine("\"It's called the D20, I suppose since it has 20 sides, but that's no longer any concern of mine.\"");
                        Console.WriteLine("With this grand new power attained, you easily went escaped the Eldritch Grove, and went on to rule the world.");
                        Console.ReadKey(true);
                        Console.WriteLine("Ending 2/6");
                        Console.ReadKey(true);
                        System.Environment.Exit(0);
                    }
                    else
                    {
                        while (true)
                        {
                            Console.Clear();
                            Console.WriteLine("You don't, well that's a shame, I don't have any other magical items to sell you at the moment.");
                            Console.WriteLine("  Steal");
                            Console.WriteLine("  Don't steal");
                            Console.WriteLine("  Inventory");
                            currentRowEnd = 4;
                            rowChoice = Menu(1, true);
                            if (rowChoice == -1) { break; }
                            else if (rowChoice == 1) 
                            { 
                                Console.WriteLine("\"Heh, I wouldn't do that if I were you.\"");
                                Console.WriteLine("The old goblin raised his arm, some strange light began to shine from his palm.");
                                Console.WriteLine("He struck down his arm as if to attack, but rather than hitting anything, the light grew much brighter...");
                                Console.ReadKey(true);
                                Console.WriteLine();
                                Console.WriteLine("Once the light seemed to be gone, you opened your eyes, and saw a familiar sight...");
                                Console.ReadKey(true);
                                route = new string[] { "0", "0", "0", "0", "0", "0" };
                                return 0;
                            }
                            else if (rowChoice == 2) { break; }
                            else { InventoryMenu(); }
                        }
                    }
                }
                else if (rowChoice == 2)
                {
                    if (gold > 20)
                    {
                        Console.WriteLine("You bought the Silver Blade of the Eldritch");
                        AddWeapon("Silver Blade of the Eldritch");
                        gold -= 20;
                        inventory[4] = gold.ToString();
                    }
                    else { Console.WriteLine("You don't seem to have the money"); Console.ReadKey(true); }
                }
                else if (rowChoice == 3)
                {
                    if (gold > 10)
                    {
                        Console.WriteLine("You bought the Weak Health Elixer");
                        AddItem("Weak Health Elixer");
                        gold -= 10;
                        inventory[4] = gold.ToString();
                    }
                }
            }
            
        }
        static void Playing()
        {
            for (int i = 0; i < potions.Length; i++) { potions[i] = "Empty"; }
            string[] path = { "0", "0", "0", "0", "0", "0" };
            Controls();
            Console.WriteLine("Please enter full screen for optimal gameplay experience.");
            Console.ReadKey(true);
            Console.Clear();
            Console.WriteLine("After searching for many moons, you have finally come across the Eldritch Grove.");
            Console.WriteLine("While there're only rumors, you're sure some ancient magic must await you in this ancient forest.");
            currentRowEnd = 2;
            for (int i = 0; i < path.Length; i++)
            {
                path[i] = RoomFetcher(path, i);
                Console.Clear(); ImportantActionPlayer(); currentRowEnd = 0;
            }

        }
        static void ImportantActionPlayer()
        {
            if (status[1]) { playerStats[2] -= 2; Console.WriteLine("You took 2 dmg from poison."); Console.WriteLine("hp: " + playerStats[2]); }
            else if (status[0]) { playerStats[2] -= 1; Console.WriteLine("You took 1 dmg from poison."); Console.WriteLine("hp: " + playerStats[2]); }
            if (status[0]) { Console.ReadKey(true); Console.Clear(); }

            if (playerStats[2] < 1) { Death(); }
        }
        static void ImportantActionEnemy(int hp, bool poisoned, bool strong)
        {

        }
        static void Death()
        {
            Random rnd = new Random();
            Console.Clear();
            Console.WriteLine("hp: 0");
            Console.ReadKey(true);
            if (rnd.Next(1, 21) == 20)
            {
                Console.WriteLine("Miraculously, you felt some incredible force, and realised your quest couldn't end here.");
                Console.WriteLine("You held firm and continued forward.");
                Console.ReadKey(true);
                playerStats[2] = 1;
                status[0] = false;
                status[1] = false;
                Console.WriteLine("hp: 1");
            }
            else
            {
                Console.WriteLine("You Died.");
                Console.ReadKey(true);
                System.Environment.Exit(0);
            }
        }
        static void Controls()
        {
            Console.WriteLine("Controls:");
            Console.WriteLine("Down Arrow - move down");
            Console.WriteLine("Up Arrow - move up");
            Console.WriteLine("Left Arrow - Previous Menu");
            Console.WriteLine("Enter - select current option");
            Console.WriteLine("Y - do proposed action");
            Console.WriteLine("N - don't do prosposed action");
        }
        static int WeaponList(string weapon, bool describe)
        {
            //string[] weaponCatalogue = { "Dagger", "Old Staff", "Shotgun" };
            Console.Clear();
            switch (weapon)
            {
                case "Dagger":
                    if (describe)
                    {
                        Console.WriteLine("A basic dagger: a six-inch steel blade, a comfortable leather-wrapped hilt - simple, practical, and ready for swift strikes in close combat.");
                        Console.WriteLine("Average dmg: 6");
                        Console.ReadKey(true);
                    }
                    return 6;
                case "Old Staff":
                    if (describe)
                    {
                        Console.WriteLine("Old Staff");
                        Console.WriteLine("Aerage dmg: 3");
                        Console.ReadKey(true);
                    }
                    return 3;
                case "Silver Blade of the Eldritch":
                    if (describe)
                    {
                        Console.WriteLine("cool sword");
                    }
                    return 10;
                case "Bronze Blade of the Eldritch":
                    if (describe)
                    {
                        Console.WriteLine("decent sword");
                    }
                    return 8;


            }
            return 6;
        }
        static int ArmorList(string armor) { return 2; }
        static void Main(string[] args)
        {
            //Console.WriteLine(Room0()); Console.ReadKey(true);
            Playing();

            Console.ReadKey(true);
        }
    }
}