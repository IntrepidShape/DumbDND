using System;
using System.Collections.Generic;
using System.Data;
using static System.Console;

namespace DumbDnD
{
    public class CoreGameplayLoop
    {

        public static int RandomNumber(int min, int max)
        {
            //Creates new instance of random seeded with current second in the current minute
            Random random = new Random(DateTime.Now.Second);
            return random.Next(min, max);
        }

        public int ElementRollAround(int elementId, bool minus, int modifier)
        {
            switch (minus)
            {
                case false:
                {
                    for (var i = 0; i < modifier; i++)
                    {
                        elementId += 1;
                        if (elementId == 6)
                        {
                            elementId = 0;
                        }
                    }
                    
                    
                    break;
                }
                case true:
                {
                    for (int i = 0; i < modifier; i++)
                    {
                        elementId -= 1;
                        if (elementId == -1)
                        {
                            elementId = 5;
                        }
                    }

                    break;
                }
            }

            return elementId;
        }

        


        public static string WhatElementAmI(int elementID)
        {
            /* Broken and does not function as intended for historical purposes...put it in a museum with all my failures
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

        private int GBRollaround(int position, int roll)
        {
            for (int i = 0; i < roll; i++)
            {
                position += 1;
                if (position > 27)
                {
                    position = 0;
                }

                if (GameBoardTile(position) == "Store")
                {
                    WriteLine("You have been Past the store!");
                }
            }

            return position;
        }

        private string GameBoardTile(int position)
        {

            string[] tiles =
            {
                "Store", "Dark", "Light", "Fire", "Thunder", "Ice", "Earth", "Store & PVP", "Dark", "Light", "Fire",
                "Thunder", "Ice", "Earth", "Store", "Dark", "Light", "Fire", "Thunder", "Ice", "Earth", "Store & PVP",
                "Dark", "Light", "Fire", "Thunder", "Ice", "Earth"
            };
            return tiles[position];
        }

        private void InvokeTile(Player player, GameplayState state)
        {
            string tile = GameBoardTile(player.Position);
            switch (tile)
            {
                case "Store & PVP":
                    WriteLine(
                        "You Landed on Store & PVP...This is where the PVP code will go! Hit enter to continue");
                    return;

                case "Store":
                    WriteLine(
                        "You Landed on the store...This is where the store code will go! Hit enter to continue");
                    return;
            }

            if (WhatElementAmI(player.ElementId) == tile)
            {
                WriteLine("You would get the choice to PVP here because you landed on a tile of your own");
            }
            else
            {
                if (WhatElementAmI(ElementRollAround(player.ElementId, false, 1)) == tile)
                {
                    WriteLine("You landed on the Battle...You are strong against " + tile + " you win.");
                    player.Gold += 5;
                }
                else if (WhatElementAmI(ElementRollAround(player.ElementId, true, 1)) == tile)
                {
                    WriteLine("You landed on the Battle...You are weak against " + tile +
                                      " you lose 10 Health");
                    player.TakeDamage(10, state);
                    WriteLine("Health is " + player.Health);


                }
                else if (WhatElementAmI(ElementRollAround(player.ElementId, false, 3)) == tile)
                {
                    WriteLine("You landed on the Battle...You are evenly matched by " + tile +
                                      " you are safe for your turn.");
                }
                else if (WhatElementAmI(ElementRollAround(player.ElementId, false, 2)) == tile ||
                         WhatElementAmI(ElementRollAround(player.ElementId, false, 4)) == tile)
                {
                    WriteLine("You landed on the Battle...Time to Duel! You will battle " + tile +
                                      " press enter to roll!");
                    ReadLine();
                    var tempRoll = RandomNumber(1, 7);
                    switch (tempRoll)
                    {
                        case >= 4:
                            WriteLine("You rolled " + tempRoll + " you Win!");
                            break;
                        case <= 3:
                            WriteLine("You rolled " + tempRoll + " you Lose!");
                            player.TakeDamage(5, state);
                            WriteLine("Health is " + player.Health);
                            break;
                        default:
                            WriteLine("The dice have rolled out of bounds, something has gone wrong.");
                            break;
                    }

                }
                else
                {
                    WriteLine(
                        "Something has gone horribly wrong with the code of the invoke Tile Function. Printing error below.");
                    WriteLine("Element ID " + WhatElementAmI(player.ElementId) + " " + "Current Tile " + tile);
                }
            }
        }

        public void pauseTheGame()
        {
            while (true)
            {
                WriteLine("Type Exit to close");
                var unpause = ReadLine();
                if (unpause is "Exit" or "exit" or "Exit()" or "exit()")
                {
                    Environment.Exit(0);
                    return;
                }
            }
        }

        public void CoreLoop(GameplayState state, List<Player> players)
        {
            
            while (state.VictoryConditionMet == false)
            {

                WriteLine("Round Number " + state.Round);

                for (var i = 0; i <= state.NumberOfPlayers -1; i++)
                {
                    if (players[i].Dead)
                    {
                        switch (state.UntilLastPlayerDead)
                        {
                            case false when state.NumOfDeadPlayers == state.NumberOfPlayers - 1:
                                state.VictoryConditionMet = true;
                                return;
                            case true when state.NumOfDeadPlayers == state.NumberOfPlayers:
                                state.VictoryConditionMet = true;
                                return;
                        }
                    }
                    else if (state.NumOfDeadPlayers == state.NumberOfPlayers - 1 && !state.UntilLastPlayerDead)
                    {
                        state.LastPlayer = players[i].Name;
                        return;
                    } else
                    {
                        
                        WriteLine("Hit Enter to roll to move");
                        ReadLine();
                        players[i].MoveRoll = RandomNumber(1, 7);
                        WriteLine(players[i].Name + " (" + players[i].ElementName + ")" +
                                          " Rolls a " + players[i].MoveRoll);

                        players[i].Position = GBRollaround(players[i].Position,
                            players[i].MoveRoll);

                        WriteLine(players[i].Name + " (" + players[i].ElementName + ")" +
                                          " Lands on " + GameBoardTile(players[i].Position));

                        InvokeTile(players[i], state);
                        ReadLine();
                        state.LastPlayer = players[i].Name;
                    }
                    state.Round++;
                }
            }
        }

        private void PrintPlayers(GameplayState state, List<Player> players)
        {
            for (var i = 0; i <= (state.NumberOfPlayers - 1); i++)
            {

                WriteLine("Player Number " + players[i].PlayerId);
                WriteLine("Name " + players[i].Name);
                WriteLine("Health " + players[i].Health);
                WriteLine("Gold " + players[i].Gold);
                WriteLine("ElementID " + players[i].ElementId);
                WriteLine("ElementName " + players[i].ElementName);
                WriteLine("");
            }
        }

        public void InstantiateGame(GameplayState state, List<Player> players)
        {

            WriteLine("How many players? type 1 to 6");
            state.NumberOfPlayers = Convert.ToInt16(ReadLine()); 
            WriteLine(state.NumberOfPlayers);
            if (state.NumberOfPlayers > 1)
            {
                WriteLine(
                    "Do you want to play until the last player is dead? (default is no) type Yes for yes ");
                var question = ReadLine();
                if (question is "Yes" or "yes")
                {
                    state.UntilLastPlayerDead = true;
                }
            }

            if (state.NumberOfPlayers > 0) //if out of bounds go into testing
            {
                for (var i = 0; i <= state.NumberOfPlayers - 1; i++)
                {

                    players.Add(new Player());
                    players[i].PlayerId = i + 1;
                    WriteLine("What is player " + players[i].PlayerId + "'s name?");
                    players[i].Name = ReadLine();
                    players[i].Health = 50;
                    players[i].Gold = 0;
                    var tempRoll = RandomNumber(0, 6);
                    players[i].ElementId = tempRoll;
                    players[i].ElementName = WhatElementAmI(players[i].ElementId);
                    players[i].Position = 0;
                    WriteLine("");
                    WriteLine(
                        players[i].Name + " (Player " + players[i].PlayerId + ")" + " Rolls a " + tempRoll);
                    WriteLine("");
                    WriteLine(players[i].Name + " (Player " + players[i].PlayerId + ")" +
                              " is assigned the element "
                              + players[i].ElementName);
                    WriteLine("");

                }

                WriteLine("Initialisation Complete");
                WriteLine("Hit Enter To Continue...");
                ReadLine();

                PrintPlayers(state, players);
                WriteLine("");
                
            }
        }
    }
}