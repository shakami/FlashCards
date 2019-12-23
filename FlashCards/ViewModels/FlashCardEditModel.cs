using FlashCards.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FlashCards.ViewModels
{
    public class FlashCardEditModel
    {
        [Required]
        public FlashCard FlashCard { get; set; }

        public IEnumerable<Deck> Decks { get; set; }

    }
}
