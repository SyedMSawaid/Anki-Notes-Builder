using AnkiNotesBuilder.Application;
using AnkiNotesBuilder.Common;
using AnkiNotesBuilder.Domain.Entities.Cards;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TextToAnki
{
    class Program
    {
        static void Main(string[] args)
        {
            string filePath = @"D:\Personal\Personal Projects\AnkiNotesBuilder\TextToAnki\WordsList.txt";
            
            List<BasicWordMeaningCard> cards = ConvertToCards(filePath);

            string ankiReadyFile = @"D:\Personal\Personal Projects\AnkiNotesBuilder\TextToAnki\AnkiReadyFile.txt";

            bool result = WriteToFile(ankiReadyFile, cards);
            if (result)
            {
                Console.WriteLine("Successfully created Anki deck.");
            }
            else
            {
                Console.WriteLine("Some issues occured during creation of deck.");
            }
        }

        private static List<BasicWordMeaningCard> ConvertToCards(string filePath)
        {
            #region OLD LOGIC

            //string[] textLines = File.ReadAllLines(filePath);

            //List<BasicWordMeaningCard> cards = new List<BasicWordMeaningCard>();

            //for (int i = 0; i < textLines.Length; i += 5)
            //{
            //    if (textLines[i] == "")
            //    {
            //        throw new Exception("Empty line detected.");
            //    }
            //    BasicWordMeaningCard card = new BasicWordMeaningCard
            //    {
            //        Word = textLines[i],
            //        PartOfSpeech = textLines[i + 1],
            //        Meaning = textLines[i + 2],
            //        Context = textLines[i + 3]
            //    };
            //    cards.Add(card);
            //}

            //return cards;
            #endregion

            #region NEW LOGIC

            string[] textLines = File.ReadAllLines(filePath);
            List<BasicWordMeaningCard> cards = new List<BasicWordMeaningCard>();

            for (int i = 0; i < textLines.Length; i++)
            {
                int groupLength = 0;
                for (int j = i; j < textLines.Length; j++)
                {
                    if (textLines[j] == "")
                    {
                        break;
                    }
                    groupLength++;
                }

                int numberOfIteration = (groupLength - 1) / 3;
                string context = textLines[(i + groupLength) - 1];

                for (int j = 0, line = i; j < numberOfIteration; j++, line += 3)
                {
                    BasicWordMeaningCard card = new BasicWordMeaningCard
                    {
                        Word = TextUtilities.Capatalize(textLines[line]),
                        PartOfSpeech = textLines[line + 1],
                        Meaning = textLines[line + 2],
                        Context = context
                    };

                    cards.Add(card);
                }
                i += groupLength;
            }

            return cards;

            #endregion
        }

        private static bool WriteToFile(string ankiReadyFile, List<BasicWordMeaningCard> cards)
        {
            try
            {
                if (File.Exists(ankiReadyFile))
                {
                    File.Delete(ankiReadyFile);
                }

                using (StreamWriter sw = File.CreateText(ankiReadyFile))
                {
                    foreach (var card in cards)
                    {
                        sw.WriteLine(NoteFormats.BasicWordMeaning, card.Word, card.PartOfSpeech, card.Meaning, card.Context);
                    }
                }

                return true;
            }
            catch(Exception ex)
            {
                Console.WriteLine("Exception occured! Following is the information");
                Console.WriteLine(ex.Message);
                return false;
            }
        }
    }
}
