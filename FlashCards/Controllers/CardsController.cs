using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlashCards.Models;
using FlashCards.Services;
using FlashCards.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace FlashCards.Controllers
{
    [Route("Decks/{deckId}/Cards")]
    public class CardsController : Controller
    {
        private readonly IFlashCardRepository _flashCardData;

        public CardsController(IFlashCardRepository flashCardDataService)
        {
            _flashCardData = flashCardDataService ??
                throw new ArgumentNullException(nameof(flashCardDataService));
        }

        [HttpGet]
        public IActionResult GetCards(int deckId)
        {
            var model = _flashCardData.GetAllDecks().FirstOrDefault(d => d.Id == deckId);
            return View(model);
        }

        [HttpGet("CreateFlashCard")]
        public IActionResult CreateFlashCard(int deckId)
        {
            var model = new FlashCardEditModel
            {
                Decks = _flashCardData.GetAllDecks(),
                FlashCard = new FlashCard { DeckId = deckId }
            };
            return View(model);
        }

        [HttpPost("CreateFlashCard")]
        public IActionResult CreateFlashCard(FlashCardEditModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction(nameof(CreateFlashCard), model);
            }
            FlashCard newFlashCard = new FlashCard
            {
                Title = model.FlashCard.Title,
                Description = model.FlashCard.Description,
                DeckId = model.FlashCard.DeckId
            };

            newFlashCard = _flashCardData.AddFlashCard(newFlashCard, newFlashCard.DeckId);
            return RedirectToAction(nameof(GetCards), new { deckId = newFlashCard.DeckId });
        }

        [HttpGet("{flashCardId}/Edit")]
        public IActionResult EditFlashCard(int flashCardId)
        {
            FlashCardEditModel model = new FlashCardEditModel
            {
                FlashCard = _flashCardData.GetFlashCard(flashCardId),
                Decks = _flashCardData.GetAllDecks()
            };
            return View(model);
        }

        [HttpPost("{flashCardId}/Edit")]
        public IActionResult EditFlashCard(FlashCardEditModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction(nameof(EditFlashCard), model);
            }

            _flashCardData.EditFlashCard(model.FlashCard);
            return RedirectToAction(nameof(GetCards), new { deckId = model.FlashCard.DeckId });
        }

        [HttpGet("{flashCardId}/Delete")]
        public IActionResult DeleteFlashCard(int flashCardId)
        {
            var deckId = _flashCardData.GetFlashCard(flashCardId).DeckId;
            _flashCardData.DeleteFlashCard(flashCardId);
            return RedirectToAction(nameof(GetCards), new { deckId = deckId });
        }
    }
}