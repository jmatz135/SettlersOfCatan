using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SettlersOfCatan
{
    //Purpose: The class creates a dice object that can then be used by the game to roll the dice simulating randomly rolling dice
    public class Dice
    {
        private int die1;
        private int die2;
        private Random randomNumbers = new Random();

        //Array containg the values of die1 and die2
        int[] diceRoll;

        //Constructor
        public Dice()
        {
            diceRoll = new int[2];
        }

        //Purpose: rolls dice and creates random numbers for each die and stores them in diceroll[]
        //Returns: diceroll[] which contains die1 and die2
        public int[] rollDice()
        {
            die1 = randomNumbers.Next(1,6);
            die2 = randomNumbers.Next(1,6);

            diceRoll[0] = die1;
            diceRoll[1] = die2;
            return diceRoll;
        }

        //Purpose: to allow the current dice roll to be retrieved
        //Returns: diceRoll[] which contains die1 and die2
        public int[] getDiceRoll()
        {
            return diceRoll;
        }

        //Purpose: sum the die of the current diceroll
        //Returns: integer sum of the dice
        public int sumDice()
        {
            int sum;
            sum = diceRoll[0] + diceRoll[1];
            return sum;
        }
    }
}
