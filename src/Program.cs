public abstract class CommonFeatures
{
    private Guid _id = Guid.NewGuid();
    private string _titleOrName;
    private DateTime _createdDate;

    public Guid Id
    {
        get { return _id; }
        set { _id = value; }
    }
    public string TitleOrName
    {
        get { return _titleOrName; }
        set
        {
            if (string.IsNullOrEmpty(_titleOrName))
            {
                throw new Exception("The book title should not be null or empty.");
            }
            _titleOrName = value;
        }
    }
    public DateTime CreatedDate
    {
        get { return _createdDate; }
        set
        {
            if (string.IsNullOrEmpty(Convert.ToString(_createdDate)))
            {
                throw new Exception("The created date should not be null or empty.");
            }
            _createdDate = value;
        }
    }
    public CommonFeatures(string titleOrName, DateTime? createdDate = null)
    {
        if (string.IsNullOrEmpty(titleOrName))
        {
            throw new Exception("The book title should not be null or empty.");
        }
        _titleOrName = titleOrName;
        _createdDate = createdDate ?? DateTime.Now;
    }

}
public class Book : CommonFeatures
{
    public Book(string titleOrName, DateTime? createdDate = null) : base(titleOrName, createdDate)
    {

    }

}

public class User : CommonFeatures
{
    public User(string titleOrName, DateTime? createdDate = null) : base(titleOrName, createdDate)
    {

    }
}
public interface INotificationService
{
    public void SendNotificationOnSucess(string name);
    public void SendNotificationOnFailure(string name);
}
class EmailNotificationService : INotificationService
{
    public void SendNotificationOnSucess(string name)
    {
        Console.WriteLine($"Hello, a new user/book name '{name}' has been successfully added to the Library. If you have any queries or feedback, please contact our support team at support@library.com.");

    }
    public void SendNotificationOnFailure(string name)
    {
        Console.WriteLine($"We encountered an issue adding '{name}'. Please review the input data. For more help, visit our FAQ at library.com/faq.");

    }
}

class SMSNotificationService : INotificationService
{
    public void SendNotificationOnSucess(string name)
    {
        Console.WriteLine($"User/Book '{name}' added to Library. Thank you!");

    }
    public void SendNotificationOnFailure(string name)
    {
        Console.WriteLine($"Error adding User/Book '{name}'. Please email support@library.com.");

    }
}

public class Library
{
    public readonly List<User> users = new List<User>();
    public readonly List<Book> books = new List<Book>();
    public INotificationService INotificationService;

    public Library(INotificationService notificationService)
    {
        INotificationService = notificationService;
    }
    public void FindBook(string title)
    {
        var foundBook = books.FirstOrDefault(book => book.TitleOrName.Contains(title, StringComparison.OrdinalIgnoreCase));
        if (foundBook == null)
        {
            Console.WriteLine($"There is no book having this title '{title}'.");
            return;
        }
        Console.WriteLine($"Found: Book ID: {foundBook.Id}, Book Title: {foundBook.TitleOrName}, Created Date: {foundBook.CreatedDate}.");

    }
    public void FindUser(string name)
    {
        var foundUser = users.FirstOrDefault(user => user.TitleOrName.Contains(name, StringComparison.OrdinalIgnoreCase));
        if (foundUser == null)
        {
            Console.WriteLine($"There is no user having this name '{name}'.");
            return;
        }
        Console.WriteLine($"Found: User ID: {foundUser.Id}, User Name: {foundUser.TitleOrName}, Created Date: {foundUser.CreatedDate}.");
    }
    public void AddBook(Book newBook)
    {
        if (books.Any(book => book.TitleOrName == newBook.TitleOrName))
        {
            INotificationService.SendNotificationOnFailure(newBook.TitleOrName);
            return;
        }
        books.Add(newBook);
        INotificationService.SendNotificationOnSucess(newBook.TitleOrName);
    }
    public void AddUser(User newUser)
    {
        if (users.Any(user => user.TitleOrName == newUser.TitleOrName))
        {
            INotificationService.SendNotificationOnFailure(newUser.TitleOrName);
            return;
        }
        users.Add(newUser);
        INotificationService.SendNotificationOnSucess(newUser.TitleOrName);
    }
    public void DeleteBook(Guid id)
    {
        var foundBook = books.FirstOrDefault(book => book.Id == id);
        if (foundBook == null)
        {
            Console.WriteLine($"There is no book having this ID '{id}'.");
            return;
        }
        Console.WriteLine($"Book '{foundBook.TitleOrName}' removed from the library.");
        books.Remove(foundBook);
    }
    public void DeleteUser(Guid id)
    {
        var foundUser = users.FirstOrDefault(user => user.Id == id);
        if (foundUser == null)
        {
            Console.WriteLine($"There is no user having this ID '{id}'.");
            return;
        }
        Console.WriteLine($"User '{foundUser.TitleOrName}' removed from the library.");
        users.Remove(foundUser);
    }
    public void Display<T>(List<T> list) where T : CommonFeatures
    {
        if (list.Count == 0)
        {
            Console.WriteLine($"The List is empty.");
            return;
        }
        foreach (var item in list)
        {
            Console.WriteLine($"ID: {item.Id}, Name: {item.TitleOrName}, Created Date: {item.CreatedDate}.");
        }
    }

    public List<Book> GetBooks()
    {
        var paginationList = Utils.Pagination(books, 1, 3);
        return paginationList.OrderByDescending(book => book.CreatedDate).ToList();
    }
    public List<User> GetUsers(int pageNumber = 1, int limit = 3)
    {
        var paginationList = Utils.Pagination(users, 1, 3);
        return paginationList.OrderByDescending(user => user.CreatedDate).ToList();
    }
}

internal class Program
{
    private static void Main()
    {
        EmailNotificationService emailService = new EmailNotificationService();
        SMSNotificationService smsService = new SMSNotificationService();

        Library libraryManager = new Library(smsService);
        var user1 = new User("Alice", new DateTime(2023, 1, 1));
        var user2 = new User("Bob", new DateTime(2023, 2, 1));
        var user3 = new User("Ian");
        var user4 = new User("Julia");
        libraryManager.AddUser(user1);
        libraryManager.AddUser(user2);
        libraryManager.AddUser(user3);
        libraryManager.AddUser(user4);
        libraryManager.FindUser(user1.TitleOrName);
        libraryManager.DeleteUser(user1.Id);
        libraryManager.FindUser(user1.TitleOrName);
        libraryManager.Display(libraryManager.users);

        var book1 = new Book("The Great Gatsby", new DateTime(2023, 1, 1));
        var book2 = new Book("1984", new DateTime(2023, 2, 1));
        var book3 = new Book("The Iliad");
        var book4 = new Book("Anna Karenina");
        libraryManager.AddBook(book1);
        libraryManager.AddBook(book2);
        libraryManager.AddBook(book3);
        libraryManager.AddBook(book4);
        libraryManager.FindBook(book1.TitleOrName);
        libraryManager.DeleteBook(book1.Id);
        libraryManager.FindBook(book1.TitleOrName);
        libraryManager.Display(libraryManager.books);

        var getUsers = libraryManager.GetUsers();
        libraryManager.Display(getUsers);

        var getBooks = libraryManager.GetBooks();
        libraryManager.Display(getBooks);
    }
}