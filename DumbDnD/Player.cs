using System;

namespace DumbDnD
{
    public class Player
    {
        public int PlayerId { get; set; }
        public string Name { get; set; }
        public int Health { get; set; }
        public int Gold { get; set; }
        public int ElementId { get; set; }
        public string ElementName { get; set; }
        public int MoveRoll { get; set; }
        public int Position { get; set; }
        public bool Dead { get; private set; }
        
        public void TakeDamage(int damage, GameplayState state)
        {
            Health -= damage;
            if (Health > 0) return;
            Health = 0;
            Dead = true;
            Console.WriteLine(Name + " has died and out of the game!");
            state.NumOfDeadPlayers++;
        }
    }
}