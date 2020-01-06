using FlashCards.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlashCards.ViewComponents
{
    public class NavbarDeckListViewComponent : ViewComponent
    {
        private readonly IFlashCardDataService _flashCardDataService;

        public NavbarDeckListViewComponent(IFlashCardDataService flashCardDataService)
        {
            _flashCardDataService = flashCardDataService;
        }

        public IViewComponentResult Invoke()
        {
            var model = _flashCardDataService.GetAllDecks();
            return View(model);
        }
    }
}
