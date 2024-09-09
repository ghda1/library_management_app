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

public class Library
{
    private readonly List<User> users = new List<User>();
    private readonly List<Book> books = new List<Book>();
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
            Console.WriteLine($"This book is already exist.");
            return;
        }
        books.Add(newBook);
        Console.WriteLine($"Book '{newBook.TitleOrName}' added to the library.");
    }
    public void AddUser(User newUser)
    {
        if (users.Any(user => user.TitleOrName == newUser.TitleOrName))
        {
            Console.WriteLine($"This user is already exist.");
            return;
        }
        users.Add(newUser);
        Console.WriteLine($"User '{newUser.TitleOrName}' added to the library.");
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
    public void DisplayAllBooks()
    {
        if (books.Count == 0)
        {
            Console.WriteLine($"The List of books is empty.");
            return;
        }
        foreach (var book in books)
        {
            Console.WriteLine($"Book ID: {book.Id}, Book Title: {book.TitleOrName}, Created Date: {book.CreatedDate}.");
        }
    }
    public void DisplayAllUsers()
    {
        if (users.Count == 0)
        {
            Console.WriteLine($"The List of users is empty.");
            return;
        }
        foreach (var user in users)
        {
            Console.WriteLine($"User ID: {user.Id}, User Name: {user.TitleOrName}, Created Date: {user.CreatedDate}.");
        }
    }
    public List<Book> GetBooks(int pageNumber = 1, int limit = 3)
    {
        return books.OrderByDescending(book => book.CreatedDate).Skip((pageNumber - 1) * limit).Take(limit).ToList();
    }
    public List<User> GetUsers(int pageNumber = 1, int limit = 3)
    {
        return users.OrderByDescending(user => user.CreatedDate).Skip((pageNumber - 1) * limit).Take(limit).ToList();
    }
}

internal class Program
{
    private static void Main()
    {
        Library libraryManager = new Library();
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
        libraryManager.DisplayAllUsers();
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
        libraryManager.DisplayAllBooks();

        var getUsers = libraryManager.GetUsers();
        foreach (var user in getUsers)
        {
            Console.WriteLine($"{user.TitleOrName}");
        }

        var getBooks = libraryManager.GetBooks();
        foreach (var book in getBooks)
        {
            Console.WriteLine($"{book.TitleOrName}");
        }
    }
}