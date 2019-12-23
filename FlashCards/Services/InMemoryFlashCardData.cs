using FlashCards.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlashCards.Services
{
    /*
    public class InMemoryFlashCardData : IFlashCardData
    {
        private readonly List<Deck> decks;
        private protected int highestId = 1;

        public InMemoryFlashCardData()
        {
            var deck1 = new Deck("deck1") { Id = 1 };
            var deck2 = new Deck("deck2") { Id = 2 };
            var deck3 = new Deck("deck3") { Id = 3 };
            var deck4 = new Deck("deck4") { Id = 4 };

            decks = new List<Deck>() { deck1, deck2, deck3, deck4 };

            Add(new FlashCard
            {
                Id = 1,
                DeckId = 1,
                Title = "Flash Card One",
                Description = "this is our first flash card."
            });
            Add(new FlashCard
            {
                Id = 2,
                DeckId = 1,
                Title = "Flash Card Two",
                Description = "this is our second flash card."
            });
            Add(new FlashCard
            {
                Id = 5,
                DeckId = 1,
                Title = "Flash Card Five",
                Description = "this is our fifth flash card."
            });
            Add(new FlashCard
            {
                Id = 10,
                DeckId = 1,
                Title = "Flash Card Ten",
                Description = "this is our tenth flash card."
            });
            Add(new FlashCard
            {
                Id = 9,
                DeckId = 1,
                Title = "Flash Card Nine",
                Description = "this is our ninth flash card."
            });

            Add(new FlashCard
            {
                Id = 3,
                DeckId = 2,
                Title = "Flash Card Three",
                Description = "this is our third flash card."
            });
            Add(new FlashCard
            {
                Id = 4,
                DeckId = 2,
                Title = "Flash Card Four",
                Description = "this is our fourth flash card."
            });
            Add(new FlashCard
            {
                Id = 6,
                DeckId = 2,
                Title = "Flash Card Six",
                Description = "this is our sixth flash card."
            });

            Add(new FlashCard
            {
                Id = 7,
                DeckId = 3,
                Title = "Flash Card Seven",
                Description = "this is our seventh flash card."
            });


            Add(new FlashCard
            {
                Id = 8,
                DeckId = 4,
                Title = "Flash Card Eight",
                Description = "this is our eighth flash card."
            });
            Add(new FlashCard
            {
                Id = 11,
                DeckId = 4,
                Title = "Flash Card Eleven",
                Description = "this is our eleventh flash card."
            });
            Add(new FlashCard
            {
                Id = 12,
                DeckId = 4,
                Title = "Flash Card Twelve",
                Description = "this is our twelveth flash card."
            });
            Add(new FlashCard
            {
                Id = 13,
                DeckId = 4,
                Title = "Flash Card Thirteen",
                Description = "this is our thirteenth flash card."
            });
            Add(new FlashCard
            {
                Id = 14,
                DeckId = 4,
                Title = "Flash Card Fourteen",
                Description = "this is our fourteenth flash card."
            });

        }

        public FlashCard Add(FlashCard flashCard)
        {
            flashCard.Id = highestId++;
            decks[flashCard.DeckId - 1].Add(flashCard);

            return flashCard;
        }

        private IEnumerable<FlashCard> GetAll()
        {
            return decks.SelectMany(d => d.GetCards());
        }

        public IEnumerable<FlashCard> GetFlashCardsByDeck(int deckId)
        {
            return decks.Where(d => d.Id == deckId).FirstOrDefault().GetCards();
        }

        public FlashCard GetFlashCard(int flashCardId)
        {
            return GetAll().FirstOrDefault(f => f.Id == flashCardId);
        }

        IEnumerable<Deck> IFlashCardData.GetAllDecks()
        {
            return decks;
        }
    }
    */
}
