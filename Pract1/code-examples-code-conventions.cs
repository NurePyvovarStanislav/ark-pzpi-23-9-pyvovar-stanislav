// 3.2 Рекомендація 1. Підтримуйте чітку структуру проєкту
// Поганий приклад
// Увесь код звалено в один проєкт і один клас
using App.Core.Services.App.Infrastructure.Repositories;

public class App
{
    // UI
    public void RenderMainPage() { /* ... */ }

    // Бізнес-логіка
    public void CreateOrder(User user, List<Product> products)
    {
        // валідація, розрахунок, збереження, e-mail – все тут
    }

    // Доступ до БД
    public void SaveToDatabase(object entity)
    {
        // прямий SQL тут
    }
}
// -------------------------------------
// Гарний приклад коду
// Шари розділені, відповідальність розмежована
// Рівень доменної логіки
namespace App.Core.Services;

public class OrderService
{
    private readonly IOrderRepository _orders;
    private readonly IEmailSender _emailSender;

    public OrderService(IOrderRepository orders, IEmailSender emailSender)
    {
        _orders = orders;
        _emailSender = emailSender;
    }

    public void CreateOrder(User user, IReadOnlyCollection<Product> products)
    {
        var order = Order.Create(user, products);
        _orders.Save(order);
        _emailSender.SendOrderCreated(user, order);
    }
}

// Рівень доступу до даних
namespace App.Infrastructure.Repositories;

public class OrderRepository : IOrderRepository
{
    public void Save(Order order)
    {
        // збереження через ORM
    }
}

// Рівень веб-інтерфейсу
namespace App.Web.Controllers;

public class OrdersController : Controller
{
    private readonly OrderService _orderService;

    public OrdersController(OrderService orderService)
    {
        _orderService = orderService;
    }

    public IActionResult Create(CreateOrderDto dto)
    {
        // мапінг DTO -> домен, виклик сервісу
        return RedirectToAction("Index");
    }
}
// -------------------------------------
// 3.3 Рекомендація 2. Дотримуйтеся принципу єдиної відповідальності
// Поганий приклад
public class UserManager
{
    public void RegisterUser(User user)
    {
        // бізнес-логіка
        // валідація
        // збереження в БД
        // надсилання e-mail
        // логування
    }
}
// -------------------------------------
// Гарний приклад коду
public class UserService
{
    private readonly IUserRepository _users;
    private readonly IEmailSender _emailSender;

    public UserService(IUserRepository users, IEmailSender emailSender)
    {
        _users = users;
        _emailSender = emailSender;
    }

    public void RegisterUser(User user)
    {
        // бізнес-логіка та валідація
        _users.Save(user);
        _emailSender.SendWelcome(user);
    }
}

public interface IUserRepository
{
    void Save(User user);
}

public interface IEmailSender
{
    void SendWelcome(User user);
}
// -------------------------------------
// 3.4 Рекомендація 3. Використовуйте коментарі для логічного поділу коду
// Поганий приклад
public void Process()
    {
        // збільшуємо лічильник
        count++;

        // проходимо по елементах
        foreach (var item in items)
        {
            // виводимо в консоль
            Console.WriteLine(item);
        }
    }
    // -------------------------------------
    // Гарний приклад коду
    public void ProcessActiveUsers()
    {
        // 1. Отримуємо активних користувачів за останні 30 днів
        var activeUsers = _userRepository.GetActiveUsers(days: 30);

        // 2. Відправляємо сповіщення
        foreach (var user in activeUsers)
        {
            _notificationService.Notify(user);
        }

        // 3. Формуємо звіт за результатами розсилки
        _reportService.GenerateActiveUsersReport(activeUsers);
    }
    // -------------------------------------
    // 3.5 Рекомендація 4. Використовуйте єдині правила відступів і форматування
    // Поганий приклад
    public void Print()
    {
        Console.WriteLine("Start");
        if (true)
        {
            Console.WriteLine("Done");
        }
    }
    // -------------------------------------
    // Гарний приклад коду
    public void Print()
    {
        Console.WriteLine("Start");

        if (true)
        {
            Console.WriteLine("Done");
        }
    }
// -------------------------------------
// 3.6 Рекомендація 5. Використовуйте узгоджений стиль дужок
// Поганий приклад
if (isValid)
{
    DoWork();
}
else
{
    DoFallback();
}
// -------------------------------------
// Гарний приклад коду
if (isValid)
{
    DoWork();
}
else
{
    DoFallback();
}
// -------------------------------------
// 3.7 Рекомендація 6. Використовуйте зрозумілі правила іменування
// Поганий приклад
int a = 10;
string str1 = "name";

