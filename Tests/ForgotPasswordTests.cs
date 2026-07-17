using Microsoft.Playwright;          // Подключаем библиотеку Playwright
using NUnit.Framework;              // Подключаем NUnit для тестов
using GW.UI.Tests.Pages;            // Подключаем папку Pages

namespace GW.UI.Tests.Tests;        // Пространство имен для тестов

public class ForgotPasswordTests    // Класс с тестами восстановления пароля
{
    [Test]                          // Атрибут NUnit: это тестовый метод
    public async Task Send_Reset_Password_Link_Test()
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

        await loginPage.Open();                  // Открываем страницу логина

        await loginPage.OpenForgotPassword();    // Нажимаем ссылку "Забыли пароль?"

        await page.WaitForURLAsync(              // Ждем открытия страницы восстановления пароля
            "**/Account/ForgotPassword*");

        await loginPage.SendPasswordResetLink(   // Отправляем ссылку для восстановления пароля
            "maksimova_rv@artintech.ru");

        Assert.That(                             // Проверяем результат теста
            page.Url,                            // Получаем текущий URL страницы
            Does.Contain("ForgotPassword"));     // URL должен содержать "ForgotPassword"
    }
}