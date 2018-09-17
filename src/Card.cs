using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Solitaire;

namespace Solitaire
{
    class Card
    {
        public Boolean isHidden {get; set;}
        public Texture2D cardFront {get; set;}
        public Texture2D cardBack{get; set;}
        public Rectangle cardLocation;
        public Game1.CardSuite cardsSuite;
        public Slot cardsSlot { get; set; }
        //1 = black, 2 = red
        public int color;
        public int cardValue;

        public Card(Texture2D card, Texture2D back, Boolean visibility, Game1.CardSuite suite, int value)
        {
            cardFront = card;
            cardBack = back;
            cardValue = value;
            isHidden = visibility;
            cardsSuite = suite;
            switch (suite)
            {
                case Game1.CardSuite.CLUBS:
                    color = 1;                    
                    break;
                case Game1.CardSuite.SPADES:
                    color = 1;
                    break;
                case Game1.CardSuite.DIAMONDS:
                    color = 2;
                    break;
                case Game1.CardSuite.HEARTS:
                    color = 2;
                    break;
            }
        }

        public Texture2D getTexture(){
            return isHidden ? cardBack : cardFront;
        }

        public void setCardRectangle(int x, int y)
        {
            cardLocation = new Rectangle(x, y, 100, 120);
        }

        public Boolean collision(Point mouseClick)
        {
            if(cardsSlot.cards[cardsSlot.cards.Count -1] == this)
            {
                return cardLocation.Contains(mouseClick);
            }
            else     
            {
                return new Rectangle(this.cardLocation.X, this.cardLocation.Y, this.cardLocation.Width, 25).Contains(mouseClick);
            }
        }
    }
}