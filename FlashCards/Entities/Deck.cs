using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FlashCards.Entities
{
    public class Deck
    {
        public List<Card> Cards;
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public Deck()
        {
            Cards = new List<Card>();
        }

        public void Add(Card flashCard)
        {
            Cards.Add(flashCard);
        }

        public bool Remove(Card flashCard)
        {
            return Cards.Remove(flashCard);
        }
    }
}
