using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlashCards.Models
{
    public class DeckForCreationDto
    {
        public string Name { get; set; }

        public ICollection<CardForCreationDto> Cards { get; set; }
            = new List<CardForCreationDto>();
    }
}
