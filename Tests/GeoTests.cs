using GW.UI.Tests.Helpers;
using GW.UI.Tests.Pages;
using GW.UI.Tests.TestData;
using Microsoft.Playwright;
using NUnit.Framework;

namespace GW.UI.Tests.Tests;

public class GeoTests
{
    private IPlaywright _playwright;      // Playwright
    private IBrowser _browser;            // Браузер
    private IPage _page;                  // Вкладка браузера

    private LoginPage _loginPage;         // Страница авторизации
    private GeoPage _geoPage;               // Страница гео

    [SetUp]
    public async Task SetUp()
    {
        _playwright = await Playwright.CreateAsync();              // Запускаем Playwright

        _browser = await _playwright.Chromium.LaunchAsync(new()
        {
            Headless = false
        });

        _page = await _browser.NewPageAsync();                     // Открываем новую вкладку

        _loginPage = new LoginPage(_page);                         // Создаем страницу авторизации
        _geoPage = new GeoPage(_page);                             // Создаем страницу гео
    }

    [TearDown]
    public async Task TearDown()
    {
        await _browser.CloseAsync();                                // Закрываем браузер
        _playwright.Dispose();                                      // Освобождаем ресурсы
    }


   // [Test] //72 test
    [Category("Catalogs")]
    public async Task Create_Country()
    {

        await _loginPage.Open();                                    // Авторизация
        await _loginPage.Login(Users.Email, Users.Password);
        await _page.WaitForURLAsync("**/Travels");
        await _geoPage.OpenCatalogs();                              // Нажимаем "Справочники"
        Console.WriteLine($"Переход в список Сотрудников");

        await _geoPage.OpenGeo();                                     // Переход на страницу geo
        Console.WriteLine($"Переход в список Гео");

        string countryName = "Test-UI";
        string newCountryName = "";


        if (GeoTestControl.CanCreateCountry())
        {
            await _geoPage.ClickAddCountry();
            await _geoPage.FillCountryName(countryName);
            await _geoPage.SaveCountry();

            GeoTestControl.SaveCountry(countryName);   // Сохраняем название страны в файл
            GeoTestControl.SaveCreateTime();           // Сохраняем время создания

            newCountryName = countryName;               // Добавляем название для дальнейших действий
        }
        else
        {
            Console.WriteLine("Страна уже создана менее 6 часов назад. Редактируем существующую.");
            await _geoPage.AddOneToCountryName();
            newCountryName = await _geoPage.GetCountryName();
            Console.WriteLine($"Новое название страны: {newCountryName}");
        }
        
        await _geoPage.AddRegionForCountry(newCountryName); // открываем создание региона
                                                            // 
        string regionName = "TestRegion-UI";        // создаем регион
        await _geoPage.FillRegionName(regionName);
        await _geoPage.SaveRegion();

        Console.WriteLine(
            $"Создан регион: {regionName}"
        );
    }
}