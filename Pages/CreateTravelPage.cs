using Microsoft.Playwright;                       // Библиотека Playwright для UI-тестов

public class CreateTravelPage                     // Page Object страницы создания поездки
{
    private readonly IPage _page;                 // Текущая страница браузера

    public CreateTravelPage(IPage page)           // Конструктор страницы
    {
        _page = page;                             // Сохраняем экземпляр страницы
    }

    public ILocator SaveButton =>                 // Кнопка сохранения поездки
        _page.GetByRole(
            AriaRole.Button,                      // Ищем элемент типа Button
            new() { Name = "Сохранить" });        // С текстом "Сохранить"

    public ILocator CancelButton =>               // Кнопка отмены создания поездки
        _page.GetByRole(
            AriaRole.Button,                      // Ищем элемент типа Button
            new() { Name = "Отменить" });         // С текстом "Отменить"
}