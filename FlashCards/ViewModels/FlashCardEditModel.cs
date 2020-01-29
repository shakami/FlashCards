using FlashCards.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FlashCards.ViewModels
{
    public class FlashCardEditModel
    {
        [Required]
        public Card FlashCard { get; set; }
        public IEnumerable<Deck> Decks { get; set; }
    }
}
