using System;
using System.IO;
//Libaries for timing
using System.Diagnostics;
using System.Threading;

namespace CardGame
{

	class BlackJack
	{
		static void Main()
		{
			Console.WriteLine("BlackJack Simulator, multiple blackjack scenario testing");
			Console.WriteLine("1:BlackJack Simulation");
			Console.WriteLine("2:Dealer Stand/Bust Simulation");
			
			string input = Console.ReadLine();
			switch(input)
			{
				case "1":
					Play();
					break;
				case "2":
					DealerTester();
					break;
				default:
					Console.WriteLine("Please enter valid data");
					break;
			}
		}
		
		static void DealerTester()
		{
			Stopwatch timer = new Stopwatch();
			timer.Start();
			//Create a default deck (52 cards)
			Deck deck = new Deck();
			
			Hand dealerHand = new Hand();
			Hand playerHand = new Hand();
			
			//Get Player Cards
			playerHand.addCard(deck.pullCard());
			playerHand.addCard(deck.pullCard());
			
			//Console.WriteLine(playerHand);
			
			//Console.WriteLine("Score: "+playerHand.getScore());
			//Console.WriteLine("Aces: "+playerHand.getAces());

			//Get Dealer Cards

		
			//Dealer stuff

			
			int [] standCounter = new int[13] {0,0,0,0,0,0,0,0,0,0,0,0,0};
			int [] bustCounter = new int[13] {0,0,0,0,0,0,0,0,0,0,0,0,0};
			int trials = 1000000;
			
			for(int x = 0; x<trials; x++)
			{
				dealerHand.resetHand();
				Card dealerUpCard = deck.pullCard();
				dealerHand.addCard(dealerUpCard);
				dealerHand.addCard(deck.pullCard());
			
			
				while(dealerHand.getScore() < 17)
				{
					if(dealerHand.getAces() > 0 && (dealerHand.getScore() > 6 && dealerHand.getScore() < 12))
					{
						break;
					}
					
					//Add a card
					dealerHand.addCard(deck.pullCard());
				}
			
				//Print if we busted or didnt
				//Console.WriteLine(dealerHand);
			
				if(dealerHand.getScore() <= 21)
				{
					//Console.WriteLine("Stand");
					standCounter[dealerUpCard.rank]++;
				}
				else
				{
					//Console.WriteLine("Bust");
					bustCounter[dealerUpCard.rank]++;
				}
			}
			
			timer.Stop();
			
			Console.WriteLine("Time elapsed: "+timer.Elapsed+"\n");
			
			Console.WriteLine("2   |3   |4   |5   |6   |7   |8   |9   |10  |10  |10  |10  |A   |");
			//Print out Simulated Results vs. "Actual Results"
			for(int x = 0;x<13;x++)
			{
				Console.Write(string.Format("{0:0.00}",(double)bustCounter[x]/(double)(standCounter[x]+bustCounter[x]))+"|");
			}
			
		}
		
		static void Play()
		{
			//Create a default deck (52 cards)
			Deck deck = new Deck();
			
			Hand dealerHand = new Hand();
			Hand playerHand = new Hand();
			
			//Get Player Cards
			playerHand.addCard(deck.pullCard());
			playerHand.addCard(deck.pullCard());
			
			//Reset the dealer hand, grab the dealer's up card
			dealerHand.resetHand();
			Card dealerUpCard = deck.pullCard();
			dealerHand.addCard(dealerUpCard);
			dealerHand.addCard(deck.pullCard());
			
			//Print current game information
			
			Console.WriteLine("Dealer's Hand:");
			Console.WriteLine(dealerUpCard);
			Console.WriteLine("Facedown Card");
			Console.WriteLine();
			
			Console.WriteLine("Player's Hand");
			Console.WriteLine(playerHand);
			
			//Print the user's score, ask if they want to hit of stand
			
			//TODO: Move this to the hand class

			string input = "";
			while(input!= "S")
			{
				Console.WriteLine("Your score is: "+playerHand.getScore());
				if(playerHand.getScore() != playerHand.getScoreBest())
				{
					Console.Write(" or "+playerHand.getScoreBest());
				}
				Console.WriteLine("Would you like to (H)it or (S)tand?");
				input = Console.ReadLine();
				switch(input)
				{
					case "H":
					{
						playerHand.addCard(deck.pullCard());
						break;
					}
					case "S":
					{
						break;
					}
					default:
					{
						break;
					}
				}
				
				if(playerHand.getScore() > 21)
				{
					break;
				}
			}
			
			//If no player does not bust, keep hitting the dealer until he busts
			while(dealerHand.getScore() < 17)
			{
				if(dealerHand.getAces() > 0 && (dealerHand.getScore() > 6 && dealerHand.getScore() < 12))
				{
					break;
				}
				
				//Add a card
				dealerHand.addCard(deck.pullCard());
			}
		
			//Print the dealers final hand:
			Console.WriteLine("Dealer has the final hand");
			Console.WriteLine(dealerHand);
			Console.WriteLine("Player has the final hand");
			Console.WriteLine(playerHand);
		
			if(playerHand.getScoreBest() > 21)
			{
				Console.WriteLine("You busted! You lose :(");
			}
			else if (dealerHand.getScoreBest() > 21)
			{
				Console.WriteLine("Dealer busted! You Win!");
			}
			else if ( dealerHand.getScoreBest() > playerHand.getScoreBest())
			{
				Console.WriteLine("The dealer out-scored you. You lose");
			}
			else if (dealerHand.getScoreBest() < playerHand.getScoreBest())
			{
				Console.WriteLine("You outscored the dealer. You win!");
			}
			else
			{
				Console.WriteLine("Tie!");
			}
			
		}
	}

