using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

class Program
{
    private static Catalog catalog = new Catalog();
    public static List<Loan> loans = new List<Loan>();
    private static User user = new User(1, "John Doe", "john@example.com");
    private static ReportGenerator reportGenerator;

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
        DateTime currentTime = DateTime.Now;
        bool running = true;
        FileManager fileManager = new FileManager("data/");
        catalog = fileManager.LoadCatalog();
        loans = fileManager.LoadLoans(catalog, user);
        reportGenerator = new ReportGenerator(catalog, loans);

    while (running)
        {
            Console.Clear();
            DisplayMenu();
            Console.Write("Select an option (1-7): ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    try
                    {
                        Console.Write("Enter item type (Book/Magazine/DVD): ");
                        string type = Console.ReadLine();
                        Console.Write("Enter Item ID: ");
                        string id = Console.ReadLine();
                        Console.Write("Enter Title: ");
                        string title = Console.ReadLine();
                        LibraryItem item = null;
                        if (type.ToLower() == "book")
                        {
                            Console.Write("Enter ISBN: ");
                            string isbn = Console.ReadLine();
                            Console.Write("Enter Author: ");
                            string author = Console.ReadLine();
                            Console.Write("Enter Publication Year: ");
                            int year = int.Parse(Console.ReadLine());
                            item = new Book(id, title, true, isbn, author, year);
                        }
                        else if (type.ToLower() == "magazine")
                        {
                            Console.Write("Enter Issue Number: ");
                            string issue = Console.ReadLine();
                            Console.Write("Enter Publication Date (yyyy-MM-dd): ");
                            DateTime pubDate = DateTime.Parse(Console.ReadLine());
                            item = new Magazine(id, title, true, issue, pubDate);
                        }
                        else if (type.ToLower() == "dvd")
                        {
                            Console.Write("Enter Run Time (minutes): ");
                            int runTime = int.Parse(Console.ReadLine());
                            Console.Write("Enter Format: ");
                            string format = Console.ReadLine();
                            item = new DVD(id, title, true, runTime, format);
                        }
                        if (item != null)
                        {
                            catalog.AddItem(item);
                            fileManager.SaveCatalog(catalog);
                            Console.WriteLine("Item added successfully.");

                        }
                        else
                            Console.WriteLine("Invalid item type.");
                    }
                    catch (FormatException)
                    {
                        Console.WriteLine("Invalid input format. Please try again.");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                    }
                    break;

                case "2":
                    Console.Write("Enter Item ID to borrow: ");
                    string borrowId = Console.ReadLine();
                    var borrowItem = catalog.SearchItems(borrowId).FirstOrDefault(i => i.ItemId == borrowId);
                    if (borrowItem != null && borrowItem.CheckAvailability())
                    {
                        user.BorrowItem(borrowItem, currentTime);
                        fileManager.SaveLoans(Program.loans);
                        Console.WriteLine("Item borrowed successfully.");
                    }
                    else
                        Console.WriteLine("Item not found or unavailable.");
                    break;
                case "3":
                    Console.Write("Enter Item ID to return: ");
                    string returnId = Console.ReadLine();
                    var loan = Program.loans.FirstOrDefault(l => l.Item.ItemId == returnId && l.ReturnDate == default);
                    if (loan != null)
                    {
                        loan.ReturnItem();
                        fileManager.SaveLoans(Program.loans);
                        decimal lateFee = loan.CalculateTotalLateFee();
                        if (lateFee > 0)
                            Console.WriteLine($"Item returned. Late fee: ${lateFee}");
                        else
                            Console.WriteLine("Item returned successfully.");
                    }
                    else
                        Console.WriteLine("Loan not found or already returned.");
                    break;

                case "4":
                    var availableItems = catalog.GetAvailableItems();
                    Console.WriteLine("Available Items:");
                    Console.WriteLine();
                    foreach (var availItem in availableItems)
                        Console.WriteLine($"{availItem.Title} (ID: {availItem.ItemId})");
                    Console.WriteLine();
                    break;

                case "5":
                    Console.Write("Enter Item ID for late fee calculation: ");
                    string feeId = Console.ReadLine();
                    var feeLoan = Program.loans.FirstOrDefault(l => l.Item.ItemId == feeId && l.ReturnDate == default);
                    if (feeLoan != null)
                        Console.WriteLine($"Late Fee: ${feeLoan.CalculateTotalLateFee()}");
                    else
                        Console.WriteLine("Loan not found or already returned.");
                    break;
                case "6":
                    Console.Clear();
                    Console.WriteLine(reportGenerator.GenerateInventory());
                    Console.WriteLine("\n" + reportGenerator.GenerateBorrowingTrends());
                    break;

                case "7":
                    running = false;
                    Console.Clear();
                    Console.WriteLine("Thank you for using the Library System");
                    break;

                default:
                    Console.WriteLine("Invalid option. Please try again.");
                    break;
            }
            Console.WriteLine();
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
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
    public string ItemId { get; protected set; }
    public string Title { get; protected set; }
    public bool IsAvailable { get; set; }

    public abstract bool CheckAvailability();
    public abstract decimal CalculateLateFee(DateTime dueDate, DateTime returnDate);
}
public class Magazine : LibraryItem
{
    private string _issueNumber;
    private DateTime _publicationDate;

