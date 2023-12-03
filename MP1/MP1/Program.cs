//Author: Victoria Mak
//File Name: Program.cs
//Project Name: MP1
//Creation Date: March 3, 2023
//Modified Date: March 24, 2023
//Description: Play the game, Go Fish, against the CPU where you try to get the most number of card matches.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MP1
{
    class Program
    {
        public const int WINDOW_WIDTH = 119;
        public const int WINDOW_HEIGHT = 35;

        const int USER = 0;
        const int CPU = 1;

        const int MENU = 0;
        const int PLAY = 1;
        const int EXIT = 2;

        const int STARTING_NUM_CARDS = 5;

        static Random rng = new Random();

        static Deck deck = new Deck();
        static Hand[] hands = new Hand[2];
        
        static bool isValid = false;
        static byte userChoice = 0;

        static List<string> availMatches = new List<string>();
        static List<int> userNumOfRanks = new List<int>();

        static int turn = USER;

        static void Main(string[] args)
        {
            hands[USER] = new Hand();
            hands[CPU] = new Hand();

            int gameState = MENU;

            Console.SetWindowSize(WINDOW_WIDTH, WINDOW_HEIGHT);

            while (gameState != EXIT)
            {
                switch (gameState)
                {
                    case PLAY:
                        DisplayGame();
                        UpdatePlay();

                        if (hands[USER].GetNumMatches() + hands[CPU].GetNumMatches() >= Deck.DECK_SIZE / 2)
                        {
                            Console.Clear();
                            Console.WriteLine("\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n");
                            DisplayInHighlight("CPU: " + hands[CPU].GetNumMatches() + " matches          YOU: " + hands[USER].GetNumMatches() + " matches", ConsoleColor.White);

                            if (hands[USER].GetNumMatches() > hands[CPU].GetNumMatches())
                            {
                                DisplayInHighlight("You won!", ConsoleColor.White);
                            }
                            else if (hands[USER].GetNumMatches() < hands[CPU].GetNumMatches())
                            {
                                DisplayInHighlight("The CPU won! Better luck next time.", ConsoleColor.White);
                            }
                            else
                            {
                                DisplayInHighlight("You tied with the CPU!", ConsoleColor.White);
                            }

                            gameState = MENU;

                            DisplayEnterMsg("to go back to the menu.");
                            Console.ReadLine();
                        }
                        break;

                    case MENU:
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        CenterMsg("Go Fish!");

                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("Choose an option:");
                        Console.WriteLine("1. Play");
                        Console.WriteLine("2. Exit");
                        Console.ResetColor();

                        isValid = Byte.TryParse(Console.ReadLine(), out userChoice);

                        switch (userChoice)
                        {
                            case PLAY:
                                hands[USER].Reset();
                                hands[CPU].Reset();
                                deck.ResetDeck();
                                availMatches.Clear();
                                userNumOfRanks.Clear();

                                for (int i = 0; i < STARTING_NUM_CARDS; i++)
                                {
                                    hands[CPU].AddCard(deck.DrawCard());
                                    hands[USER].AddCard(deck.DrawCard());

                                    AddAvailableMatch(hands[USER].GetCard(i));
                                }

                                turn = USER;
                                gameState = PLAY;
                                break;

                            case EXIT:
                                gameState = EXIT;
                                break;

                            default:
                                DisplayInHighlight("That choice is invalid.", ConsoleColor.Red);
                                DisplayEnterMsg(" to continue.");
                                Console.ReadLine();
                                break;
                        }

                        break;
                }
            }
        }

        static private void UpdatePlay()
        {
            switch (turn)
            {
                case USER:
                    ShowAvailMatches();

                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine();
                    Console.WriteLine("1. Ask for a card (If possible)");
                    Console.WriteLine("2. Drop a pair (If possible)");
                    Console.WriteLine("3. Draw a card (If possible)");
                    Console.Write("Choose: ");
                    Console.ResetColor();

                    isValid = Byte.TryParse(Console.ReadLine(), out userChoice);

                    switch (userChoice)
                    {
                        case 1:
                            AskForCard();
                            break;

                        case 2:
                            TryToDropCards();
                            break;

                        case 3:
                            DrawACardFromDeck();
                            break;

                        default:
                            DisplayInHighlight("Sorry, that's an invalid choice.", ConsoleColor.Red);
                            DisplayEnterMsg("to go back to your options.");
                            Console.ReadLine();
                            break;
                    }
                    break;

                case CPU:
                    DetermineCpuAction();
                    
                    DisplayEnterMsg("to continue");
                    Console.ReadLine();
                    break;
            }
        }

        private static void DisplayGame()
        {
            Console.Clear();

            if (turn == CPU)
            {
                DisplayInHighlight("**Active** ─CPU─ (Matches: " + hands[CPU].GetNumMatches() + ")", ConsoleColor.Green);
            }
            else
            {
                CenterMsg("─CPU─ (Matches: " + hands[CPU].GetNumMatches() + ")");
            }

            hands[CPU].DisplayHand(false);

            Console.ForegroundColor = ConsoleColor.Yellow;
            CenterMsg("deck size: " + deck.GetSize());

            if (!deck.IsEmpty())
            {
                CenterMsg("┌───┐");
                CenterMsg("|** |");
                CenterMsg("└───┘");
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine();
            }

            Console.ResetColor();
            Console.WriteLine();

            if (turn == USER)
            {
                DisplayInHighlight("**Active** ─YOU─ (Matches: " + hands[USER].GetNumMatches() + ")", ConsoleColor.Green);
            }
            else
            {
                CenterMsg("─YOU─ (Matches: " + hands[USER].GetNumMatches() + ")");
            }

            hands[USER].DisplayHand(true);
        }

        private static void AskForCard()
        {
            int cpuCardIdx;

            if (hands[USER].GetSize() > 0 && hands[CPU].GetSize() > 0)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Select the card index you would like to try to ask for a pair.");
                Console.Write("Choose: ");
                Console.ResetColor();

                isValid = Byte.TryParse(Console.ReadLine(), out userChoice);

                if (!isValid)
                {
                    DisplayInHighlight("Not valid - you have to enter an index number.", ConsoleColor.Red);
                    DisplayEnterMsg("to go back to your options.");

                }
                else if (userChoice < 0 || userChoice >= hands[USER].GetSize())
                {
                    DisplayInHighlight("Not valid - index must be between 0 and " + (hands[USER].GetSize() - 1) + ".", ConsoleColor.Red);
                    DisplayEnterMsg("to go back to your options.");
                }
                else
                {
                    cpuCardIdx = hands[CPU].HasCardMatch(hands[USER].GetCard(userChoice));

                    if (cpuCardIdx != -1)
                    {
                        DisplayInHighlight("The CPU has a " + hands[CPU].GetCard(cpuCardIdx).GetRank() + hands[CPU].GetCard(cpuCardIdx).GetSuit() + " and you stole it!", ConsoleColor.Blue);
                        DisplayEnterMsg("to continue.");

                        AddAvailableMatch(hands[CPU].GetCard(cpuCardIdx));
                        hands[USER].AddCard(hands[CPU].StealCard(cpuCardIdx));
                    }
                    else if (!deck.IsEmpty())
                    {
                        DisplayInHighlight("The CPU does not have a " + hands[USER].GetCard(userChoice).GetRank() + ". Go Fish!", ConsoleColor.Cyan);
                        DisplayEnterMsg("to Go Fish and end your turn.");

                        hands[USER].AddCard(deck.DrawCard());
                        AddAvailableMatch(hands[USER].GetCard(hands[USER].GetSize() - 1));

                        turn = CPU;
                    }
                    else
                    {
                        turn = CPU;

                        DisplayInHighlight("The CPU does not have a match and", ConsoleColor.DarkCyan);
                        DisplayInHighlight("there are no more cards to draw from the deck.", ConsoleColor.DarkCyan);
                        DisplayEnterMsg("to end your turn");
                    }

                }
            }
            else
            {
                DisplayInHighlight("You or the CPU has no cards so you can't ask for a card.", ConsoleColor.Red);
                DisplayEnterMsg("to select a different option.");
            }

            Console.ReadLine();
        }

        private static void TryToDropCards()
        {
            int firstIndex;
            int secIndex;

            string rank;

            if (hands[USER].HasAPair() != -1)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("Enter first index: ");
                Console.ResetColor();

                isValid = Int32.TryParse(Console.ReadLine(), out firstIndex);

                if (isValid && firstIndex >= 0 && firstIndex < hands[USER].GetSize())
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write("Enter second index: ");
                    Console.ResetColor();

                    isValid = Int32.TryParse(Console.ReadLine(), out secIndex);

                    if (isValid && secIndex >= 0 && secIndex < hands[USER].GetSize() && firstIndex != secIndex)
                    {
                        rank = hands[USER].GetCard(firstIndex).GetRank();

                        if (hands[USER].DropCards(firstIndex, secIndex))
                        {
                            RemoveAvailMatch(rank, 2);

                            DisplayInHighlight("You dropped the pair of " + rank + "'s!", ConsoleColor.Green);


                            if (hands[USER].GetSize() == 0 && deck.IsEmpty())
                            {
                                turn = CPU;
                                DisplayInHighlight("You have no cards to ask for, drop, or pick up. Your turn is over.", ConsoleColor.Magenta);
                            }
                        }
                        else
                        {
                            DisplayInHighlight("That's not a matching pair.", ConsoleColor.Red);
                            DisplayEnterMsg("to go back to your other options");
                        }
                    }
                    else
                    {
                        DisplayInHighlight("That's not a valid second index.", ConsoleColor.Red);
                        DisplayEnterMsg("to go back to your other options");
                    }
                }
                else
                {
                    DisplayInHighlight("That's not a valid first index.", ConsoleColor.Red);
                    DisplayEnterMsg("to go back to your other options.");
                }
            }
            else
            {
                DisplayInHighlight("You have no pairs to drop. Choose a different option.", ConsoleColor.Red);
                DisplayEnterMsg("to go back to your other options");
            }

            Console.ReadLine();
        }

        private static void DrawACardFromDeck()
        {
            if (!deck.IsEmpty())
            {
                hands[USER].AddCard(deck.DrawCard());
                AddAvailableMatch(hands[USER].GetCard(hands[USER].GetSize() - 1));

                turn = CPU;

                DisplayInHighlight("You drew a card from the deck.", ConsoleColor.Cyan);
                DisplayEnterMsg("to finish your turn.");
                Console.ReadLine();
            }
            else
            {
                DisplayInHighlight("There are no more cards to draw from the deck.", ConsoleColor.DarkCyan);
                DisplayEnterMsg("to choose a different option.");
                Console.ReadLine();
            }
        }

        private static void DetermineCpuAction()
        {
            int cpuCardIdx = hands[CPU].HasAPair();
            int userCardIdx;

            if (cpuCardIdx != -1)
            {
                DisplayInHighlight("The CPU dropped a pair of " + hands[CPU].GetCard(cpuCardIdx).GetRank() + "'s!", ConsoleColor.Green);

                hands[CPU].DropCards(cpuCardIdx, hands[CPU].HasCardMatch(hands[CPU].GetCard(cpuCardIdx)));
            }
            else if (hands[CPU].GetSize() > 0 && hands[USER].GetSize() > 0)
            {
                cpuCardIdx = rng.Next(0, hands[CPU].GetSize());
                userCardIdx = hands[USER].HasCardMatch(hands[CPU].GetCard(cpuCardIdx));

                if (userCardIdx != -1)
                {
                    RemoveAvailMatch(hands[CPU].GetCard(cpuCardIdx).GetRank(), 1);
                    hands[CPU].AddCard(hands[USER].StealCard(userCardIdx));

                    DisplayInHighlight("The CPU asked for a " + hands[CPU].GetCard(cpuCardIdx).GetRank() + ". It stole your " + hands[CPU].GetCard(cpuCardIdx).GetRank() + hands[CPU].GetCard(hands[CPU].GetSize() - 1).GetSuit() + "!", ConsoleColor.Blue);
                }
                else
                {
                    hands[CPU].AddCard(deck.DrawCard());
                    turn = USER;

                    DisplayInHighlight("The CPU asked for a " + hands[CPU].GetCard(cpuCardIdx).GetRank() + " but you didn't have it.", ConsoleColor.Cyan);
                    DisplayInHighlight("It was forced to Go Fish!", ConsoleColor.Cyan);
                }
            }
            else if (deck.GetSize() > 0)
            {
                hands[CPU].AddCard(deck.DrawCard());
                turn = USER;

                DisplayInHighlight("The CPU forced itself to go fish.", ConsoleColor.Cyan);
            }
            else
            {
                turn = USER;

                DisplayInHighlight("There are no available moves for the CPU.", ConsoleColor.Magenta); 
            }
        }

        private static void CenterMsg(string msg)
        {
            Console.WriteLine(("".PadLeft((WINDOW_WIDTH - msg.Length) / 2) + msg).PadRight(WINDOW_WIDTH));
        }

        private static void DisplayInHighlight(string msg, ConsoleColor color)
        {
            Console.BackgroundColor = color;
            Console.ForegroundColor = ConsoleColor.Black;
            CenterMsg(msg);
            Console.ResetColor();
        }

        private static void DisplayEnterMsg(string additionalMsg)
        {
            DisplayInHighlight("Press ENTER " + additionalMsg, ConsoleColor.Yellow);
        }

        private static void AddAvailableMatch(Card card)
        {
            if (hands[USER].HasCardMatch(card) != -1)
            {
                if (!availMatches.Contains(card.GetRank()))
                {
                    availMatches.Add(card.GetRank());
                    userNumOfRanks.Add(2);
                }
                else
                {
                    userNumOfRanks[availMatches.IndexOf(card.GetRank())]++;
                }
            }
        }

        private static void RemoveAvailMatch(string rank, int numCardsRemoved)
        {
            if (availMatches.Contains(rank))
            {
                userNumOfRanks[availMatches.IndexOf(rank)] -= numCardsRemoved;

                if (userNumOfRanks[availMatches.IndexOf(rank)] < 2)
                {
                    userNumOfRanks.RemoveAt(availMatches.IndexOf(rank));
                    availMatches.Remove(rank);
                }
            }
        }

        private static void ShowAvailMatches()
        {
            Console.ForegroundColor = ConsoleColor.Magenta;

            for (int i = 0; i < availMatches.Count; i++)
            {
                CenterMsg("You have " + (userNumOfRanks[i] / 2) + " pair(s) of " + availMatches[i] + "'s!");
            }

            Console.ResetColor();
        }
    }
}
