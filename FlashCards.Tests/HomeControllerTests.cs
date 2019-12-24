using FlashCards.Controllers;
using FlashCards.Models;
using FlashCards.Services;
using FlashCards.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace FlashCards.Tests
{
    [TestClass]
    public class HomeControllerTests
    {
        static IFlashCardData mockData;
        static HomeController controller;

        [ClassInitialize]
        public static void HomeControllerInitialize(TestContext context)
        {
            mockData = new JsonFlashCardData("./Data/testData.json");
            controller = new HomeController(mockData);
        }

        [TestMethod]
        public void IndexShouldPopulateAllDecks()
        {
            var indexResult = controller.Index() as ViewResult;

            var model = indexResult.Model as IEnumerable<Deck>;
            var expected = mockData.GetAllDecks().Count();

            Assert.AreEqual(expected, model.Count());
        }

        [TestMethod]
        public void DeckDisplayShouldPopulateCards()
        {
            var testDeckId = mockData.GetAllDecks().FirstOrDefault().Id;
            var testDeck = mockData.GetCardsInDeck(testDeckId);

            var deckDisplayResult = controller.DeckDisplay(testDeckId) as ViewResult;
            var model = deckDisplayResult.Model as Deck;

            Assert.AreEqual(testDeck.Count(), model.Cards.Count());
        }

        [TestMethod]
        public void DeleteDeckTest()
        {
            var testCard = new FlashCard
            {
                Title = "testCard",
                Description = "testCard Description"
            };
            var testDeck = new Deck
            {
                Name = "testDeck1"
            };

            testDeck = mockData.AddDeck(testDeck);
            var testDeckId = testDeck.Id;
            mockData.AddFlashCard(testCard, testDeckId);

            controller.DeleteDeck(testDeckId);

            Assert.IsTrue(!mockData.GetAllDecks().Contains(testDeck));
        }

        [TestMethod]
        public void DeleteFlashCardTest()
        {
            var testCard = new FlashCard
            {
                Title = "testCard",
                Description = "testCard Description"
            };
            var testDeck = new Deck
            {
                Name = "testDeck2"
            };

            testDeck = mockData.AddDeck(testDeck);
            var testDeckId = testDeck.Id;
            testCard = mockData.AddFlashCard(testCard, testDeckId);
            var testCardId = testCard.Id;
            Assert.IsNotNull(mockData.GetFlashCard(testCardId));

            controller.DeleteFlashCard(testCardId);

            Assert.IsNull(mockData.GetFlashCard(testCardId));

            // cleanup
            mockData.DeleteDeck(testDeckId);
        }

        [TestMethod]
        public void CreateFlashCardTest()
        {
            var testCard = new FlashCard
            {
                Title = "testCard",
                Description = "testCard Description"
            };
            var testDeck = new Deck
            {
                Name = "testDeck3"
            };

            testDeck = mockData.AddDeck(testDeck);
            var testDeckId = testDeck.Id;
            testCard.DeckId = testDeckId;

            controller.CreateFlashCard(new FlashCardEditModel()
            {
                Decks = mockData.GetAllDecks(),
                FlashCard = testCard
            });

            Assert.IsNotNull(mockData.GetCardsInDeck(testDeckId).Contains(testCard));

            // cleanup
            mockData.DeleteDeck(testDeckId);
        }

        [TestMethod]
        public void EditFlashCardTest()
        {
            var testCard = new FlashCard
            {
                Title = "testCard",
                Description = "testCard Description"
            };
            var testDeck = new Deck
            {
                Name = "testDeck4"
            };

            testDeck = mockData.AddDeck(testDeck);
            var testDeckId = testDeck.Id;
            testCard.DeckId = testDeckId;
            testCard = mockData.AddFlashCard(testCard, testDeckId);
            var testCardId = testCard.Id;

            var editedCard = new FlashCard
            {
                Id = testCardId,
                DeckId = testDeckId,
                Title = "edited",
                Description = "edited description"
            };

            controller.EditFlashCard(new FlashCardEditModel()
            {
                Decks = mockData.GetAllDecks(),
                FlashCard = editedCard
            });

            Assert.AreNotSame(testCard, mockData.GetFlashCard(testCardId));
            Assert.IsNotNull(mockData.GetCardsInDeck(testDeckId).Contains(editedCard));

            // cleanup
            mockData.DeleteDeck(testDeckId);
        }

        [TestMethod]
        public void CreateDeckTest()
        {
            var testDeck = new Deck
            {
                Name = "testDeck5"
            };
            Assert.IsFalse(mockData.GetAllDecks().Contains(testDeck));

            controller.CreateDeck(testDeck);

            Assert.IsTrue(mockData.GetAllDecks().Contains(testDeck));

            // cleanup
            testDeck = mockData.GetAllDecks().FirstOrDefault(d => d.Name == testDeck.Name);
            mockData.DeleteDeck(testDeck.Id);
        }

        [TestMethod]
        public void VerifyDeleteDeckTest()
        {
            var testDeck = new Deck
            {
                Name = "testDeck6"
            };
            testDeck = mockData.AddDeck(testDeck);
            var testDeckId = testDeck.Id;
            Assert.IsTrue(mockData.GetAllDecks().Contains(testDeck));
            
            var model = new VerifyDeleteViewModel
            {
                Id = testDeckId,
                ItemType = "deck"
            };
            var viewResult = controller.VerifyDeletePost(model) as RedirectToActionResult;
            Assert.AreEqual("DeleteDeck", viewResult.ActionName);

            var actual = (int)viewResult.RouteValues.ElementAtOrDefault(0).Value;
            Assert.AreEqual(testDeckId, actual);

            // cleanup
            mockData.DeleteDeck(testDeckId);
        }
    }
}