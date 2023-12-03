//Author: Victoria Mak
//File Name: Card.cs
//Project Name: MP1
//Creation Date: March 3, 2023
//Modified Date:
//Description: 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MP1
{
    class Card
    {
        public const int CARD_WIDTH = 5;

        public const int NUM_RANKS = 13;
        private readonly string[] SUITS = { "H", "S", "D", "C" };
        private readonly ConsoleColor[] SUIT_COLOURS = { ConsoleColor.Red, ConsoleColor.Blue, ConsoleColor.Red, ConsoleColor.Blue };

        private string rank;
        private string suit;
        private ConsoleColor colour;

        public Card(int cardNum)
        {
            switch (cardNum % NUM_RANKS)
            {
                case 0:
                    rank = "A";
                    break;

                case 10:
                    rank = "J";
                    break;

                case 11:
                    rank = "Q";
                    break;

                case 12:
                    rank = "K";
                    break;

                default:
                    rank = Convert.ToString(cardNum % NUM_RANKS + 1);
                    break;
            }

            suit = SUITS[cardNum / NUM_RANKS];
            colour = SUIT_COLOURS[cardNum / NUM_RANKS];
        }

        public string GetRank()
        {
            return rank;
        }

        public string GetSuit()
        {
            return suit;
        }

        public void Display(bool visible)
        {
            if (visible)
            {
                Console.ForegroundColor = colour;
                Console.Write((rank + suit).PadRight(CARD_WIDTH - 2));
                Console.ResetColor();
            }
            else
            {
                Console.Write("** ");
            }
        }

        public bool MatchCard(Card card)
        {
            if (card.GetRank().Equals(GetRank()) && !card.GetSuit().Equals(GetSuit()))
            {
                return true;
            }

            return false;
        }
    }
}
