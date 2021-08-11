using System;
using System.Collections.Generic;

namespace DumbDnD
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var coreLoop = new CoreGameplayLoop();
            var players = new List<Player>();
            var state = new GameplayState();

            coreLoop.InstantiateGame(state, players);
            coreLoop.CoreLoop(state, players);
          
            Console.WriteLine(state.LastPlayer + " Wins!");
            Console.WriteLine("GameOver!");
            coreLoop.pauseTheGame();
        }
    }
}