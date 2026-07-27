using Microsoft.Playwright;
using NUnit.Framework;
using GW.UI.Tests.Pages;
using GW.UI.Tests.TestData;

namespace GW.UI.Tests.Tests;

public class OrganizationsTests
{
    private IPlaywright _playwright;      // Playwright
    private IBrowser _browser;            // Браузер
    private IPage _page;                  // Вкладка браузера

    private LoginPage _loginPage;         // Страница авторизации
    private CatalogsPage _catalogsPage;
    private OrganizationsPage _organizationsPage; // Страница подразделений


    [SetUp]
    public async Task SetUp()
    {
        _playwright = await Playwright.CreateAsync();                          // Запускаем Playwright

        _browser = await _playwright.Chromium.LaunchAsync(new()
        {
            Headless = false
        });

        _page = await _browser.NewPageAsync();                                 // Открываем новую вкладку

        _loginPage = new LoginPage(_page);
        _catalogsPage = new CatalogsPage(_page);
        _organizationsPage = new OrganizationsPage(_page);
    }

    [TearDown]
    public async Task TearDown()
    {
        await _browser.CloseAsync();                                           // Закрываем браузер
        _playwright.Dispose();                                                 // Освобождаем ресурсы
    }

    [Test]
    [Category("Catalogs")]
    public async Task Open_First_Organization()
    {
       
        await _loginPage.Open();                                             // Авторизация
        await _loginPage.Login(Users.Email, Users.Password);
        await _page.WaitForURLAsync("**/Travels");
        await _organizationsPage.OpenCatalogs();              // Нажимаем "Справочники"
        await _organizationsPage.OpenOrganizations();             
                                           
        Console.WriteLine(await _page.Locator(".list-item").CountAsync());  // Проверяем, что появился список

        
        await _organizationsPage.OpenFirstOrganization();                   // Открываем первую организацию
        await _page.WaitForURLAsync("**/Catalogs/OrganizationEdit/**"); // Ждем открытия карточки

        Assert.That(_organizationsPage.GetCurrentUrl(),
            Does.Contain("/Catalogs/OrganizationEdit/"));
    }
}