using FlashCards.Entities;
using System.Collections.Generic;

namespace FlashCards.Repository
{
    public interface IFlashCardRepository
    {
        Deck AddDeck(Deck newDeck);
        IEnumerable<Deck> GetDecks();
        Deck GetDeck(int deckId);
        void DeleteDeck(Deck deck);

        Card AddCard(Card newCard, int deckId);
        IEnumerable<Card> GetCards(int deckId);
        Card GetCard(int cardId);
        Card UpdateCard(Card updatedCard);
        void DeleteCard(Card card);

        bool DeckExists(int deckId);
        void Save();
    }
}
