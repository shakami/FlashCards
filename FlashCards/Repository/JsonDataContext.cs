using FlashCards.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FlashCards.Repository
{
    public class JsonDataContext
    {
        private readonly string _dataPath;
        private JsonDataFileWrapper data;

        public List<Deck> Decks
        {
            get{
                return data.Decks;
            }
        }

        public JsonDataContext() : this("./Data/data.json")
        { }

        public JsonDataContext(string dataPath)
        {
            _dataPath = dataPath;
            ReadFromJson();
            if (data == null)
            {
                data = new JsonDataFileWrapper();
                InitializeJson();
            }
        }

        public int GetNewDeckId()
        {
            return data._deckId++;
        }

        public int GetNewFlashCardId()
        {
            return data._cardId++;
        }

        private void ReadFromJson()
        {
            try
            {
                using StreamReader sr = new StreamReader(_dataPath);
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
                using StreamWriter sw = new StreamWriter(_dataPath);
                sw.Write(JsonConvert.SerializeObject(output, Formatting.Indented));
            }
            catch (Exception ex)
            {
                throw new IOException("was not able to write to the data file.", ex);
            }
        }

        public void SaveChanges()
        {
            WriteToJson(data);
        }

        private void InitializeJson()
        {
            WriteToJson(data.GetInitialData());
        }
    }
}
