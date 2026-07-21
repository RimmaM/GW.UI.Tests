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

    /// <summary>
    /// Открывает страницу сотрудников
    /// </summary>
    public async Task Open()
    {
        await _page.GotoAsync("https://gw-exp.dev.artintech.ru/Catalogs"); // Переходим на страницу сотрудников
        //await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);           // Ждем полной загрузки страницы
        await SearchField.WaitForAsync();                                  // Ждем появления поля поиска
    }

    /// <summary>
    /// Выбирает статус "Подтвержден"
    /// </summary>
    public async Task SelectConfirmedStatus()
    {
        await StatusSelector.ClickAsync();                           // Открываем список
        await ConfirmedStatus.WaitForAsync();                        // Ждем появления пункта "Подтвержден"
        await ConfirmedStatus.ClickAsync();                          // Выбираем "Подтвержден"
    }

    /// <summary>
    /// Сбрасывает все фильтры
    /// </summary>
    public async Task ResetFilters()
    {
        await ResetButton.ClickAsync();                                     // Нажимаем кнопку "Сбросить все"
        //await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);           // Ждем обновления списка
        await SearchField.WaitForAsync();      // Ждем, пока поле поиска снова станет доступным
        Console.WriteLine(await SearchField.InputValueAsync());
    }

    /// <summary>
    /// Выполняет поиск сотрудника
    /// </summary>
    /// <param name="employeeName">Фамилия сотрудника</param>
    public async Task SearchEmployee(string employeeName)
    {
        await SearchField.ClearAsync();                                     // Очищаем поле поиска
        await SearchField.FillAsync(employeeName);                          // Вводим фамилию сотрудника
       // await SearchField.PressAsync("Enter");                              // Запускаем поиск
        //await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);           // Ждем обновления списка
        await _page.WaitForTimeoutAsync(1000);     // Даем странице обновить список
    }


    /// <summary>
    /// Открывает карточку сотрудника по фамилии
    /// </summary>
    /// <param name="lastName">Фамилия сотрудника</param>
    public async Task OpenEmployee(string lastName)
    {
        var employee = _page.Locator(".list-item")                                   // Находим все строки списка сотрудников
            .Filter(new() { HasText = lastName })                                    // Оставляем строку с нужной фамилией
            .Locator("a[href*='EmployeeView']");                                     // Находим ссылку с номером сотрудника

        await employee.ClickAsync();                                                 // Открываем карточку сотрудника
        await _page.WaitForURLAsync("**/Catalogs/EmployeeView/**");                  // Ждем открытия страницы сотрудника
    }

    /// <summary>
    /// Возвращает значение поля поиска
    /// </summary>
    public async Task<string> GetSearchValue()
    {
        return await SearchField.InputValueAsync(); // Получаем текст из поля поиска
    }

    /// <summary>
    /// Возвращает текущий адрес страницы
    /// </summary>
    public string GetCurrentUrl()
    {
        return _page.Url; // Возвращаем адрес открытой страницы
    }



}