using System;
using System.Security.Cryptography.X509Certificates;
using Microsoft.VisualBasic;

class Program
{
    private static Catalog catalog = new Catalog();
    private static List<Loan> loans = new List<Loan>();
    private static User user = new User(1, "John Doe", "john@example.com");
    private static ReportGenerator reportGenerator = new ReportGenerator(catalog, loans);

    public void DisplayMenu()
    {
        Console.WriteLine("\nLibrary Management System Menu:");
        Console.WriteLine("1. Add Item");
        Console.WriteLine("2. Borrow Item");
        Console.WriteLine("3. Return Item");
        Console.WriteLine("4. Check Availability");
        Console.WriteLine("5. Calculate Late Fee");
        Console.WriteLine("6. Generate Inventory Report");
        Console.WriteLine("7. Exit");
        Console.WriteLine();
    }
    public void Run()
    {
        DateTime currentTime = new DateTime(2025, 7, 9, 8, 48, 0); // 08:48 AM MDT, July 9, 2025
        bool running = true;

        while (running)
        {
            Console.Clear();
            DisplayMenu();
            Console.WriteLine();
            Console.WriteLine("Select an option(1-7): ");
            string choice = Console.ReadLine();

            if (choice == "1")
            {

            }
            else if (choice == "2")
            {

            }
            else if (choice == "3")
            {

            }
            else if (choice == "4")
            {

            }
            else if (choice == "5")
            {

            }
            else if (choice == "6")
            {

            }
            else if (choice == "7")
            {
                running = false;
                Console.Clear();
                Console.WriteLine("Thank you for using the Library System");
                break;
            }
            else
            {
                Console.WriteLine("Invalid option. Please try again.");
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
            }

        }
    }

    static void Main(string[] args)
    {
        Program program = new Program();
        program.Run();
    }
}
public abstract class LibraryItem
{
    public LibraryItem(string itemId, string title, bool isAvailable)
    {
        ItemId = itemId;
        Title = title;
        IsAvailable = isAvailable;
    }
    protected string ItemId { get; set; }
    protected string Title { get; set; }
    protected bool IsAvailable { get; set; }

    public abstract bool CheckAvailability();
    public abstract decimal CalculateLateFee(DateTime dueDate, DateTime returnDate);
}
public class Magazine : LibraryItem
{
    private string _issueNumber;
    private DateTime _publicationDate;

    public Magazine(string itemId, string title, bool isAvailable, string issueNumber, DateTime publicationDate) : base(itemId, title, isAvailable)
    {
        issueNumber = _issueNumber;
        publicationDate = _publicationDate;
    }
    public override bool CheckAvailability()
    {
        throw new NotImplementedException();
    }
    public override decimal CalculateLateFee(DateTime dueDate, DateTime returnDate)
    {
        throw new NotImplementedException();
    }
}
public class Book : LibraryItem
{
    private string _isbn;
    private string _author;
    private int _publicationYear;

    public Book(string itemId, string title, bool isAvailable, string isbn, string author, int publicationYear) : base(itemId, title, isAvailable)
    {
        isbn = _isbn;
        author = _author;
        publicationYear = _publicationYear;
    }
    public override bool CheckAvailability()
    {
        throw new NotImplementedException();
    }
    public override decimal CalculateLateFee(DateTime dueDate, DateTime returnDate)
    {
        throw new NotImplementedException();
    }

}
public class DVD : LibraryItem
{
    private int _runTime;
    public string _format;

    public DVD(string itemId, string title, bool isAvailable, int runTime, string format) : base(itemId, title, isAvailable)
    {
        runTime = _runTime;
        format = _format;
    }
    public override bool CheckAvailability()
    {
        throw new NotImplementedException();
    }
    public override decimal CalculateLateFee(DateTime dueDate, DateTime returnDate)
    {
        throw new NotImplementedException();
    }

}
public class User
{
    private int _userId;
    private string _name;
    private string _email;
    private List<Loan> _loans = new List<Loan>();

    public User(int userId, string name, string email)
    {
        userId = _userId;
        name = _name;
        email = _email;
    }

    public void BorrowItem()
    {

    }
    public void GetLoanHistory()
    {

    }
}
public class Loan
{
    private int _loanId;
    private LibraryItem _item;
    private User _user;
    private int _borrowDate;
    private int _returnDate;
    private int _dueDate;

    public Loan(int loanId, LibraryItem item, User user, int borrowDate, int returnDate, int dueDate)
    {
        loanId = _loanId;
        item = _item;
        user = _user;
        borrowDate = _borrowDate;
        returnDate = _returnDate;
        dueDate = _dueDate;
    }
    public bool IsOverdue()
    {
        throw new NotImplementedException();
    }
    public decimal CalculateTotalLateFee()
    {
        throw new NotImplementedException();
    }
    public void ReturnItem()
    {

    }

}
public class Catalog
{
    private List<LibraryItem> _items = new List<LibraryItem>();
    public Catalog()
    { }
    public void AddItem(LibraryItem item)
    {

    }
    public void RemoveItem(LibraryItem item)
    {

    }
    public List<LibraryItem> Searchitems(string query)
    {
        throw new NotImplementedException();
    }
    public List<LibraryItem> GetAvailableItems()
    {
        throw new NotImplementedException();
    }

}
public class ReportGenerator
{
    private Catalog _catalog;
    private List<Loan> _loans = new List<Loan>();

    public ReportGenerator(Catalog catalog, List<Loan> loans)
    {
        catalog = _catalog;
        loans = _loans;
    }
    public string GenerateInventory()
    {
        throw new NotImplementedException();
    }
    public string GenerateBorrowingTrends()
    {
        throw new NotImplementedException();
    }

}
public class FileManager
{
    private string _filepath;

    public FileManager(string filepath)
    {
        filepath = _filepath;
    }
    public void SaveCatalog(Catalog catalog)
    {

    }
    public Catalog LoadCatalog()
    {
        throw new NotImplementedException();
    }
    public void SaveLoans(List<Loan> loans)
    {

    }
    public List<Loan> LoadLoans()
    {
        throw new NotImplementedException();
    }

}
public class NotificationSystem
{
    private List<Loan> _loans = new List<Loan>();

    public NotificationSystem(List<Loan> loans)
    {
        loans = _loans;
    }
    public List<Loan> CheckOverdueLoans()
    {
        throw new NotImplementedException();
    }
    public string SendNotification()
    {
        throw new NotImplementedException();
    }

}
