
using Microsoft.Playwright;                                   // Подключаем библиотеку Playwright

namespace GW.UI.Tests.Pages;                                  // Пространство имён проекта

public class LoginPage                                        // Page Object страницы авторизации
{
    private readonly IPage _page;                             // Текущая вкладка браузера

    public LoginPage(IPage page)                              // Конструктор страницы
    {
        _page = page;                                         // Сохраняем экземпляр страницы
    }

    public async Task Open()                                  // Открыть страницу логина
    {
        await _page.GotoAsync(
            "https://gw-exp.dev.artintech.ru/Account/Login"); // Переход на страницу авторизации
    }

    public async Task Login(string email, string password)    // Выполнить вход в систему
    {
        await _page.Locator("input")                          // создаёт ссылку (локатор) на элементы <input>
            .Nth(0)                                           // Поле Email
            .FillAsync(email);                                // Ввод email

        await _page.Locator("input[type='password']")
            .FillAsync(password);                             // Ввод пароля

        await _page.GetByRole(
                AriaRole.Button,
                new() { Name = "Войти" })                     // Кнопка "Войти"
            .ClickAsync();                                    // Нажатие на кнопку
    }

    public async Task OpenForgotPassword()                    // Открыть форму восстановления пароля
    {
        await _page.GetByRole(
                AriaRole.Link,
                new() { Name = "Забыли пароль?" })            // Ссылка восстановления пароля
            .ClickAsync();                                    // Переход на страницу восстановления
    }

    public async Task SendPasswordResetLink(string email)     // Отправить ссылку для сброса пароля
    {
        await _page.Locator("input[maxlength='1000']")
            .FillAsync(email);                                // Ввод email для восстановления

        await _page.GetByRole(
                AriaRole.Button,
                new() { Name = "Отправить ссылку" })          // Кнопка отправки ссылки
            .ClickAsync();                                    // Отправка запроса
    }
}
