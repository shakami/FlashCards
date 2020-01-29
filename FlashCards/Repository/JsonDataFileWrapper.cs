using FlashCards.Entities;
using System.Collections.Generic;

namespace FlashCards.Repository
{
    public class JsonDataFileWrapper
    {
        public int _deckId { get; set; }
        public int _cardId { get; set; }
        public List<Deck> Decks { get; set; }

        public JsonDataFileWrapper()
        {
            Decks = new List<Deck>();
        }

        public JsonDataFileWrapper GetInitialData()
        {
            return new JsonDataFileWrapper()
            {
                _deckId = 1,
                _cardId = 1,
                Decks = new List<Deck>()
            };
        }
    }
}
