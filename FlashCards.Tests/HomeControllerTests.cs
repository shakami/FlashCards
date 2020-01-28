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
        static IFlashCardRepository mockData;
        static HomeController controller;

        [ClassInitialize]
        public static void HomeControllerInitialize(TestContext context)
        {
            mockData = new JsonFlashCardDataService("./Data/testData.json");
            controller = new HomeController(mockData);
        }

        [TestMethod]
        public void IndexShouldPopulateAllDecks()
        {
            //-- Arrange
            var indexResult = controller.Index() as ViewResult;

            //-- Act
            var model = indexResult.Model as IEnumerable<Deck>;
            var expected = mockData.GetAllDecks().Count();

            //-- Assert
            Assert.AreEqual(expected, model.Count());
        }

        [TestMethod]
        public void DeckDisplayShouldPopulateCards()
        {
            //-- Arrange
            var testDeckId = mockData.GetAllDecks().FirstOrDefault().Id;
            var testDeck = mockData.GetCardsInDeck(testDeckId);

            //-- Act
            var deckDisplayResult = controller.DeckDisplay(testDeckId) as ViewResult;
            var model = deckDisplayResult.Model as Deck;

            //-- Assert
            Assert.AreEqual(testDeck.Count(), model.Cards.Count());
        }

        [TestMethod]
        public void DeleteDeckTest()
        {
            //-- Arrange
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

            //-- Act
            controller.DeleteDeck(testDeckId);

            //-- Assert
            Assert.IsTrue(!mockData.GetAllDecks().Contains(testDeck));
        }

        [TestMethod]
        public void DeleteFlashCardTest()
        {
            //-- Arrange
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

            //-- Act
            controller.DeleteFlashCard(testCardId);

            //-- Assert
            Assert.IsNull(mockData.GetFlashCard(testCardId));

            // cleanup
            mockData.DeleteDeck(testDeckId);
        }

        [TestMethod]
        public void CreateFlashCardTest()
        {
            //-- Arrange
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

            //-- Act
            controller.CreateFlashCard(new FlashCardEditModel()
            {
                Decks = mockData.GetAllDecks(),
                FlashCard = testCard
            });

            //-- Assert
            Assert.IsNotNull(mockData.GetCardsInDeck(testDeckId).Contains(testCard));

            // cleanup
            mockData.DeleteDeck(testDeckId);
        }

        [TestMethod]
        public void EditFlashCardTest()
        {
            //-- Arrange
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
                Description = "esited description"
            };

            //-- Act
            controller.EditFlashCard(new FlashCardEditModel()
            {
                Decks = mockData.GetAllDecks(),
                FlashCard = editedCard
            });

            //-- Assert
            Assert.AreEqual(editedCard.Title, testCard.Title);
            Assert.AreEqual(editedCard.Description, testCard.Description);
            Assert.IsTrue(mockData.GetCardsInDeck(testDeckId).ToList().Contains(testCard));

            // cleanup
            mockData.DeleteDeck(testDeckId);
        }

        [TestMethod]
        public void CreateDeckTest()
        {
            //-- Arrange
            var testDeck = new Deck
            {
                Name = "testDeck5"
            };
            Assert.IsFalse(mockData.GetAllDecks().Contains(testDeck));

            //-- Act
            controller.CreateDeck(testDeck);

            //-- Assert
            Assert.IsTrue(mockData.GetAllDecks().Contains(testDeck));

            // cleanup
            testDeck = mockData.GetAllDecks().FirstOrDefault(d => d.Name == testDeck.Name);
            mockData.DeleteDeck(testDeck.Id);
        }

        [TestMethod]
        public void VerifyDeleteDeckTest()
        {
            //-- Arrange
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

            //-- Act
            var viewResult = controller.VerifyDeletePost(model) as RedirectToActionResult;

            //-- Assert
            Assert.AreEqual("DeleteDeck", viewResult.ActionName);
            var actual = (int)viewResult.RouteValues.ElementAtOrDefault(0).Value;
            Assert.AreEqual(testDeckId, actual);

            // cleanup
            mockData.DeleteDeck(testDeckId);
        }

        [TestMethod]
        public void VerifyDeleteFlashCardTest()
        {
            //-- Arrange
            var testDeck = new Deck
            {
                Name = "testDeck7"
            };
            testDeck = mockData.AddDeck(testDeck);
            var testDeckId = testDeck.Id;
            Assert.IsTrue(mockData.GetAllDecks().Contains(testDeck));

            var testCard = new FlashCard
            {
                DeckId = testDeckId,
                Title = "testCard",
                Description = "testCatdDescription"
            };
            testCard = mockData.AddFlashCard(testCard, testDeckId);
            var testCardId = testCard.Id;
            Assert.IsNotNull(mockData.GetFlashCard(testCardId));

            var model = new VerifyDeleteViewModel
            {
                Id = testCardId,
                ItemType = "flashCard"
            };

            //-- Act
            var viewResult = controller.VerifyDeletePost(model) as RedirectToActionResult;

            //-- Assert
            Assert.AreEqual("DeleteFlashCard", viewResult.ActionName);
            var actual = (int)viewResult.RouteValues.ElementAtOrDefault(0).Value;
            Assert.AreEqual(testCardId, actual);

            // cleanup
            mockData.DeleteDeck(testDeckId);
        }
    }
}