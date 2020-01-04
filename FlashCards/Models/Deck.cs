using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FlashCards.Models
{
    public class Deck
    {
        public List<FlashCard> Cards;
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public Deck()
        {
            Cards = new List<FlashCard>();
        }

        public void Add(FlashCard flashCard)
        {
            Cards.Add(flashCard);
        }

        public bool Remove(FlashCard flashCard)
        {
            return Cards.Remove(flashCard);
        }
    }
}