    public Magazine(string itemId, string title, bool isAvailable, string issueNumber, DateTime publicationDate) : base(itemId, title, isAvailable)
    {
        _issueNumber = issueNumber;
        _publicationDate = publicationDate;
    }
    public string IssueNumber => _issueNumber;
    public DateTime PublicationDate => _publicationDate;
    public override bool CheckAvailability()
    {
        return IsAvailable;
    }
    public override decimal CalculateLateFee(DateTime dueDate, DateTime returnDate)
    {
        if (returnDate <= dueDate)
            return 0m;
        int daysLate = (returnDate - dueDate).Days;
        return daysLate * 1.50m;
    }
}
public class Book : LibraryItem
{
    private string _isbn;
    private string _author;
    private int _publicationYear;

    public Book(string itemId, string title, bool isAvailable, string isbn, string author, int publicationYear) : base(itemId, title, isAvailable)
    {
        _isbn = isbn;
        _author = author;
        _publicationYear = publicationYear;
    }
    public string Isbn => _isbn;
    public string Author => _author;
    public int PublicationYear => _publicationYear;
    public override bool CheckAvailability()
    {
        return IsAvailable;
    }
    public override decimal CalculateLateFee(DateTime dueDate, DateTime returnDate)
    {
        if (returnDate <= dueDate)
            return 0m;
        int daysLate = (returnDate - dueDate).Days;
        return daysLate * 3.00m;
    }

}
public class DVD : LibraryItem
{
    private int _runTime;
    private string _format;

    public DVD(string itemId, string title, bool isAvailable, int runTime, string format) : base(itemId, title, isAvailable)
    {
        _runTime = runTime;
        _format = format;
    }
    public int RunTime => _runTime;
    public string Format => _format;
    public override bool CheckAvailability()
    {
        return IsAvailable;
    }
    public override decimal CalculateLateFee(DateTime dueDate, DateTime returnDate)
    {
         if (returnDate <= dueDate)
            return 0m;
        int daysLate = (returnDate - dueDate).Days;
        return daysLate * 2.50m;
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
        _userId = userId;
        _name = name;
        _email = email;
    }

    public int UserId => _userId;
    public string Name => _name;
    public string Email => _email;

    public void BorrowItem(LibraryItem item, DateTime borrowDate)
    {
        if (item.CheckAvailability())
        {
            item.IsAvailable = false;
            DateTime dueDate = borrowDate.AddDays(14);
            Loan loan = new Loan(_loans.Count + 1, item, this, borrowDate, default, dueDate);
            _loans.Add(loan);
            Program.loans.Add(loan);
        }
    }

