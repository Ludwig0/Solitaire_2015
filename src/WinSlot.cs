using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Solitaire
{
    class WinSlot
    {
        private Texture2D slotHighlight;
        private Card[] cards = new Card[13];
        public Rectangle location;

        public WinSlot(Point location, Texture2D slot)
        {
            slotHighlight = slot;
            this.location = new Rectangle(location.X, location.Y, 100, 120);
        }

        public int cardsInSlot()
        {
            int cardsInSlot = 0;
            while(cards[cardsInSlot] != null)
            {
                cardsInSlot++;
            }
            return cardsInSlot;
        }

        public void draw(SpriteBatch spriteBatch)
        {
            if(cardsInSlot() == 0)
            {
                spriteBatch.Draw(slotHighlight, location, Color.White);
            }
            else
            {
                spriteBatch.Draw(cards[cardsInSlot()-1].getTexture(), location, Color.White);
            }
        }

        public Boolean placeCard(Card selectedCard)
        {
            if (cards[0] == null || selectedCard.cardsSuite.Equals(cards[cardsInSlot() - 1].cardsSuite))
            {
                if (selectedCard.cardValue == cardsInSlot() + 1)
                {
                    if(selectedCard.cardsSlot == null || selectedCard.cardsSlot.cards.Last() == selectedCard)
                    {
                    selectedCard.isHidden = false;
                    cards[cardsInSlot()] = selectedCard;
                    return true;
                    }
                }
            }
            return false;
        }
    }
}
