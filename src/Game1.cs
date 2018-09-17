#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;
using System.IO;
using System.Reflection;
#endregion

namespace Solitaire
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Dictionary<CardSuite, List<Card>> suiteDictionary = new Dictionary<CardSuite, List<Card>>();
        Card selectedCard;
        Card deckRevealedCard;
        Deck deck;
        public static int deckCycle = 23;
        SpriteFont font;
        WinSlot[] winSlots = new WinSlot[4];
        Slot[] poolSlots = new Slot[7];
        MouseState mouseState;
        MouseState oldMouseState;

        public enum CardSuite { HEARTS, DIAMONDS, SPADES, CLUBS, NO_SUITE }

        public Game1()
            : base()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 865;
            graphics.PreferredBackBufferHeight = 560;
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            this.IsMouseVisible = true;
            mouseState = Mouse.GetState();
            oldMouseState = Mouse.GetState();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            string suite = null;
            CardSuite dictionaryKey = CardSuite.NO_SUITE;
            for (int suiteDecider = 0; suiteDecider < 4; suiteDecider++)
            {
                switch (suiteDecider)
                {
                    case 0:
                        suite = "h";
                        dictionaryKey = CardSuite.HEARTS;
                        break;
                    case 1:
                        suite = "d";
                        dictionaryKey = CardSuite.DIAMONDS;
                        break;
                    case 2:
                        suite = "c";
                        dictionaryKey = CardSuite.CLUBS;
                        break;
                    case 3:
                        suite = "s";
                        dictionaryKey = CardSuite.SPADES;
                        break;
                }
                List<Card> temp = new List<Card>();
                for (int i = 0; i < 13; i++)
                {
                    string cardGetter = (string)suite + (i + 1);
                    temp.Add(new Card(Content.Load<Texture2D>(@"cards/" + cardGetter), Content.Load<Texture2D>("cardBack"), true, dictionaryKey, i + 1));
                }
                for (int i = 0; i < 4; i++)
                {
                    winSlots[i] = new WinSlot(new Point(385 + 120 * i, 15), Content.Load<Texture2D>("slotHighlight"));
                }
                    font = Content.Load<SpriteFont>("Font");
                suiteDictionary.Add(dictionaryKey, temp);
                deck = new Deck(25, 10, Content.Load<Texture2D>("cardBack"), Content.Load<Texture2D>("slotHighlight"));
            }

            //make all the slot objects
            Random random = new Random();
            for (int i = 0; i < poolSlots.Length; i++)
            {
                poolSlots[i] = new Slot(25 + 120 * i, 140, Content.Load<Texture2D>(@"slotHighlight"));
                for (int c = 0; c < i + 1; c++)
                {
                    int cardNum;
                    switch (random.Next(4))
                    {
                        case 0:
                            if (suiteDictionary[CardSuite.CLUBS].Count == 0)
                            {
                                c--;
                            }
                            else
                            {
                                cardNum = random.Next(suiteDictionary[CardSuite.CLUBS].Count - 1);
                                poolSlots[i].addCard(suiteDictionary[CardSuite.CLUBS][cardNum]);
                                suiteDictionary[CardSuite.CLUBS][cardNum].cardsSlot = poolSlots[i];
                                suiteDictionary[CardSuite.CLUBS].RemoveAt(cardNum);
                            }
                            break;
                        case 1:
                            if (suiteDictionary[CardSuite.DIAMONDS].Count == 0)
                            {
                                c--;
                            }
                            else
                            {
                                cardNum = random.Next(suiteDictionary[CardSuite.DIAMONDS].Count - 1);
                                poolSlots[i].addCard(suiteDictionary[CardSuite.DIAMONDS][cardNum]);
                                suiteDictionary[CardSuite.DIAMONDS][cardNum].cardsSlot = poolSlots[i];
                                suiteDictionary[CardSuite.DIAMONDS].RemoveAt(cardNum);
                            }
                            break;
                        case 2:
                            if (suiteDictionary[CardSuite.HEARTS].Count == 0)
                            {
                                c--;
                            }
                            else
                            {
                                cardNum = random.Next(suiteDictionary[CardSuite.HEARTS].Count - 1);
                                poolSlots[i].addCard(suiteDictionary[CardSuite.HEARTS][cardNum]);
                                suiteDictionary[CardSuite.HEARTS][cardNum].cardsSlot = poolSlots[i];
                                suiteDictionary[CardSuite.HEARTS].RemoveAt(cardNum);
                            }
                            break;
                        case 3:
                            if (suiteDictionary[CardSuite.SPADES].Count == 0)
                            {
                                c--;
                            }
                            else
                            {
                                cardNum = random.Next(suiteDictionary[CardSuite.SPADES].Count - 1);
                                poolSlots[i].addCard(suiteDictionary[CardSuite.SPADES][cardNum]);
                                suiteDictionary[CardSuite.SPADES][cardNum].cardsSlot = poolSlots[i];
                                suiteDictionary[CardSuite.SPADES].RemoveAt(cardNum);
                            }
                            break;
                    }
                }
            }
            List<Card> clubs = suiteDictionary[CardSuite.CLUBS];
            List<Card> spades = suiteDictionary[CardSuite.SPADES];
            List<Card> diamonds = suiteDictionary[CardSuite.DIAMONDS];
            List<Card> hearts = suiteDictionary[CardSuite.HEARTS];
            while (deck.cardsInDeck.Count != 24)
            {
                int randomCardNumber;
                switch (random.Next(4))
                {
                    case 0:
                        if (clubs.Count != 0)
                        {
                            randomCardNumber = random.Next(clubs.Count - 1);
                            deck.cardsInDeck.Add(clubs[randomCardNumber]);
                            clubs.RemoveAt(randomCardNumber);
                        }
                        break;
                    case 1:
                        if (diamonds.Count != 0)
                        {
                            randomCardNumber = random.Next(diamonds.Count - 1);
                            deck.cardsInDeck.Add(diamonds[randomCardNumber]);
                            diamonds.RemoveAt(randomCardNumber);
                        }
                        break;
                    case 2:
                        if (hearts.Count != 0)
                        {
                            randomCardNumber = random.Next(hearts.Count - 1);
                            deck.cardsInDeck.Add(hearts[randomCardNumber]);
                            hearts.RemoveAt(randomCardNumber);
                        }
                        break;
                    case 3:
                        if (spades.Count != 0)
                        {
                            randomCardNumber = random.Next(spades.Count - 1);
                            deck.cardsInDeck.Add(spades[randomCardNumber]);
                            spades.RemoveAt(randomCardNumber);
                        }
                        break;
                }
            }
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            mouseState = Mouse.GetState();
            //NOTE I compare mouseState and oldMouseStates leftButton so it doesn't run repeatedly if button is held
            if (mouseState.LeftButton.Equals(ButtonState.Pressed) && mouseState.LeftButton != oldMouseState.LeftButton)
            {
                if (deck.cardsInDeck.Count != 0 && new Rectangle(deck.deckX, deck.deckY, 115, 120).Contains(Mouse.GetState().Position))
                {
                    if (deckCycle == deck.cardsInDeck.Count) { deckCycle--;}
                    deckRevealedCard = deck.cardsInDeck[deckCycle];
                    deckRevealedCard.cardLocation = new Rectangle(145, 10, 100, 120);
                    deckRevealedCard.isHidden = false;
                    selectedCard = null;
                    if (deck.cardsInDeck.Count != 0 && deckCycle == 0)
                    {
                        deckCycle = deck.cardsInDeck.Count - 1;
                    }
                    else
                    {
                        deckCycle--;
                    }
                }
                else if (deckRevealedCard != null && deckRevealedCard.cardLocation.Contains(Mouse.GetState().Position))
                {
                    selectedCard = deckRevealedCard;
                }
                else
                {
                    foreach (Slot slot in poolSlots)
                    {
                        for (int i = slot.cards.Count - 1; i >= 0; i--)
                        {
                            if (!slot.cards[i].isHidden)
                            {
                                if (slot.cards[i].collision(Mouse.GetState().Position))
                                {
                                    if (selectedCard != null)
                                    {
                                        if(slot.cards.Count != 0 && slot.cards[i].cardValue - 1 == selectedCard.cardValue && slot.cards[i].color != selectedCard.color)
                                        {
                                            if (selectedCard.cardsSlot != null)
                                            {
                                                int index = selectedCard.cardsSlot.cards.Count - 1;
                                                Slot selectedOldSlot = selectedCard.cardsSlot;
                                                List<Card> cardsToMove = new List<Card>();

                                                while (selectedOldSlot.cards.Contains(selectedCard))
                                                {
                                                    cardsToMove.Add(selectedOldSlot.cards[index]);
                                                    selectedOldSlot.cards[index].cardsSlot = slot;
                                                    selectedOldSlot.cards.RemoveAt(index);
                                                    index--;
                                                }
                                                int loopStart;
                                                if (cardsToMove.Count == 1)
                                                {
                                                    slot.addCard(cardsToMove[0]);
                                                }
                                                else
                                                {
                                                    loopStart = cardsToMove.Count - 1;
                                                    while (loopStart >= 0)
                                                    {
                                                        slot.addCard(cardsToMove[loopStart]);
                                                        loopStart--;
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                slot.addCard(selectedCard);
                                                selectedCard.cardsSlot = slot;
                                            }

                                        }
                                        if (selectedCard == deckRevealedCard)
                                        {
                                            deck.cardsInDeck.Remove(selectedCard);
                                            deckRevealedCard = null;
                                        }
                                        selectedCard = null;
                                    }
                                    else
                                    {
                                        selectedCard = slot.cards[i];
                                    }
                                }
                            }
                        }
                        if (slot.cards.Count == 0 && selectedCard != null && new Rectangle(slot.slotX, slot.slotY, 100, 120).Contains(Mouse.GetState().Position) && selectedCard.cardValue == 13)
                        {
                            if (selectedCard == deckRevealedCard)
                            {
                                slot.addCard(selectedCard);
                                selectedCard.cardsSlot = slot;
                                deck.cardsInDeck.Remove(selectedCard);
                                deckRevealedCard = null;
                            }
                            else
                            {
                                int index = selectedCard.cardsSlot.cards.Count - 1;
                                Slot selectedOldSlot = selectedCard.cardsSlot;
                                List<Card> cardsToMove = new List<Card>();

                                while (selectedOldSlot.cards.Contains(selectedCard))
                                {
                                    cardsToMove.Add(selectedOldSlot.cards[index]);
                                    selectedOldSlot.cards[index].cardsSlot = slot;
                                    selectedOldSlot.cards.RemoveAt(index);
                                    index--;
                                }
                                int loopStart = cardsToMove.Count - 1;
                                while (loopStart >= 0)
                                {
                                    slot.addCard(cardsToMove[loopStart]);
                                    loopStart--;
                                }

                            }
                            selectedCard = null;
                        }
                    }

                    foreach(WinSlot winSlot in winSlots)
                    {
                        if(winSlot.location.Contains(Mouse.GetState().Position) &&  selectedCard != null && winSlot.placeCard(selectedCard))
                        {
                            if(selectedCard == deckRevealedCard)
                            {
                                deck.cardsInDeck.Remove(deckRevealedCard);
                                deckRevealedCard = null;
                            }
                            else
                            {
                                selectedCard.cardsSlot.cards.RemoveAt(selectedCard.cardsSlot.cards.Count - 1);
                            }
                         
                            selectedCard = null;
                        }
                    }
                }
            }
            oldMouseState = Mouse.GetState();
            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.LightGreen);
            spriteBatch.Begin();
            //TODO add draw method calls
            deck.draw(spriteBatch);
            foreach (Slot poolSlot in poolSlots)
            {
                poolSlot.draw(spriteBatch);
            }
            foreach(WinSlot winSlot in winSlots)
            {
                winSlot.draw(spriteBatch);
            }
            spriteBatch.DrawString(font, "Selected Card Value:", new Vector2(10, graphics.PreferredBackBufferHeight - 25), Color.Black);
            if (selectedCard != null)
            {
                spriteBatch.DrawString(font, selectedCard.cardValue.ToString(), new Vector2(170, graphics.PreferredBackBufferHeight - 25), Color.Black);
            }
            else
            {
                spriteBatch.DrawString(font, "No value", new Vector2(170, graphics.PreferredBackBufferHeight - 25), Color.Black);
            }
            if (deckRevealedCard != null)
            {
                spriteBatch.Draw(deckRevealedCard.getTexture(), deckRevealedCard.cardLocation, Color.White);
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}