    public void GetLoanHistory()
    {
        Console.WriteLine($"Loan History for {_name}");
        if (_loans.Count == 0)
        {
            Console.WriteLine("No loans found.");
            return;
        }
        foreach (var loan in _loans)
        {
            Console.WriteLine($"Item: {loan.Item.Title}, Borrowed: {loan.BorrowDate:yyyy-MM-dd}, Due: {loan.DueDate:yyyy-MM-dd}, Returned: {(loan.ReturnDate == default ? "Not returned" : loan.ReturnDate.ToString("yyyy-MM-dd"))}");
        }
    }
}
public class Loan
{
    private int _loanId;
    private LibraryItem _item;
    private User _user;
    private DateTime _borrowDate;
    private DateTime _returnDate;
    private DateTime _dueDate;

    public Loan(int loanId, LibraryItem item, User user, DateTime borrowDate, DateTime returnDate, DateTime dueDate)
    {
        _loanId = loanId;
        _item = item;
        _user = user;
        _borrowDate = borrowDate;
        _returnDate = returnDate;
        _dueDate = dueDate;
    }

    public int LoanId => _loanId;
    public LibraryItem Item => _item;
    public User User => _user;
    public DateTime BorrowDate => _borrowDate;
    public DateTime ReturnDate => _returnDate;
    public DateTime DueDate => _dueDate;

    public bool IsOverdue()
    {
        if (_returnDate != default)
            return _returnDate > _dueDate;
        return DateTime.Now > _dueDate;
    }

    public decimal CalculateTotalLateFee()
    {
        if (_returnDate == default)
            return _item.CalculateLateFee(_dueDate, DateTime.Now);
        return _item.CalculateLateFee(_dueDate, _returnDate);
    }

    public void ReturnItem()
    {
        _returnDate = DateTime.Now;
        _item.IsAvailable = true;
    }
}
public class Catalog
{
    private List<LibraryItem> _items = new List<LibraryItem>();
    public Catalog()
    { }
    public void AddItem(LibraryItem item)
    {
        _items.Add(item);
    }
    public void RemoveItem(LibraryItem item)
    {
        _items.Remove(item);
    }
    public List<LibraryItem> SearchItems(string query)
    {
        query = query.ToLower();
        return _items.Where(item => item.Title.ToLower().Contains(query) || item.ItemId.ToLower() == query).ToList();
    }
    public List<LibraryItem> GetAvailableItems()
    {
        return _items.Where(item => item.CheckAvailability()).ToList();
    }
    public List<LibraryItem> GetAllItems() => new List<LibraryItem>(_items);
}
public class ReportGenerator
{
    private Catalog _catalog;
    private List<Loan> _loans = new List<Loan>();

    public ReportGenerator(Catalog catalog, List<Loan> loans)
    {
        _catalog = catalog;
        _loans = loans;
    }
    public string GenerateInventory()
    {
        var items = _catalog.GetAvailableItems();
        string report = "Inventory Report:\n";
        report += $"Total items: {_catalog.GetAllItems().Count}, Available: {items.Count}\n";
        foreach (var item in _catalog.GetAllItems())
        {
            report += $"{item.Title} (ID: {item.ItemId}, {(item.CheckAvailability() ? "Available" : "Borrowed")})\n";
        }
        return report;
    }
    public string GenerateBorrowingTrends()
    {
        string report = "Borrowing Trends Report:\n";
        report += $"Total Loans: {_loans.Count}\n";
        var overdue = _loans.Count(l => l.IsOverdue());
        report += $"Overdue Loans: {overdue}\n";
        var byType = _loans.GroupBy(l => l.Item.GetType().Name)
                        .Select(g => $"{g.Key}: {g.Count()} loans");
        report += string.Join("\n", byType);
        return report;
    }   
}
public class FileManager
{
    private string _filepath;

