using FlashCards.Data;
using FlashCards.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FlashCards.Services
{
    public class JsonFlashCardDataService : IFlashCardDataService
    {
        private readonly string DataPath;
        private JsonDataFileWrapper data;

        public JsonFlashCardDataService() : this("./Data/data.json")
        { }

        public JsonFlashCardDataService(string dataPath)
        {
            this.DataPath = dataPath;
            ReadFromJson();
            if (data == null)
            {
                data = new JsonDataFileWrapper();
                InitializeJson();
            }
        }

        private void ReadFromJson()
        {
            try
            {
                using StreamReader sr = new StreamReader(DataPath);
                data = JsonConvert.DeserializeObject<JsonDataFileWrapper>(sr.ReadToEnd());
            }
            catch (Exception ex)
            {
                Exception dataExcepion = new FileLoadException("data file could not be read. invalid file/format.", ex);
                data = new JsonDataFileWrapper();
                InitializeJson();
                ReadFromJson();
                throw dataExcepion;
            }
        }

        private void WriteToJson(JsonDataFileWrapper output)
        {
            try
            {
                using StreamWriter sw = new StreamWriter(DataPath);
                sw.Write(JsonConvert.SerializeObject(output, Formatting.Indented));
            }
            catch (Exception ex)
            {
                throw new IOException("was not able to write to the data file.", ex);
            }
        }

        private void InitializeJson()
        {
            WriteToJson(data.GetInitialData());
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
            return data.Decks
                .SelectMany(d => d.Cards)
                .FirstOrDefault(f => f.Id == flashCardId);
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

            if (oldCard.DeckId == updatedFlashCard.DeckId)
            {
                oldCard.Title = updatedFlashCard.Title;
                oldCard.Description = updatedFlashCard.Description;
            }
            else
            {
                deck.Remove(oldCard);
                deck = GetDeck(updatedFlashCard.DeckId);
                deck.Add(updatedFlashCard);
            }

            WriteToJson(data);
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
