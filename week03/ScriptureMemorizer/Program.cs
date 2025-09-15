using System;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        Reference reference = new Reference("Proverbs", 3, 5, 6);
        Scripture scripture = new Scripture(reference,
            "Trust in the Lord with all thine heart; and lean not unto thine own understanding.");

        while (true)
        {
            Console.Clear();
            Console.WriteLine(scripture.GetDisplayText());
            Console.WriteLine("\nPress Enter to continue or type 'quit' to exit.");
            string input = Console.ReadLine();

            if (input.ToLower() == "quit")
                break;

            if (scripture.AllWordsHidden())
                break;

            scripture.HideRandomWords(3);
        }
    }
}

class Reference
{
    private string _book;
    private int _chapter;
    private int _verse;
    private int _endVerse;

    // Single verse
    public Reference(string book, int chapter, int verse)
    {
        _book = book;
        _chapter = chapter;
        _verse = verse;
    }

    // Verse range
    public Reference(string book, int chapter, int startVerse, int endVerse)
    {
        _book = book;
        _chapter = chapter;
        _verse = startVerse;
        _endVerse = endVerse;
    }

    public string GetDisplayText()
    {
        if (_endVerse == 0)
            return $"{_book} {_chapter}:{_verse}";
        else
            return $"{_book} {_chapter}:{_verse}-{_endVerse}";
    }
}

class Word
{
    private string _text;
    private bool _hidden;

    public Word(string text)
    {
        _text = text;
        _hidden = false;
    }

    public void Hide()
    {
        _hidden = true;
    }

    public string GetDisplayText()
    {
        return _hidden ? new string('_', _text.Length) : _text;
    }

    public bool IsHidden()
    {
        return _hidden;
    }
}

class Scripture
{
    private Reference _reference;
    private List<Word> _words;
    private Random _random = new Random();

    public Scripture(Reference reference, string text)
    {
        _reference = reference;
        _words = new List<Word>();
        foreach (string word in text.Split(' '))
            _words.Add(new Word(word));
    }

    public string GetDisplayText()
    {
        List<string> displayWords = new List<string>();
        foreach (Word word in _words)
            displayWords.Add(word.GetDisplayText());

        return $"{_reference.GetDisplayText()} - {string.Join(" ", displayWords)}";
    }

    public void HideRandomWords(int count)
    {
        for (int i = 0; i < count; i++)
        {
            int index = _random.Next(_words.Count);
            _words[index].Hide();
        }
    }

    public bool AllWordsHidden()
    {
        foreach (Word word in _words)
            if (!word.IsHidden())
                return false;
        return true;
    }
}