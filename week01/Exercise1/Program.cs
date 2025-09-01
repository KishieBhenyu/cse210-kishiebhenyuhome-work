using System;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Hello World! This is the Exercise1 Project.");
        Console.Write("Enter First Name: ");
string firstName = Console.ReadLine();

Console.Write("Enter Last Name: ");
string lastName = Console.ReadLine();

Console.WriteLine($"Your name is {lastName}, {firstName} {lastName}");
    }
}