	class Card
	{
		static string [] Suits = new string[4] {"Clubs","Diamonds","Spades","Hearts"};
		static string [] Ranks = new string[13] {"Two","Three","Four","Five","Six","Seven","Eight","Nine","Ten","Jack","Queen","King","Ace"};
	

		public int suit;
		public int rank;
		
		public Card()
		{
			this.suit = -1;
			this.rank = -1;
		}
		
		public Card(int suit, int rank)
		{
			this.suit = suit;
			this.rank = rank;
		}
		
		public override string ToString()
		{
			if(this.suit < 0 || this.rank < 0)
			{	
				return "Invalid Card";
			}
			return Ranks[this.rank] + " of " + Suits[this.suit];
		}

	}

	class Deck1
	{
		int numDecks;
		int cardsLeft;
		Card[] cards;

		public Deck()
		{
			this.numDecks = 1;
			shuffle();
			
		}
		
		public Deck(int numDecks)
		{
			this.numDecks = numDecks;
			shuffle();

			
			
		}
		
		public void shuffle()
		{
			this.cards = new Card[52*numDecks];
			for(int x = 0; x < numDecks;x++)
			{
				for(int suit = 0; suit < 4; suit++)
				{
					for(int rank = 0; rank< 13; rank++)
					{
						this.cards[x*52 + suit*13 + rank] = new Card(suit,rank);
					}
				}
			}
			this.cardsLeft = 52*numDecks;
		}
		
		//pullCard: Takes a card from the deck ()

		public Card pullCard()
		{
			if(cardsLeft <= 0)
			{
				//No cards left in the deck
				shuffle();
				//Console.WriteLine("Error: No card left in the deck!");
				//return new Card();
			}
			Random rnd = new Random();
			
			Card pulledCard;
			

			int randomNum = rnd.Next(0,52*numDecks);
			while(this.cards[randomNum].suit == -1){
				randomNum = rnd.Next(0,52*numDecks);
			}
			pulledCard = this.cards[randomNum];
			this.cards[randomNum] = new Card();
			cardsLeft--;

			return pulledCard;
		}


	}
	
	class Deck
	{
		int numDecks;
		int cardsLeft;
		Card[] cards;
		int cardPointer;

		public Deck()
		{
			this.numDecks = 1;
			shuffle();
			
		}
		
		public Deck(int numDecks)
		{
			this.numDecks = numDecks;
			shuffle();

			
			
		}
		
		public void shuffle()
		{
			this.cards = new Card[52*numDecks];
			for(int x = 0; x < numDecks;x++)
			{
				for(int suit = 0; suit < 4; suit++)
				{
					for(int rank = 0; rank< 13; rank++)
					{
						this.cards[x*52 + suit*13 + rank] = new Card(suit,rank);
					}
				}
			}
			this.cardsLeft = 52*numDecks;
			
			Card [] shuffledDeck = new Card[52*numDecks];
			//SHuffle data deck dough
			Random rnd = new Random();
			
			Card pulledCard;
			
			foreach(Card card in this.cards)
			{
				int randomNum = rnd.Next(0,52*numDecks);
				while(shuffledDeck[randomNum].suit == -1){
					randomNum = rnd.Next(0,52*numDecks);
				}
				shuffledDeck[randomNum] = card;
			}
			cardPointer = 52*numDecks-1;
			//pulledCard = this.cards[randomNum];
			//this.cards[randomNum] = new Card();
			//cardsLeft--;

			//return pulledCard;
		}
		
		//pullCard: Takes a card from the deck ()

		public Card pullCard()
		{
			if(cardPointer < 0)
			{
				shuffle();
			}
			return this.cards[cardPointer];
			cardPointer--;
		}


	}
	
	//Class used for holding a dealt set of cards
	//TODO: Make child class for BlackJack Hand as to better organize
	class Hand
	{
	
	
		Card[] cards;
		int numCards;
		
		//Just for blackjack stuff, maybe move to child class?
		int score;
		int aces;
		
		int [] ScoringConverter = new int[13] {2,3,4,5,6,7,8,9,10,10,10,10,1};
		
		//
		
		public Hand()
		{
			cards = new Card[1];
			numCards = 0;
		}
		
		//Add a card to the user's hand
		public bool addCard(Card cardAdded)
		{
			numCards++;
			Card[] prevHand = cards;
			
			cards = new Card[numCards];
			
			for(int x = 0; x < numCards-1; x++)
			{
				cards[x] = prevHand[x];
			}
			cards[numCards-1] = cardAdded;
			
			return true;
		}
		
		public void resetHand()
		{
			cards = new Card[1];
			numCards = 0;
		}
		
		public override string ToString()
		{
			string toString = "The hand contains the following cards\n";
			foreach(Card card in cards)
			{
				toString += card.ToString() + "\n";
			}
			return toString;
		}
		
		//Get the hand score where aces = 1
		public int getScore()
		{
			int counter = 0;
			foreach(Card card in cards)
			{
				counter += ScoringConverter[card.rank];
			}
			return counter;
		}
		
		public int getScoreLow()
		{
			int counter = 0;
			foreach(Card card in cards)
			{
				counter += ScoringConverter[card.rank];
			}
			return counter;
		}
		
		//Get the best hand score that is less then 22 (aces can be 1 or 11)
		public int getScoreBest()
		{
			int counter = 0;
			foreach(Card card in cards)
			{
				counter += ScoringConverter[card.rank];
			}
			int aces = getAces();
			if(getScore() <= 11 && aces > 0)
			{
				counter += 10;
			}
			return counter;
		}
		
		public int getAces()
		{
			int counter = 0;
			foreach(Card card in cards)
			{
				if(card.rank == 12)
				{
					counter++;
				}
			}
			return counter;
		}

	}

}