using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlashCards.Models;
using FlashCards.Services;
using Microsoft.AspNetCore.Mvc;

namespace FlashCards.Controllers
{
    [Route("Decks")]
    public class DecksController : Controller
    {
        private readonly IFlashCardRepository _flashCardData;

        public DecksController(IFlashCardRepository flashCardDataService)
        {
            _flashCardData = flashCardDataService ??
                throw new ArgumentNullException(nameof(flashCardDataService));
        }

        [HttpGet(Name = "GetDecks")]
        public IActionResult GetDecks()
        {
            var model = _flashCardData.GetAllDecks().ToList();
            return View(model);
        }

        [HttpGet("CreateDeck")]
        public IActionResult CreateDeck()
        {
            return View();
        }

        [HttpPost("CreateDeck")]
        public IActionResult CreateDeck(Deck model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction(nameof(CreateDeck), model);
            }
            _flashCardData.AddDeck(model);

            return RedirectToAction(nameof(GetDecks));
        }

        [HttpGet("{deckId}/Delete")]
        public IActionResult DeleteDeck(int deckId)
        {
            _flashCardData.DeleteDeck(deckId);
            return RedirectToAction(nameof(GetDecks));
        }
    }
}