public void Do(int x)
{
    Console.WriteLine(a + x);
}
// -------------------------------------
// Гарний приклад коду
private const int DefaultOffset = 10;

public void PrintSum(int value)
{
    Console.WriteLine(DefaultOffset + value);
}

string userName = "Alex";
// -------------------------------------
// 3.8 Рекомендація 7. Уникайте «магічних» чисел і використовуйте константи
// Поганий приклад
if (age > 18)
{
    discount = 0.1m;
}
// -------------------------------------
// Гарний приклад коду
private const int AdultAge = 18;
private const decimal AdultDiscount = 0.10m;

if (age > AdultAge)
{
    discount = AdultDiscount;
}
// -------------------------------------
// 3.9 Рекомендація 8. Використовуйте коментарі лише там, де це дійсно необхідно
// Поганий приклад
// додаємо до списку
users.Add(user);

// виводимо кількість
Console.WriteLine(users.Count);
// -------------------------------------
// Гарний приклад коду
// Обчислення середнього значення зважених показників
public double GetWeightedAverage(IEnumerable<Metric> metrics)
{
    double totalWeight = metrics.Sum(m => m.Weight);

    return metrics.Sum(m => m.Value * m.Weight) / totalWeight;
}
// -------------------------------------
// 3.10 Рекомендація 9. Документуйте код за допомогою XML-коментарів
// Поганий приклад
public decimal CalculateDiscountedPrice(decimal price, int discountPercent)
{
    return price - price * discountPercent / 100m;
}
// -------------------------------------
// Гарний приклад коду
/// <summary>
/// Обчислює ціну з урахуванням знижки.
/// </summary>
/// <param name="price">Початкова ціна.</param>
/// <param name="discountPercent">Розмір знижки у відсотках.</param>
/// <returns>Ціна після застосування знижки.</returns>
public decimal CalculateDiscountedPrice(decimal price, int discountPercent)
{
    return price - price * discountPercent / 100m;
}
// --------------------------------------
// 3.11 Рекомендація 10. Використовуйте інструменти перевірки стилю коду
// Поганий приклад
public class user
{
    public string name;
    public void print()
    {
        Console.WriteLine(name);
    }
}
// -------------------------------------
// Гарний приклад коду
public class User
{
    public string Name { get; set; }

    public void Print()
    {
        Console.WriteLine(Name);
    }
}
// 3.12 Рекомендація 11. Використовуйте статичний аналіз коду
// Поганий приклад
public int Divide(int a, int b)
{
    return a / b; // можливий поділ на нуль
}
// Гарний приклад коду
public int Divide(int a, int b)
{
    if (b == 0)
    {
        throw new DivideByZeroException("Denominator must not be zero.");
    }

    return a / b;
}
// -------------------------------------
// 3.13 Рекомендація 12. Застосовуйте тестування під час розробки
// Поганий приклад (метод без тестів)
public int GetUserAge(User user)
{
    if (user == null)
    {
        return -1;
    }

    return user.Age;
}
// -------------------------------------
// Гарний приклад коду (з тестом)
public int GetUserAge(User user)
{
    if (user == null)
    {
        return -1;
    }

    return user.Age;
}

// xUnit-тест
[Fact]
public void GetUserAge_ReturnsAge_ForValidUser()
{
    var user = new User { Age = 25 };
    var result = GetUserAge(user);

    Assert.Equal(25, result);
}
// -------------------------------------
// 3.14 Рекомендація 13. Використовуйте інструменти автоматизації
// Поганий приклад

// Розробник ніколи не запускає тести локально і не має CI
// Тести існують, але ними ніхто не користується.
// -------------------------------------
// Гарний приклад коду (умовний фрагмент YAML для GitHub Actions)

/*
name: .NET CI

on:
  push:
    branches: [ "main" ]
  pull_request:
    branches: [ "main" ]

jobs:
  build:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v4
      - name: Setup .NET
        uses: actions/setup-dotnet@v4
        with:
          dotnet-version: 8.0.x
      - name: Restore
        run: dotnet restore
      - name: Build
        run: dotnet build --no-restore
      - name: Test
        run: dotnet test --no-build --verbosity normal
*/

