using Microsoft.Playwright;          // Подключаем библиотеку Playwright
using NUnit.Framework;              // Подключаем NUnit для тестов
using GW.UI.Tests.Pages;            // Подключаем папку Pages
using GW.UI.Tests.TestData;         // Подключаем тестовые данные

namespace GW.UI.Tests.Tests;        // Пространство имен для тестов

public class LoginTests             // Класс с тестами авторизации
{
    [Test]                          // Атрибут NUnit: это тестовый метод
    public async Task Positive_Login_Test()
    {
        using var playwright =
            await Playwright.CreateAsync();    // Создаем экземпляр Playwright

        await using var browser =
            await playwright.Chromium.LaunchAsync(
                new BrowserTypeLaunchOptions
                {
                    Headless = false           // false = браузер виден пользователю
                });

        var page = await browser.NewPageAsync(); // Открываем новую вкладку браузера

        var loginPage = new LoginPage(page);     // Создаем объект страницы авторизации

        await loginPage.Open();                  // Переходим на страницу логина

        await loginPage.Login(                   // Выполняем авторизацию
            Users.Email,                         // Логин из файла Users.cs
            Users.Password);                     // Пароль из файла Users.cs

        await page.WaitForURLAsync("**/Travels"); // Ждем перехода на страницу Travels

        Assert.That(                              // Проверяем результат теста
            page.Url,                             // Текущий URL страницы
            Does.Contain("Travels"));             // URL должен содержать "Travels"
    }
}