using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Solitaire
{
    class Deck
    {
        private Texture2D cardBack;
        private Texture2D slotHighlight;
        public int deckX, deckY;
        public List<Card> deck = new List<Card>();
        public List<Card> cardsInDeck = new List<Card>();

        public Deck(int x, int y, Texture2D cardBack, Texture2D slotHighlight)
        {
            deckX = x;
            deckY = y;
            this.cardBack = cardBack;
            this.slotHighlight = slotHighlight;
        }

        public void draw(SpriteBatch spriteBatch)
        {
            if(Game1.deckCycle == 0)
            {
                spriteBatch.Draw(slotHighlight, new Rectangle(deckX,deckY, 115, 120), Color.White);
            }
            else
            {
                for(int i = 0; i < 3; i++)
                {
                    spriteBatch.Draw(cardBack, new Rectangle(deckX + 5*i, deckY, 100, 120), Color.White);
                }
            }
        }
    }
}