using Microsoft.Playwright;
using NUnit.Framework;
using GW.UI.Tests.Pages;
using GW.UI.Tests.TestData;

namespace GW.UI.Tests.Tests;

public class DivisionsTests
{
    private IPlaywright _playwright;      // Playwright
    private IBrowser _browser;            // Браузер
    private IPage _page;                  // Вкладка браузера

    private LoginPage _loginPage;         // Страница авторизации
    private DivisionsPage _divisionsPage; // Страница подразделений

    [SetUp]
    public async Task SetUp()
    {
        _playwright = await Playwright.CreateAsync();                          // Запускаем Playwright

        _browser = await _playwright.Chromium.LaunchAsync(new()
        {
            Headless = false
        });

        _page = await _browser.NewPageAsync();                                 // Открываем новую вкладку

        _loginPage = new LoginPage(_page);                                     // Создаем страницу авторизации
        _divisionsPage = new DivisionsPage(_page);                             // Создаем страницу подразделений
    }

    [TearDown]
    public async Task TearDown()
    {
        await _browser.CloseAsync();                                           // Закрываем браузер
        _playwright.Dispose();                                                 // Освобождаем ресурсы
    }

    [Test]
    public async Task Open_Division_Manager()
    {
        await _loginPage.Open();                                               // Открываем страницу входа

        await _loginPage.Login(Users.Email, Users.Password);                   // Авторизуемся

        await _page.WaitForURLAsync("**/Travels");                             // Ждем открытия главной страницы

        await _divisionsPage.Open();                                         // Открываем страницу подразделений

        await _divisionsPage.ExpandDivision("Головной офис г. Чебоксары");   // Раскрываем головной офис

        await _divisionsPage.OpenManager("Исрафилов");                       // Открываем карточку руководителя

        Assert.That(
            _divisionsPage.GetCurrentUrl(),
            Does.Contain("/Catalogs/EmployeeView/"));                        // Проверяем открытие карточки сотрудника
    }
}