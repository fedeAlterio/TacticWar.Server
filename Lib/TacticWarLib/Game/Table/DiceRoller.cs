﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TacticWar.Lib.Game.Abstractions;

namespace TacticWar.Lib.Game.Table
{
    public class DiceRoller : IDiceRoller
    {
        // Private fields
        private static readonly Random _random = new();



        // Core
        public int RollDice()
        {
            return _random.Next(1, 7);
        }
    }
}
