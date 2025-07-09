using System;
using System.Collections.Generic;

namespace LibraryManagement
{
    public class User
    {
        public int UserId { get; private set; }
        public string Name { get; private set; }
        public string Email { get; private set; }
        public List<Loan> Loans { get; private set; }

        public User(int userId, string name, string email)
        {
            UserId = userId;
            Name = name;
            Email = email;
            Loans = new List<Loan>();
        }

        public void BorrowItem(LibraryItem item, DateTime dueDate)
        {
            if (item.IsAvailable)
            {
                var loan = new Loan(this, item, dueDate);
                Loans.Add(loan);
                item.IsAvailable = false;
                Console.WriteLine($"Borrowed: {item.Title} by {Name}. Due: {dueDate}");
            }
            else
            {
                Console.WriteLine($"Item {item.Title} is not available.");
            }
        }

        public void ReturnItem(LibraryItem item)
        {
            var loan = Loans.Find(l => l.Item == item);
            if (loan != null)
            {
                Loans.Remove(loan);
                item.IsAvailable = true;
                Console.WriteLine($"Returned: {item.Title} by {Name}.");
            }
            else
            {
                Console.WriteLine($"No loan record for {item.Title} by {Name}.");
            }
        }
    }

    public abstract class LibraryItem
    {
        public string ItemId { get; set; }
        public string Title { get; set; }
        public bool IsAvailable { get; set; }

        public LibraryItem(string itemId, string title)
        {
            ItemId = itemId;
            Title = title;
            IsAvailable = true;
        }

        public abstract bool CheckAvailability();
        public abstract decimal CalculateLateFee(DateTime dueDate, DateTime returnDate);
    }

    public class Magazine : LibraryItem
    {
        public string IssueNumber { get; set; }
        public DateTime PublicationDate { get; set; }

        public Magazine(string itemId, string title, string issueNumber, DateTime publicationDate)
            : base(itemId, title)
        {
            IssueNumber = issueNumber;
            PublicationDate = publicationDate;
        }

        public override bool CheckAvailability()
        {
            return IsAvailable;
        }

        public override decimal CalculateLateFee(DateTime dueDate, DateTime returnDate)
        {
            if (returnDate > dueDate)
            {
                int daysLate = (returnDate - dueDate).Days;
                return daysLate * 0.25m; // $0.25 per day late
            }
            return 0m;
        }
    }

    public class Book : LibraryItem
    {
        public string Isbn { get; set; }
        public string Author { get; set; }
        public int PublicationYear { get; set; }

        public Book(string itemId, string title, string isbn, string author, int publicationYear)
            : base(itemId, title)
        {
            Isbn = isbn;
            Author = author;
            PublicationYear = publicationYear;
        }

        public override bool CheckAvailability()
        {
            return IsAvailable;
        }

        public override decimal CalculateLateFee(DateTime dueDate, DateTime returnDate)
        {
            if (returnDate > dueDate)
            {
                int daysLate = (returnDate - dueDate).Days;
                return daysLate * 0.50m; // $0.50 per day late
            }
            return 0m;
        }
    }

    public class DVD : LibraryItem
    {
        public int RunTime { get; set; }
        public string Format { get; set; }

        public DVD(string itemId, string title, int runTime, string format)
            : base(itemId, title)
        {
            RunTime = runTime;
            Format = format;
        }

        public override bool CheckAvailability()
        {
            return IsAvailable;
        }

        public override decimal CalculateLateFee(DateTime dueDate, DateTime returnDate)
        {
            if (returnDate > dueDate)
            {
                int daysLate = (returnDate - dueDate).Days;
                return daysLate * 1.00m; // $1.00 per day late
            }
            return 0m;
        }
    }

    public class Loan
    {
        public int LoanId { get; set; }
        public LibraryItem Item { get; private set; }
        public User User { get; private set; }
        public DateTime BorrowDate { get; private set; }
        public DateTime DueDate { get; private set; }

        public Loan(User user, LibraryItem item, DateTime dueDate)
        {
            LoanId = new Random().Next(1000, 9999);
            User = user;
            Item = item;
            BorrowDate = DateTime.Now;
            DueDate = dueDate;
        }

        public decimal GetOverdueAmount(DateTime returnDate)
        {
            return Item.CalculateLateFee(DueDate, returnDate);
        }
    }

    public class Catalog
    {
        public List<LibraryItem> Items { get; set; }

        public Catalog()
        {
            Items = new List<LibraryItem>();
        }

        public void AddItem(LibraryItem item)
        {
            Items.Add(item);
        }

        public List<LibraryItem> GetAvailableItems()
        {
            return Items.FindAll(item => item.CheckAvailability());
        }
    }

    public class ReportGenerator
    {
        private Catalog Catalog { get; set; }
        private List<Loan> Loans { get; set; }

        public ReportGenerator(Catalog catalog, List<Loan> loans)
        {
            Catalog = catalog;
            Loans = loans;
        }

        public string GenerateInventory()
        {
            return $"Total Items: {Catalog.GetAvailableItems().Count}, Available: {Catalog.GetAvailableItems().Count}";
        }
    }

