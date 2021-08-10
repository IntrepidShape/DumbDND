using System;
using System.Collections.Generic;

namespace DumbDnD
{
    internal class Player
    {
        public int PlayerId = 0;
        public string Name = "";
        public int Health = 50;
        public int Gold = 0;
        public int ElementId = 0;
        public string ElementName = "";
        public int MoveRoll = 0;
        public int Position = 0;
        public bool Dead = false;
    }
    
     static class Program
     { 
         private static int numOfDeadPlayers = 0;
        private static int RandomNumber(int min, int max)
        {
            //Creates new instance of random seeded with current second in the current minute
            Random random = new Random(DateTime.Now.Second);
            return random.Next(min, max);
        }

        private static int ElementRollAround(int _elementID, bool minus, int modifier)
        {
            switch (minus)
            {
                case false:
                {
                    for (int i = 0; i < modifier; i++)
                    {
                        _elementID += 1;
                        if (_elementID == 6)
                        {   
                            _elementID = 0;
                        }
                    }

                    break;
                }
                case true:
                {
                    for (int i = 0; i < modifier; i++)
                    {
                        _elementID -= 1;
                        if (_elementID == -1)
                        {
                            _elementID = 5;
                        }
                    }

                    break;
                }
            }
            return _elementID;
        }

        public static void TakeDamage(Player _player, int damage)
        {
            _player.Health -= damage;
            if (_player.Health <= 0)
            {
                _player.Health = 0;
                _player.Dead = true;
                numOfDeadPlayers++;
            }
        }

        private static string WhatElementAmI(int elementID)
        {
            /* Broken and does not function as intended
            if(elementID > 5)
            {
                elementID -= 5;
            }
            if(elementID < 0)
            {
                elementID += 5;
            }*/
            string[] elements = { "Earth", "Light", "Ice", "Thunder", "Dark", "Fire" };
            return elements[elementID];
        }

        public static int GBRollaround(int position, int roll)
        {
            for(int i = 0; i < roll; i++)
            {
                position += 1; 
                if (position > 27)
                {
                    position = 0;
                }
                if(GameBoardTile(position) == "Store")
                {
                    Console.WriteLine("You have been Past the store!");
                }
            }
            return position;
        }

        public static string GameBoardTile (int position)
        {
            
            string[] tiles = { "Store", "Dark", "Light", "Fire", "Thunder", "Ice", "Earth", "Store & PVP", "Dark", "Light", "Fire", "Thunder", "Ice", "Earth", "Store", "Dark", "Light", "Fire", "Thunder", "Ice", "Earth", "Store & PVP", "Dark", "Light", "Fire", "Thunder", "Ice", "Earth" };
            return tiles[position];
        }

        public static void InvokeTile(Player _player)
        {
            string tile = GameBoardTile(_player.Position);
            switch (tile)
            {
                case "Store & PVP":
                    Console.WriteLine("You Landed on Store & PVP...This is where the PVP code will go! Hit enter to continue");
                    return;

                case "Store":
                    Console.WriteLine("You Landed on the store...This is where the store code will go! Hit enter to continue" );
                    return;
            }

            if (WhatElementAmI(_player.ElementId) == tile)
            {
                Console.WriteLine("You would get the choice to PVP here because you landed on a tile of your own");
            } else 
            {   
                if (WhatElementAmI(ElementRollAround(_player.ElementId,false, 1)) == tile)
                {
                    Console.WriteLine("You landed on the Battle...You are strong against " + tile + " you win.");
                    _player.Gold += 5;
                }
                else if (WhatElementAmI(ElementRollAround(_player.ElementId, true, 1)) == tile)
                {
                    Console.WriteLine("You landed on the Battle...You are weak against " + tile + " you lose 10 Health");
                    TakeDamage(_player,10);
                    Console.WriteLine("Health is " + _player.Health);
                    
                    
                }
                else if (WhatElementAmI(ElementRollAround(_player.ElementId, false, 3)) == tile)
                {
                    Console.WriteLine("You landed on the Battle...You are evenly matched by " + tile + " you are safe for your turn.");
                }
                else if (WhatElementAmI(ElementRollAround(_player.ElementId, false, 2)) == tile || WhatElementAmI(ElementRollAround(_player.ElementId, false,  4)) == tile)
                {
                    Console.WriteLine("You landed on the Battle...Time to Duel! You will battle " + tile + " press enter to roll!");
                    Console.ReadLine();
                    int tempRoll = RandomNumber(1, 7);
                    if (tempRoll >= 4)
                    {
                        Console.WriteLine("You rolled " + tempRoll + " you Win!");
                    }
                    if (tempRoll <= 3)
                    {
                        Console.WriteLine("You rolled " + tempRoll + " you Lose!");
                        TakeDamage(_player,5);
                        Console.WriteLine("Health is " + _player.Health);
                    }

                }
                else
                {
                    Console.WriteLine("Something has gone horribly wrong with the code of the invoke Tile Function. Printing error below.");
                    Console.WriteLine("Element ID " + WhatElementAmI(_player.ElementId) + " " + "Current Tile " + tile);
                }
            }
        }

