using FlashCards.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FlashCards.ViewModels
{
    public class FlashCardEditModel
    {
        [Required]
        public FlashCard FlashCard { get; set; }
        public IEnumerable<Deck> Decks { get; set; }
    }
}
