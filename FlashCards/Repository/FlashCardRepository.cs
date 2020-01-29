using FlashCards.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FlashCards.Repository
{
    public class FlashCardRepository : IFlashCardRepository
    {
        private readonly JsonDataContext _context = new JsonDataContext();

        public Deck AddDeck(Deck newDeck)
        {
            newDeck.Id = _context.GetNewDeckId();
            _context.Decks.Add(newDeck);
            return newDeck;
        }

        public IEnumerable<Deck> GetDecks()
        {
            return _context.Decks;
        }

        public Deck GetDeck(int deckId)
        {
            return _context.Decks.FirstOrDefault(d => d.Id == deckId);
        }

        public void DeleteDeck(Deck deck)
        {
            _context.Decks.Remove(deck);
        }

        public Card AddCard(Card newCard, int deckId)
        {
            var deck = GetDeck(deckId);
            if (deck == null)
            {
                return null;
            }
            newCard.DeckId = deckId;
            newCard.Id = _context.GetNewFlashCardId();
            deck.Add(newCard);
            return newCard;
        }

        public IEnumerable<Card> GetCards(int deckId)
        {
            return GetDeck(deckId)?.Cards;
        }

        public Card GetCard(int cardId)
        {
            return _context.Decks
                .SelectMany(d => d.Cards)
                .FirstOrDefault(f => f.Id == cardId);
        }

        public Card UpdateCard(Card updatedCard)
        {
            var oldCard = GetCard(updatedCard.Id);
            if (oldCard == null)
            {
                return null;
            }

            var deck = GetDeck(oldCard.DeckId);
            if (deck == null)
            {
                return null;
            }

            if (oldCard.DeckId == updatedCard.DeckId)
            {
                oldCard.Title = updatedCard.Title;
                oldCard.Description = updatedCard.Description;
            }
            else
            {
                deck.Remove(oldCard);
                deck = GetDeck(updatedCard.DeckId);
                deck.Add(updatedCard);
            }
            return updatedCard;
        }

        public void DeleteCard(Card cardToDelete)
        {
            if (cardToDelete != null)
            {
                var deck = GetDeck(cardToDelete.DeckId);
                deck.Remove(cardToDelete);
            }
        }

        public bool DeckExists(int deckId)
        {
            return _context.Decks.Any(d => d.Id == deckId);
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