    public FileManager(string filepath)
    {
        _filepath = filepath;
        Directory.CreateDirectory(_filepath);
    }
    public void SaveCatalog(Catalog catalog)
    {
        using (StreamWriter writer = new StreamWriter(_filepath + "catalog.txt"))
        {
            foreach (var item in catalog.GetAllItems())
            {
                string type = item.GetType().Name;
                string line = $"{type}|{item.ItemId}|{item.Title}|{item.IsAvailable}";
                if (item is Book book)
                    line += $"|{book.Isbn}|{book.Author}|{book.PublicationYear}";
                else if (item is Magazine mag)
                    line += $"|{mag.IssueNumber}|{mag.PublicationDate:yyyy-MM-dd}";
                else if (item is DVD dvd)
                    line += $"|{dvd.RunTime}|{dvd.Format}";
                writer.WriteLine(line);
            }
        }
    }
    public Catalog LoadCatalog()
    {
        Catalog catalog = new Catalog();
        if (!File.Exists(_filepath + "catalog.txt"))
            return catalog;
        using (StreamReader reader = new StreamReader(_filepath + "catalog.txt"))
        {
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                var parts = line.Split("|");
                string type = parts[0], id = parts[1], title = parts[2];
                bool isAvailable = bool.Parse(parts[3]);
                if (type == "Book")
                    catalog.AddItem(new Book(id, title, isAvailable, parts[4], parts[5], int.Parse(parts[6])));
                else if (type == "Magazine")
                    catalog.AddItem(new Magazine(id, title, isAvailable, parts[4], DateTime.Parse(parts[5])));
                else if (type == "DVD")
                    catalog.AddItem(new DVD(id, title, isAvailable, int.Parse(parts[4]), parts[5]));
            }
        }
        return catalog;
    }
    public void SaveLoans(List<Loan> loans)
    {
        using (StreamWriter writer = new StreamWriter(_filepath + "loans.txt"))
        {
            foreach (var loan in loans)
            {
                string line = $"{loan.LoanId}|{loan.Item.ItemId}|{loan.User.UserId}|{loan.BorrowDate:yyyy-MM-dd}|{(loan.ReturnDate == default ? "" : loan.ReturnDate.ToString("yyyy-MM-dd"))}|{loan.DueDate:yyyy-MM-dd}";
                writer.WriteLine(line);
            }
        }
    }
    public List<Loan> LoadLoans(Catalog catalog, User user)
    {
        List<Loan> loans = new List<Loan>();
        if (!File.Exists(_filepath + "loans.txt"))
            return loans;
        using (StreamReader reader = new StreamReader(_filepath + "loans.txt"))
        {
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                var parts = line.Split('|');
                if (parts.Length != 6)
                    continue;

                try
                {
                    int loanId = int.Parse(parts[0]);
                    string itemId = parts[1];
                    int userId = int.Parse(parts[2]);
                    DateTime borrowDate = DateTime.Parse(parts[3]);
                    DateTime returnDate = parts[4] == "" ? default : DateTime.Parse(parts[4]);
                    DateTime dueDate = DateTime.Parse(parts[5]);
                    var item = catalog.SearchItems(itemId).FirstOrDefault(i => i.ItemId == itemId);
                    if (item != null && user.UserId == userId)
                        loans.Add(new Loan(loanId, item, user, borrowDate, returnDate, dueDate));
                }
                catch (FormatException)
                {
                    Console.WriteLine($"Skipping invalid loan entry: {line}");
                    continue;
                } 
            }
        }
        return loans;
    }
}
public class NotificationSystem
{
    private List<Loan> _loans = new List<Loan>();

    public NotificationSystem(List<Loan> loans)
    {
        _loans = loans;
    }

    public List<Loan> CheckOverdueLoans()
    {
        return _loans.Where(l => l.IsOverdue()).ToList();
    }

    public string SendNotification()
    {
        var overdue = CheckOverdueLoans();
        string message = "Overdue Notifications:\n";
        foreach (var loan in overdue)
        {
            message += $"User {loan.User.Name}, Item {loan.Item.Title} is overdue. Due: {loan.DueDate:yyyy-MM-dd}\n";
        }
        return message;
    }
}