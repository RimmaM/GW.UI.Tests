using Microsoft.Playwright;

namespace GW.UI.Tests.Pages;

public class DivisionsPage
{
    private readonly IPage _page; // Ссылка на открытую страницу браузера

    public DivisionsPage(IPage page) // Конструктор страницы
    {
        _page = page;
    }

    public async Task Open()    // Открывает страницу подразделений
    {
        await _page.GotoAsync("https://gw-exp.dev.artintech.ru/Catalogs/Company/Divisions"); 
    }

    public async Task ExpandDivision(string divisionName)                   // Раскрывает подразделение по названию
    {
        var division = _page.Locator(".list-item")
            .Filter(new() { HasText = divisionName });                                        // Находим строку подразделения

        await division.Locator(".button-node-open").ClickAsync();                            // Нажимаем на шеврон
    }

    public async Task OpenManager(string lastName)                                          // Открывает карточку руководителя по фамилии
    {
        var manager = _page.Locator(".list-item")
            .Filter(new() { HasText = lastName })
            .Locator("a[href*='EmployeeView']");                                              // Находим ссылку руководителя

        await manager.ClickAsync();                                                           // Открываем карточку
        await _page.WaitForURLAsync("**/Catalogs/EmployeeView/**");                           // Ждем открытия страницы
    }

    public string GetCurrentUrl()       // Возвращает текущий URL
    {
        return _page.Url;
    }
}