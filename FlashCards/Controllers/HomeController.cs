using FlashCards.Models;
using FlashCards.Services;
using FlashCards.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Linq;

namespace FlashCards.Controllers
{
    public class HomeController : Controller
    {
        private readonly IFlashCardDataService _flashCardData;

        public HomeController(IFlashCardDataService flashCardData)
        {
            _flashCardData = flashCardData;
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Index()
        {
            var model = _flashCardData.GetAllDecks().ToList();
            return View(model);
        }

        [HttpGet]
        public IActionResult DeckDisplay(int id)
        {
            var model = _flashCardData.GetAllDecks().FirstOrDefault(d => d.Id == id);
            return View(model);
        }

        public IActionResult DeleteDeck(int id)
        {
            _flashCardData.DeleteDeck(id);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult DeleteFlashCard(int id)
        {
            var deckId = _flashCardData.GetFlashCard(id).DeckId;
            _flashCardData.DeleteFlashCard(id);
            return RedirectToAction(nameof(DeckDisplay), new { id = deckId });
        }

        [HttpGet]
        public IActionResult CreateFlashCard(int deckId)
        {
            var model = new FlashCardEditModel
            {
                Decks = _flashCardData.GetAllDecks(),
                FlashCard = new FlashCard { DeckId = deckId }
            };
            return View(model);
        }

        [HttpPost]
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
            return RedirectToAction(nameof(DeckDisplay), new { id = newFlashCard.DeckId });
        }

        [HttpGet]
        public IActionResult EditFlashCard(int flashCardId)
        {
            FlashCardEditModel model = new FlashCardEditModel
            {
                FlashCard = _flashCardData.GetFlashCard(flashCardId),
                Decks = _flashCardData.GetAllDecks()
            };
            return View(model);
        }


        [HttpPost]
        public IActionResult EditFlashCard(FlashCardEditModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction(nameof(EditFlashCard), model);
            }

            _flashCardData.EditFlashCard(model.FlashCard);
            return RedirectToAction(nameof(DeckDisplay), new { id = model.FlashCard.DeckId });
        }

        [HttpGet]
        public IActionResult CreateDeck()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateDeck(Deck model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction(nameof(CreateDeck), model);
            }
            _flashCardData.AddDeck(model);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult VerifyDelete(VerifyDeleteViewModel model)
        {
            return View(model);
        }

        public IActionResult VerifyDeletePost(VerifyDeleteViewModel model)
        {
            if (model.ItemType == "flashCard")
            {
                return RedirectToAction(nameof(DeleteFlashCard), new { model.Id });
            }
            else if (model.ItemType == "deck")
            {
                return RedirectToAction(nameof(DeleteDeck), new { model.Id });
            }
            else
            {
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
