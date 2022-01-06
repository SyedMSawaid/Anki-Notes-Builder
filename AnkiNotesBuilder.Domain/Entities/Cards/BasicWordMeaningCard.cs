using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnkiNotesBuilder.Domain.Entities.Cards
{
    public class BasicWordMeaningCard
    {
        public string Word { get; set; }
        public string PartOfSpeech { get; set; }
        public string Meaning { get; set; }
        public string Context { get; set; }
    }
}
