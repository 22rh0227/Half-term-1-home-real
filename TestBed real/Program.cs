using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestBed_real
{
    internal class Program
    {
        static int currentRowEnd;
        static string[] inventory = { "Dagger", "Empty", "Old Tattered Clothing", "Items", "0 gold" };
        static int gold = 0;
        static string[] potions = new string[10];
        static int[] playerStats = { 6, 2, 30, 1 };
        static bool[] status = { false, false, false, false, false, false, false, false };
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
                        if (roll == 20 || status[7]) { Console.WriteLine("Lukily, you dropped the potion.");  break; }
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
                    case "St": break;
                    case "Sh": break;
                    case "Ex": break;
                    case "Fo": break;
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
            if (YesOrNo() == 'y') { Console.WriteLine("You equipped " + shield + ", and threw away " + inventory[1]); inventory[1] = shield; Console.ReadKey(true); Console.Clear(); currentRowEnd = 0; return true; }
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
            Console.WriteLine("You added " + money + " to your inventory");
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
                                            switch (route[5])
                                            {
                                                case "1": return Room1211().ToString();
                                                default: return Room1221().ToString();
                                            }

                                    }
                            }
                    }
                
                    
                    
                    
                default: Console.WriteLine("something went wrong in RoomFetcher()"); return "8";
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
                    else if (rowChoice == currentRowEnd - 2) { Console.WriteLine("Strange Light"); return 4; }
                    else { InventoryMenu(); }
                }
                else
                {
                    if (rowChoice == currentRowEnd - 3)
                    {
                        rowChoice = RepeatedOption(3, "Path");
                        if (rowChoice != -1) { return rowChoice; }
                    }
                    else if (rowChoice == currentRowEnd - 2) { Console.WriteLine("Strange Light"); return 4; }
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
                if (!gotBottle) { Console.WriteLine("  Bottle");  currentRowEnd = 5; }
                else { currentRowEnd = 4; }
                Console.WriteLine("  Explore dark room further");
                Console.WriteLine("  Inventory");
                rowChoice = Menu(1, false);
                if (rowChoice == 1) { route[1] = "2" ; return 2; }
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

                if (rowChoice == 2) { fought = true; }
                else if (rowChoice == 3) 
                { 
                    int roll = rnd.Next(1, 21);
                    rowChoice = RepeatedOption(2, "Path");
                    Console.Clear();
                    if (rowChoice == 1)
                    {
                        if (roll > 14 || status[6] || status[2]) { Console.WriteLine("You snuk past the goblin and went through the first path."); return rowChoice; }
                        else { Console.WriteLine("You got caught!"); Console.ReadKey(true); fought = true; }
                    }
                    else if (rowChoice == 2)
                    {
                        if (roll > 7 || status[6] || status[2]) { Console.WriteLine("You snuk past the goblin and went through the second path."); return rowChoice; }
                        else { Console.WriteLine("You tripped and got caught!"); Console.ReadKey(true); fought = true; }
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

                if (rowChoice == 2) { fought = true; }
                else if (rowChoice == 3)
                {
                    Console.Clear();
                    int roll = rnd.Next(1, 21);
                    if (roll == 15 || status[6] || status[2])
                    {
                        Console.WriteLine("You managed to sneak past the bear-like creature, and went through the exit.");
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
            bool fought = false;
            int rowChoice;
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
            Console.WriteLine("  Fight");
            Console.WriteLine("  Inventory");
            currentRowEnd = 10;
            while (!fought)
            {
                if (currentRowEnd != 10)
                {
                    Console.WriteLine("You know you cannot leave now, right on the cusp of the ancient eldrictch magic you so desire.");
                    Console.WriteLine("  Fight");
                    Console.WriteLine("  Inventory");
                    currentRowEnd = 3;
                    rowChoice = Menu(1, false);
                }
                else { rowChoice = Menu(8, false); }
                if (rowChoice == 8 || rowChoice == 1)
                {
                    Console.Clear();
                    Console.WriteLine("You charged at the king, weapon in hand.");
                    Console.WriteLine("He smiled. And with the lift of his staff, summoned thunderous booms and jagged bolts of lightning all around the room.");
                    fought = true;
                }
                else { InventoryMenu(); }
            }
            Console.WriteLine("You win");
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
                    Console.WriteLine("Chests:");
                    for (int i = 0; i < chestsOpened.Length; i++) { Console.WriteLine("  Chest " + (i + 1)); }
                    currentRowEnd = 3;
                    rowChoice = Menu(1, true);
                    if (rowChoice == 1 && !chestsOpened[0]) { if (Chest("Weak Explosive Falsk", "potion")) { chestsOpened[0] = true; } }
                    else if (rowChoice == 2 && !chestsOpened[1]) 
                    {
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
                    Console.WriteLine("You defeated them.");
                    AddWeapon("Copper Blade of the Eldritch");
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
                    else if (rowChoice == 2) { fought = false; }
                    else { InventoryMenu(); }
                }
            }
        }
        static void Playing()
        {
            for (int i = 0; i < potions.Length; i++) { potions[i] = "Empty"; }
            potions[2] = "Strong Venomous Vial";
            potions[3] = "Weak Venomous Vial";
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
                Console.Clear(); ImportantAction(); currentRowEnd = 0;
            }

        }
        static void ImportantAction()
        {
            if (status[1]) { playerStats[2] -= 2; Console.WriteLine("You took 2 dmg from poison."); Console.WriteLine("hp: " + playerStats[2]); }
            else if (status[0]) { playerStats[2] -= 1; Console.WriteLine("You took 1 dmg from poison."); Console.WriteLine("hp: " + playerStats[2]); }
            if (status[0]) { Console.ReadKey(true); Console.Clear(); }

            if (playerStats[2] < 1) { Death(); }
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
                        Console.WriteLine("old staff");
                        Console.WriteLine("Aerage dmg: 3");
                        Console.ReadKey(true);
                    }
                    return 3;
                case "Shotgun":
                    if (describe)
                    {
                        Console.WriteLine("KABLAM");
                        Console.WriteLine("Average dmg: 10");
                        Console.ReadKey(true);
                    }
                    return 10;


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
