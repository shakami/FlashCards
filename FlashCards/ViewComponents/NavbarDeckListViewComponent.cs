using FlashCards.Repository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlashCards.ViewComponents
{
    public class NavbarDeckListViewComponent : ViewComponent
    {
        private readonly IFlashCardRepository _flashCardDataService;

        public NavbarDeckListViewComponent(IFlashCardRepository flashCardDataService)
        {
            _flashCardDataService = flashCardDataService;
        }

        public IViewComponentResult Invoke()
        {
            var model = _flashCardDataService.GetDecks();
            return View(model);
        }
    }
}
