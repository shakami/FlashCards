using FlashCards.Models;
using System.Collections.Generic;

namespace FlashCards.Services
{
    public interface IFlashCardData
    {
        Deck AddDeck(Deck newDeck);
        void DeleteDeck(int deckId);
        FlashCard AddFlashCard(FlashCard newFlashCard, int deckId);
        FlashCard EditFlashCard(FlashCard updatedFlashCard);
        FlashCard GetFlashCard(int flashCardId);
        void DeleteFlashCard(int flashCardId);
        IEnumerable<FlashCard> GetCardsInDeck(int deckId);
        IEnumerable<Deck> GetAllDecks();
    }
}
