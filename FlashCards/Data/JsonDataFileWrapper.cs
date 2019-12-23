using FlashCards.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlashCards.Data
{
    public class JsonDataFileWrapper
    {
        public int _deckId { get; set; }
        public int _flashCardId { get; set; }
        public List<Deck> Decks { get; set; }

        public JsonDataFileWrapper()
        {
            Decks = new List<Deck>();
        }

        public object GetInitialData()
        {
            return new JsonDataFileWrapper() { _deckId = 1, _flashCardId = 1, Decks = new List<Deck>() };
}
    }
}
