//Author: Victoria Mak
//File Name: Hand.cs
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
    class Hand
    {
        private List<Card> cards;
        private int numMatches;

        public Hand()
        {
            cards = new List<Card>();
            numMatches = 0;
        }
        
        public int GetNumMatches()
        {
            return numMatches;
        }

        public int GetSize()
        {
            return cards.Count;
        }

        public Card GetCard(int idx)
        {
            return cards[idx];
        }

        public void Reset()
        {
            cards = new List<Card>();
            numMatches = 0;
        }

        public void DisplayHand(bool visible)
        {
            int padding = FindLeftPaddingToCenterCards();

            Console.ResetColor();
            Console.Write("".PadLeft(padding));

            for (int i = 0; i < GetSize(); i++)
            {
                Console.Write("┌───┐");
            }

            Console.WriteLine();
            Console.Write("".PadLeft(padding));

            for (int i = 0; i < GetSize(); i++)
            {
                Console.Write("|");
                cards[i].Display(visible);
                Console.Write("|");
            }

            Console.WriteLine();
            Console.Write("".PadLeft(padding));

            for (int i = 0; i < GetSize(); i++)
            {
                Console.Write("└───┘");
            }

            Console.WriteLine();

            if (GetSize() > 0 && visible)
            {
                Console.Write("Index:  ".PadLeft(padding + 2));
            
                for (int i = 0; i < GetSize(); i++)
                {
                    Console.Write(Convert.ToString(i).PadRight(Card.CARD_WIDTH));
                }
            }

            Console.WriteLine();
            Console.WriteLine();
        }

        public void AddCard(Card card)
        {
            cards.Add(card);
        }

        public int HasAPair()
        {
            for (int i = 0; i < cards.Count; i++)
            {
                if (HasCardMatch(cards[i]) != -1)
                {
                    return i;
                }
            }

            return -1;
        }

        public int HasCardMatch(Card card)
        {
            for (int i = 0; i < GetSize(); i++)
            {
                if (cards[i].MatchCard(card))
                {
                    return i;
                }
            }

            return -1;
        }

        public bool DropCards(int idx1, int idx2)
        {
            Card tempCard1;
            Card tempCard2;

            if (cards[idx1].GetRank().Equals(cards[idx2].GetRank()) && idx1 != idx2)
            {
                tempCard1 = GetCard(idx1);
                tempCard2 = GetCard(idx2);

                cards.Remove(tempCard1);
                cards.Remove(tempCard2);
                numMatches++;
                return true;
            }
           
            return false;
        }

        public Card StealCard(int idx)
        {
            Card tempCard;

            if (GetSize() > idx)
            {
                tempCard = cards[idx];
                cards.RemoveAt(idx);
                return tempCard;
            }

            return null;
        }

        private int FindLeftPaddingToCenterCards()
        {
            return (Program.WINDOW_WIDTH - GetSize() * Card.CARD_WIDTH) / 2;
        }
    }
}
