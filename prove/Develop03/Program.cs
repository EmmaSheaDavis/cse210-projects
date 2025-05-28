using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;

public class Program
{
    private Scripture _scripture;

    public static void Main()
    {
        Console.WriteLine("Welcome to the scripture memorization program!");
        Console.Write("Enter the scripture reference (example: Proverbs 3:5-6) ");
        string user_ref = Console.ReadLine();
        
        Console.WriteLine("Enter the scripture text");
        string user_txt = Console.ReadLine();


         if (string.IsNullOrWhiteSpace(user_ref) || string.IsNullOrWhiteSpace(user_txt))
        {
            Console.WriteLine("Refrence and text cannot be empty.");
            return;
        }

        string[] parts = user_ref.Split(new[] { " ", ":" }, StringSplitOptions.RemoveEmptyEntries);
        string book = parts[0];
        string chapter = parts[1];
        string verse = parts[2];

        if (parts.Length < 3)
        {
            Console.WriteLine("Invalid refrence format. Please us: Book Chapter:Verse");
            return;
        }

        int chapterAsInt;
        bool isChapterValid = int.TryParse(chapter, out chapterAsInt);
        if (!isChapterValid)
        {
            Console.WriteLine("Invalid chapter number.");
            return;
        }

        string[] verseParts = verse.Split("-");
        int startVerseInt, endVerseInt;
        var verseRanges = new List<(int, int)>();
        if (verseParts.Length == 2)
        {
            if (int.TryParse(verseParts[0], out startVerseInt) && int.TryParse(verseParts[1], out endVerseInt))
            {
                verseRanges.Add((startVerseInt, endVerseInt));
            }
            else
            {
                Console.WriteLine("Invalid verse range.");
                return;
            }
        }
        else if (verseParts.Length == 1)
        {
            if (int.TryParse(verseParts[0], out startVerseInt))
            {
                endVerseInt = startVerseInt;
                verseRanges.Add((startVerseInt, endVerseInt));
            }
            else
            {
                Console.WriteLine("Invalid verse number.");
                return;
            }
        }
        else
        {
            Console.WriteLine("Invalid verse format.");
            return;
        }

        Verses verses = new Verses(book, chapterAsInt, verseRanges);
        string text = user_txt;
        Program program = new Program();
        program._scripture = new Scripture(verses, text);
        program.RunProgram();

    }

    private void RunProgram()
    {
        while (!_scripture.IsCompletelyHidden())
        {
            ClearConsole();
            Console.WriteLine(_scripture.GetDisplayText());
            Console.WriteLine("\nPress Enter to hide more words or type 'quit' to exit:");
            string input = GetUserInput();

            if (input.ToLower() == "quit")
            {
                break;
            }

            // Hide 2-3 random non-hidden words
            _scripture.HideRandomWords(new Random().Next(2, 4));
        }

        // Final display when all words are hidden or user quits
        ClearConsole();
        Console.WriteLine(_scripture.GetDisplayText());
        Console.WriteLine("\nProgram ended. All words hidden or user quit.");
    }

    private void ClearConsole()
    {
        Console.Clear();
    }

    private string GetUserInput()
    {
        return Console.ReadLine()?.Trim() ?? "";
    }
}

public class Scripture
{
    private Verses _verses;
    private List<Word> _words;

    public Scripture(Verses verses, string text)
    {
        _verses = verses;
        _words = new List<Word>();
        string[] wordArray = text.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        foreach (string word in wordArray)
        {
            _words.Add(new Word(word));
        }
    }

    public string GetDisplayText()
    {
        string displayText = $"{_verses.GetDisplayVerses()}\n";
        displayText += string.Join(" ", _words.Select(word => word.GetDisplayText()));
        return displayText;
    }

    public bool HideRandomWords(int numToHide)
    {
        List<int> nonHiddenIndices = _words
            .Select((word, index) => new { Word = word, Index = index })
            .Where(w => !w.Word.IsHidden())
            .Select(w => w.Index)
            .ToList();

        if (nonHiddenIndices.Count == 0)
        {
            return false;
        }

        Random random = new Random();
        int wordsToHide = Math.Min(numToHide, nonHiddenIndices.Count);
        nonHiddenIndices = nonHiddenIndices.OrderBy(x => random.Next()).ToList();

        for (int i = 0; i < wordsToHide; i++)
        {
            _words[nonHiddenIndices[i]].Hide();
        }

        return true;
    }

    public bool IsCompletelyHidden()
    {
        return _words.All(word => word.IsHidden());
    }
}

public class Verses
{
    private string _book;
    private int _chapter;
    private List<(int start, int end)> _verseRanges;

    public Verses(string book, int chapter, int verse)
    {
        _book = book;
        _chapter = chapter;
        _verseRanges = new List<(int, int)> { (verse, verse) };
    }

    public Verses(string book, int chapter, List<int> verses)
    {
        _book = book;
        _chapter = chapter;
        _verseRanges = verses.Select(v => (v, v)).ToList();
    }

    public Verses(string book, int chapter, List<(int, int)> ranges)
    {
        _book = book;
        _chapter = chapter;
        _verseRanges = ranges;
    }

    public string GetDisplayVerses()
    {
        var sortedRanges = _verseRanges.OrderBy(r => r.start).ToList();
        var verseStrings = new List<string>();

        foreach (var (start, end) in sortedRanges)
        {
            if (start == end)
                verseStrings.Add($"{start}");
            else
                verseStrings.Add($"{start}-{end}");
        }

        return $"{_book} {_chapter}:{string.Join(",", verseStrings)}";
    }
}

public class Word
{
    private string _text;
    private bool _isHidden;

    public Word(string text)
    {
        _text = text;
        _isHidden = false;
    }

    public void Hide()
    {
        _isHidden = true;
    }

    public bool IsHidden()
    {
        return _isHidden;
    }

    public string GetDisplayText()
    {
        return _isHidden ? "____" : _text;
    }
}