    class Program
    {
        private static Catalog catalog = new Catalog();
        private static List<Loan> loans = new List<Loan>();
        private static User user = new User(1, "John Doe", "john@example.com");
        private static ReportGenerator reportGenerator = new ReportGenerator(catalog, loans);

        static void Main(string[] args)
        {
            DateTime currentTime = new DateTime(2025, 7, 9, 8, 48, 0); // 08:48 AM MDT, July 9, 2025
            bool running = true;

            while (running)
            {
                Console.WriteLine("\nLibrary Management System Menu:");
                Console.WriteLine("1. Add Item");
                Console.WriteLine("2. Borrow Item");
                Console.WriteLine("3. Return Item");
                Console.WriteLine("4. Check Availability");
                Console.WriteLine("5. Calculate Late Fee");
                Console.WriteLine("6. Generate Inventory Report");
                Console.WriteLine("7. Exit");
                Console.Write("Select an option (1-7): ");

                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        AddItem();
                        break;
                    case "2":
                        BorrowItem(currentTime);
                        break;
                    case "3":
                        ReturnItem();
                        break;
                    case "4":
                        CheckAvailability();
                        break;
                    case "5":
                        CalculateLateFee(currentTime);
                        break;
                    case "6":
                        GenerateReport();
                        break;
                    case "7":
                        running = false;
                        Console.WriteLine("Exiting...");
                        break;
                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        break;
                }
            }
        }

        static void AddItem()
        {
            Console.WriteLine("Enter item type (Magazine/Book/DVD): ");
            string type = Console.ReadLine();
            Console.Write("Enter Item ID: ");
            string itemId = Console.ReadLine();
            Console.Write("Enter Title: ");
            string title = Console.ReadLine();

            switch (type.ToLower())
            {
                case "magazine":
                    Console.Write("Enter Issue Number: ");
                    string issue = Console.ReadLine();
                    Console.Write("Enter Publication Date (yyyy-mm-dd): ");
                    DateTime pubDate = DateTime.Parse(Console.ReadLine());
                    catalog.AddItem(new Magazine(itemId, title, issue, pubDate));
                    break;
                case "book":
                    Console.Write("Enter ISBN: ");
                    string isbn = Console.ReadLine();
                    Console.Write("Enter Author: ");
                    string author = Console.ReadLine();
                    Console.Write("Enter Publication Year: ");
                    int year = int.Parse(Console.ReadLine());
                    catalog.AddItem(new Book(itemId, title, isbn, author, year));
                    break;
                case "dvd":
                    Console.Write("Enter Run Time (minutes): ");
                    int runTime = int.Parse(Console.ReadLine());
                    Console.Write("Enter Format: ");
                    string format = Console.ReadLine();
                    catalog.AddItem(new DVD(itemId, title, runTime, format));
                    break;
                default:
                    Console.WriteLine("Invalid item type.");
                    break;
            }
            Console.WriteLine("Item added successfully.");
        }

        static void BorrowItem(DateTime currentTime)
        {
            Console.Write("Enter Item ID to borrow: ");
            string itemId = Console.ReadLine();
            var item = catalog.GetAvailableItems().Find(i => i.ItemId == itemId);
            if (item != null)
            {
                Console.Write("Enter due date (yyyy-mm-dd): ");
                DateTime dueDate = DateTime.Parse(Console.ReadLine());
                user.BorrowItem(item, dueDate);
                loans.Add(user.Loans[user.Loans.Count - 1]);
            }
            else
            {
                Console.WriteLine("Item not found or unavailable.");
            }
        }

        static void ReturnItem()
        {
            Console.Write("Enter Item ID to return: ");
            string itemId = Console.ReadLine();
            var item = catalog.GetAvailableItems().Find(i => i.ItemId == itemId);
            if (item == null) item = user.Loans.Find(l => l.Item.ItemId == itemId)?.Item;
            if (item != null) user.ReturnItem(item);
            else Console.WriteLine("Item not found in loans.");
        }

        static void CheckAvailability()
        {
            Console.Write("Enter Item ID to check: ");
            string itemId = Console.ReadLine();
            var item = catalog.Items.Find(i => i.ItemId == itemId);
            if (item != null)
                Console.WriteLine($"Item {item.Title} is {(item.CheckAvailability() ? "available" : "not available")}.");
            else
                Console.WriteLine("Item not found.");
        }

        static void CalculateLateFee(DateTime currentTime)
        {
            Console.Write("Enter Item ID to check late fee: ");
            string itemId = Console.ReadLine();
            var loan = user.Loans.Find(l => l.Item.ItemId == itemId);
            if (loan != null)
            {
                decimal fee = loan.GetOverdueAmount(currentTime);
                Console.WriteLine($"Late Fee for {loan.Item.Title}: ${fee}");
            }
            else
            {
                Console.WriteLine("No active loan for this item.");
            }
        }

        static void GenerateReport()
        {
            Console.WriteLine(reportGenerator.GenerateInventory());
        }
    }
}