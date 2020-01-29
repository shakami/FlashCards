using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FlashCards.Models;
using FlashCards.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FlashCards.Controllers
{
    [ApiController]
    public class DecksController : Controller
    {
        private readonly IFlashCardRepository _flashCardRepository;
        private readonly IMapper _mapper;

        public DecksController(IFlashCardRepository flashCardRepository,
                                IMapper mapper)
        {
            _flashCardRepository = flashCardRepository ??
                throw new ArgumentNullException(nameof(flashCardRepository));
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet("decks")]
        public IActionResult Index()
        {
            return View("GetDecks");
        }

        [HttpOptions("api/decks")]
        public IActionResult GetDecksOptions()
        {
            Response.Headers.Add("Allow", "GET,OPTIONS,POST");
            return Ok();
        }

        [HttpOptions("api/decks/{deckId}")]
        public IActionResult GetDeckOptions()
        {
            Response.Headers.Add("Allow", "GET,OPTIONS,DELETE");
            return Ok();
        }

        [HttpGet("api/decks")]
        public IActionResult GetDecks()
        {
            var decksFromRepo = _flashCardRepository.GetDecks();
            var decksToReturn = _mapper.Map<IEnumerable<DeckDto>>(decksFromRepo);
            return Ok(decksToReturn);
        }

        [HttpGet("api/decks/{deckId}", Name = "GetDeck")]
        public ActionResult<DeckDto> GetDeck(int deckId)
        {
            var deckFromRepo = _flashCardRepository.GetDeck(deckId);
            if (deckFromRepo == null)
            {
                return NotFound();
            }

            var deckToReturn = _mapper.Map<DeckDto>(deckFromRepo);
            return Ok(deckToReturn);
        }

        [HttpPost("api/decks")]
        public IActionResult CreateDeck(DeckForCreationDto deck)
        {
            var deckEntity = _mapper.Map<Entities.Deck>(deck);

            _flashCardRepository.AddDeck(deckEntity);
            _flashCardRepository.Save();

            var deckToReturn = _mapper.Map<DeckDto>(deckEntity);

            return CreatedAtRoute(nameof(GetDeck), new { deckId = deckToReturn.Id }, deckToReturn);
        }

        [HttpDelete("api/decks/{deckId}")]
        public IActionResult DeleteDeck(int deckId)
        {
            var deckFromRepo = _flashCardRepository.GetDeck(deckId);

            if (deckFromRepo == null)
            {
                return NotFound();
            }

            _flashCardRepository.DeleteDeck(deckFromRepo);
            _flashCardRepository.Save();

            return NoContent();
        }
    }
}