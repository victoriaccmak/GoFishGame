using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MP1
{
    class Deck
    {
        public const int DECK_SIZE = 52;

        private List<Card> cards = new List<Card>();
        private Random rng = new Random();

        public Deck()
        {
            for (int i = 0; i < DECK_SIZE; i++)
            {
                cards.Add(new Card(i));
            }
        }

        public void ResetDeck()
        {
            cards.Clear();

            for (int i = 0; i < DECK_SIZE; i++)
            {
                cards.Add(new Card(i));
            }

            ShuffleDeck();
        }

        private void ShuffleDeck()
        {
            int rank;
            List<Card> temporaryCards = new List<Card>();

            while (cards.Count > 0)
            {
                rank = rng.Next(0, cards.Count);
                temporaryCards.Add(cards[rank]);
                cards.RemoveAt(rank);
            }

            cards = temporaryCards;
        }

        public Card DrawCard()
        {
            if (!IsEmpty())
            {
                Card temporaryCard = cards[0];
                cards.RemoveAt(0);
                return temporaryCard;
            }

            return null;
        }

        public bool IsEmpty()
        {
            return cards.Count <= 0;
        }

        public int GetSize()
        {
            return cards.Count;
        }
    }
}
