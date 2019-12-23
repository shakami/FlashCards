using FlashCards.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using FlashCards.Data;

namespace FlashCards.Services
{
    public class JsonFlashCardData : IFlashCardData
    {
        private readonly string dataPath;

        private JsonDataFileWrapper data;

        public JsonFlashCardData() : this("./Data/data.json")
        { }

        public JsonFlashCardData(string dataPath)
        {
            this.dataPath = dataPath;
            ReadFromJson();
            if (data == null)
            {
                data = new JsonDataFileWrapper();
                InitializeJson();
            }
        }

        private void ReadFromJson()
        {
            using StreamReader sr = new StreamReader(dataPath);
            try
            {
                data = JsonConvert.DeserializeObject<JsonDataFileWrapper>(sr.ReadToEnd());
            }
            catch (Exception)
            {
                data = new JsonDataFileWrapper();
                InitializeJson();
                ReadFromJson();
                throw;
            }
        }

        private void WriteToJson(Object output)
        {
            using StreamWriter sw = new StreamWriter(dataPath);
            sw.Write(JsonConvert.SerializeObject(output, Formatting.Indented));
        }

        private void InitializeJson()
        {
            using StreamWriter sw = new StreamWriter(dataPath);
            sw.Write(JsonConvert.SerializeObject(data.GetInitialData(), Formatting.Indented));
        }

        public Deck AddDeck(Deck newDeck)
        {
            newDeck.Id = GetNewDeckId();
            data.Decks.Add(newDeck);
            WriteToJson(data);
            return newDeck;
        }

        private int GetNewDeckId()
        {
            return data._deckId++;
        }

        public FlashCard AddFlashCard(FlashCard newFlashCard, int deckId)
        {
            var deck = GetDeck(deckId);
            if (deck == null)
            {
                return null;
            }
            
            newFlashCard.DeckId = deckId;
            newFlashCard.Id = GetNewFlashCardId();
            deck.Add(newFlashCard);
            WriteToJson(data);
            return newFlashCard;
        }

        private int GetNewFlashCardId()
        {
            return data._flashCardId++;
        }

        public IEnumerable<Deck> GetAllDecks()
        {
            return data.Decks;
        }

        public IEnumerable<FlashCard> GetCardsInDeck(int deckId)
        {
            return GetDeck(deckId)?.Cards;
        }

        public FlashCard GetFlashCard(int flashCardId)
        {
            return data.Decks.SelectMany(d => d.Cards).FirstOrDefault(f => f.Id == flashCardId);
        }

        public void DeleteDeck(int deckId)
        {
            data.Decks.Remove(GetDeck(deckId));
            WriteToJson(data);
        }

        public FlashCard EditFlashCard(FlashCard updatedFlashCard)
        {
            var oldCard = GetFlashCard(updatedFlashCard.Id);
            if (oldCard == null)
            {
                return null;
            }
            
            var deck = GetDeck(oldCard.DeckId);
            if (deck == null)
            {
                return null;
            }
            
            deck.Remove(oldCard);
            
            if (oldCard.DeckId != updatedFlashCard.DeckId)
            {
                deck = GetDeck(updatedFlashCard.DeckId);
            }
            deck.Add(updatedFlashCard);

            return updatedFlashCard;
        }

        public void DeleteFlashCard(int flashCardId)
        {
            var cardToDelete = GetFlashCard(flashCardId);

            if (cardToDelete != null)
            {
                var deck = GetDeck(cardToDelete.DeckId);
                deck.Remove(cardToDelete);
                WriteToJson(data);
            }
        }

        private Deck GetDeck(int deckId)
        {
            return data.Decks.FirstOrDefault(d => d.Id == deckId);
        }
    }
}