        static void Main(string[] args)
        {
            int NumberOfPlayers;

            List<Player> Players = new List<Player>();
            Console.WriteLine("How many players?");
            NumberOfPlayers = Convert.ToInt16(Console.ReadLine());
            
            if (NumberOfPlayers <= 6 && NumberOfPlayers > 0) //if out of bounds go into testing
            {
                for (int i = 0; i <= (NumberOfPlayers - 1); i++)
                {

                    Players.Add(new Player());
                    Players[i].PlayerId = i + 1;
                    Console.WriteLine("What is player " + Players[i].PlayerId + "'s name?");
                    Players[i].Name = Console.ReadLine();
                    Players[i].Health = 50;
                    Players[i].Gold = 0;
                    int tempRoll = RandomNumber(0, 6);
                    Players[i].ElementId = tempRoll;
                    Players[i].ElementName = WhatElementAmI(Players[i].ElementId);
                    Players[i].Position = 0;
                    Console.WriteLine("");
                    Console.WriteLine(Players[i].Name + " (Player " + Players[i].PlayerId + ")" + " Rolls a " + tempRoll);
                    Console.WriteLine("");
                    Console.WriteLine(Players[i].Name + " (Player " + Players[i].PlayerId + ")" + " is assigned the element " + Players[i].ElementName);
                    Console.WriteLine("");
                }

                for (int i = 0; i <= (NumberOfPlayers - 1); i++)
                {

                    Console.WriteLine("Player Number " + Players[i].PlayerId);
                    Console.WriteLine("Name " + Players[i].Name);
                    Console.WriteLine("Health " + Players[i].Health);
                    Console.WriteLine("Gold " + Players[i].Gold);
                    Console.WriteLine("ElementID " + Players[i].ElementId);
                    Console.WriteLine("ElementName " + Players[i].ElementName);
                    Console.WriteLine("");
                }
                Console.WriteLine("Initialisation Complete");
                Console.WriteLine("Hit Enter To Continue...");
                Console.ReadLine();

                Console.WriteLine("");

                int round = 0;
                //first Loop?
                while (numOfDeadPlayers <= NumberOfPlayers - 1)
                {
                    round++;
                    Console.WriteLine("RoundNumber" + round);
                    
                    for (int PlayerNumber = 0; PlayerNumber <= (NumberOfPlayers - 1); PlayerNumber++)
                    {
                        if (Players[PlayerNumber].Dead)
                        {
                            break;
                        }
                        Console.WriteLine("Hit Enter to roll to move");
                        Console.ReadLine();
                        Players[PlayerNumber].MoveRoll = RandomNumber(1, 7);
                        Console.WriteLine(Players[PlayerNumber].Name + " (" + Players[PlayerNumber].ElementName + ")" + " Rolls a " + Players[PlayerNumber].MoveRoll);
                        Players[PlayerNumber].Position = GBRollaround(Players[PlayerNumber].Position, Players[PlayerNumber].MoveRoll);
                        Console.WriteLine(Players[PlayerNumber].Name + " (" + Players[PlayerNumber].ElementName + ")" + " Lands on " + GameBoardTile(Players[PlayerNumber].Position));
                        InvokeTile(Players[PlayerNumber]);
                        Console.ReadLine();
                    }
                }

                
                for(var i = 0; i < NumberOfPlayers; i++)
                {
                    if (!Players[i].Dead)
                    {
                        Console.WriteLine(Players[i].Name + " Wins!");
                        Console.WriteLine("GameOver!");
                        break;
                    }
                }

            }
            else
            {
                //Function Tests go in here /unit tests
                Console.WriteLine(ElementRollAround(2,true, 3));
            }
                
        }
    }
}
