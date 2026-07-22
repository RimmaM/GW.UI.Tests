using GW.UI.Tests.Pages;
using GW.UI.Tests.TestData;
using Microsoft.Playwright;
using NUnit.Framework;

namespace GW.UI.Tests.Tests;

[TestFixture]
public class CatalogsTests
{
    private IPlaywright _playwright;      // Экземпляр Playwright
    private IBrowser _browser;            // Экземпляр браузера
    private IPage _page;                  // Открытая вкладка браузера

    private LoginPage _loginPage;         // Страница авторизации
    private CatalogsPage _catalogsPage;   // Страница сотрудников

    [SetUp]
    public async Task SetUp()
    {
        _playwright = await Playwright.CreateAsync();                                              // Запускаем Playwright
        _browser = await _playwright.Chromium.LaunchAsync(new() { Headless = false });             // Запускаем браузер
        _page = await _browser.NewPageAsync();                                                      // Открываем новую вкладку

        _loginPage = new LoginPage(_page);                                                         // Создаем объект страницы авторизации
        _catalogsPage = new CatalogsPage(_page);                                                   // Создаем объект страницы сотрудников
    }

    [TearDown]
    public async Task TearDown()
    {
        await _browser.CloseAsync();                                                               // Закрываем браузер
        _playwright.Dispose();                                                                     // Освобождаем ресурсы Playwright
    }

    [Test]
    public async Task Search_Confirmed_Employee()
    {
        await _loginPage.Open();                                      // Открываем страницу входа
        await _loginPage.Login(Users.Email, Users.Password);          // Авторизуемся
        await _page.WaitForURLAsync("**/Travels");                    // Ждем открытия страницы после входа
        await _catalogsPage.Open();                                                   // Переходим на страницу сотрудников
        Console.WriteLine($"Переход в список Сотрудников");

        await _catalogsPage.SearchEmployee("Родионова");                              // Вводим фамилию в поиск
        await _catalogsPage.ClearSearch();                                            // Очищаем поле поиска крестиком
        Assert.That(await _catalogsPage.GetSearchValue(), Is.EqualTo(string.Empty));  // Проверяем, что поле поиска пустое
        Console.WriteLine($"Поиск + удаление введеного имени в поле поиска");

        await _catalogsPage.SelectConfirmedStatus();                                  // Выбираем статус "Подтвержден"
        await _catalogsPage.ResetFilters();                                           // Сбрасываем фильтры
        Assert.That(await _catalogsPage.GetSearchValue(), Is.EqualTo(string.Empty));  // Проверяем, что после сброса поиск снова пустой

        await _catalogsPage.SearchEmployee("Родионова");                              // Снова ищем сотрудника

        await _catalogsPage.OpenEmployee("Родионова");                                // Открываем карточку сотрудника

        Assert.That(
            _catalogsPage.GetCurrentUrl(),
            Does.Contain("/Catalogs/EmployeeView/"));                                 // Проверяем, что открылась карточка сотрудника}
    }

}