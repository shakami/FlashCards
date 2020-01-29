using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlashCards.Profiles
{
    public class DecksProfile : Profile
    {
        public DecksProfile()
        {
            CreateMap<Entities.Deck, Models.DeckDto>();
            CreateMap<Models.DeckForCreationDto, Entities.Deck>();
        }
    }
}
