using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;
using System.Text;

namespace Solitaire
{
    /*
     * This class is going to be inherited by any slot that will contain a card 
     */

    //TODO Make abstract
    class Slot
    {
        //TODO change from Texture2D to Card
        public List<Card> cards = new List<Card>();
        public int slotX, slotY;
        private Texture2D slotHighlight;

        public Slot(int x, int y, Texture2D slotHighlight)
        {
            slotX = x;
            slotY = y;
            this.slotHighlight = slotHighlight;
        }

        public void addCard(Card newCard)
        {
            cards.Add(newCard);
        }

        public void draw(SpriteBatch spriteBatch)
        {
            if (cards.Count != 0)
            {
                cards.Last().isHidden = false;

                for (int i = 0; i < cards.Count; i++)
                {
                    spriteBatch.Draw(cards[i].getTexture(), new Rectangle(slotX, slotY + 25 * i, 100, 120), Color.White);
                    cards[i].setCardRectangle(slotX, slotY + 25 * i);
                }
            }
            else
            {
                spriteBatch.Draw(slotHighlight, new Rectangle(slotX, cards.Count != 0 ? slotY + 25 * (cards.Count - 1) : slotY, 100, 120), Color.White);
            }            
        }
    }
}