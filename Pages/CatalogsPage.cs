using Microsoft.Playwright;

namespace GW.UI.Tests.Pages;

/// <summary>
/// Страница "Сотрудники"
/// </summary>
public class CatalogsPage
{
    private readonly IPage _page; // Ссылка на открытую страницу браузера

    public CatalogsPage(IPage page) // Конструктор страницы
    {
        _page = page;
    }

    private ILocator SearchField => _page.Locator("input[data-searchfield]");                          // Поле поиска сотрудников
    private ILocator StatusSelector => _page.Locator(".selector-field");                               // Выпадающий список "Статус"
    private ILocator ConfirmedStatus => _page.Locator(".option-item[data-id='Confirmed']");            // Пункт "Подтвержден"
    private ILocator ResetButton => _page.GetByRole(AriaRole.Button, new() { Name = "Сбросить все" }); // Кнопка "Сбросить все"
    private ILocator ClearSearchButton => _page.Locator(".field-base-icons .icon-close"); // Кнопка очистки поля поиска

    // Открывает страницу сотрудников
    public async Task Open()
    {
        await _page.GotoAsync("https://gw-exp.dev.artintech.ru/Catalogs"); // Переходим на страницу сотрудников
        Console.WriteLine($"URL: {_page.Url}");                            // Выводим текущий адрес страницы
        await SearchField.WaitForAsync();                                  // Ждем появления поля поиска
    }

    // Выбирает статус "Подтвержден"
    public async Task SelectConfirmedStatus()
    {
        await StatusSelector.ClickAsync();                           // Открываем список
        await ConfirmedStatus.WaitForAsync();                        // Ждем появления пункта "Подтвержден"
        await ConfirmedStatus.ClickAsync();                          // Выбираем "Подтвержден"
    }

    // Сбрасывает все фильтры
    public async Task ResetFilters()
    {
        await ResetButton.ClickAsync();                                     // Нажимаем кнопку "Сбросить все"
        await SearchField.WaitForAsync();      // Ждем, пока поле поиска снова станет доступным
        Console.WriteLine(await SearchField.InputValueAsync());
    }

    // Выполняет поиск сотрудника    
    public async Task SearchEmployee(string employeeName)     // <param name="employeeName">Фамилия сотрудника</param>
    {
        await SearchField.ClearAsync();            // Очищаем поле поиска
        await SearchField.FillAsync(employeeName); // Вводим фамилию сотрудника
        await _page.WaitForTimeoutAsync(1000);     // Ждем обновления списка
    }

    // Очищает поле поиска нажатием на крестик
    public async Task ClearSearch()
    {
        await ClearSearchButton.ClickAsync();      // Нажимаем на крестик
        await _page.WaitForTimeoutAsync(500);      // Ждем очистки поля
    }

    // Открывает карточку сотрудника по фамилии
    public async Task OpenEmployee(string lastName)                 //<param name="lastName">Фамилия сотрудника</param>
    {
        var employee = _page.Locator(".list-item")                                   // Находим все строки списка сотрудников
            .Filter(new() { HasText = lastName })                                    // Оставляем строку с нужной фамилией
            .Locator("a[href*='EmployeeView']");                                     // Находим ссылку с номером сотрудника

        await employee.ClickAsync();                                                 // Открываем карточку сотрудника
        await _page.WaitForURLAsync("**/Catalogs/EmployeeView/**");                  // Ждем открытия страницы сотрудника
    }

    // Возвращает значение поля поиска
    public async Task<string> GetSearchValue()
    {
        return await SearchField.InputValueAsync(); // Получаем текст из поля поиска
    }

    // Возвращает текущий адрес страницы
    public string GetCurrentUrl()
    {
        return _page.Url; // Возвращаем адрес открытой страницы
    }



}