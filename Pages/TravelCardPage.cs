using Microsoft.Playwright;

namespace GW.UI.Tests.Pages;                           // Пространство имен

public class TravelCardPage                            // Page Object карточки поездки
{
    private readonly IPage _page;                      // Текущая страница браузера

    public TravelCardPage(IPage page)                  // Конструктор класса
    {
        _page = page;                                  // Сохраняем экземпляр страницы
    }

    public async Task SetDepartureDate()
    {
        var departureDate = _page
            .Locator("div.field-base")
            .Filter(new() { HasText = "Дата выезда" })
            .Locator("input.clear-input");

        await departureDate.ClickAsync();

        await departureDate.PressAsync("Control+A");

        await departureDate.PressAsync("Backspace");

        await departureDate.FillAsync(
            DateTime.Today.ToString("dd.MM.yyyy"));

        await departureDate.PressAsync("Tab");

        Console.WriteLine(
            $"Дата выезда: {await departureDate.InputValueAsync()}");
    }

    public async Task SetArrivalDate()
    {
        var arrivalDate = _page
            .Locator("div.field-base")
            .Filter(new() { HasText = "Дата приезда" })
            .Locator("input.clear-input");

        await arrivalDate.ClickAsync();

        await arrivalDate.PressAsync("Control+A");

        await arrivalDate.PressAsync("Backspace");

        await arrivalDate.FillAsync(
            DateTime.Today.AddDays(14).ToString("dd.MM.yyyy"));

        await arrivalDate.PressAsync("Tab");

        Console.WriteLine(
            $"Дата приезда: {await arrivalDate.InputValueAsync()}");
    }

    public async Task SelectFirstCity(string city = "Сочи")
    {
        var cityBlock = _page
            .Locator("div.field-base")
            .Filter(new() { HasText = "Город пребывания сотрудника" }); // Находим блок "Город пребывания сотрудника"

        await cityBlock
            .Locator(".selector-field")
            .ClickAsync();                                              // Открываем справочник городов

        var popup = _page
            .Locator(".selector-popup-wrapper:not(.d-none)");           // Получаем открывшееся окно выбора

        var search = popup
            .Locator("input[placeholder='Поиск']");                     // Находим поле поиска города

        await search
            .FillAsync(city);                                           // Вводим название города

        var cityOption = popup
            .Locator(".option-item")
            .Filter(new() { HasText = city })
            .First;                                                     // Находим нужный город в списке результатов

        await cityOption
            .WaitForAsync();                                            // Ожидаем появления найденного города

        Console.WriteLine(
            $"Выбираем город: {await cityOption.TextContentAsync()}");  // Выводим выбранный город в лог

        await cityOption
            .ClickAsync();                                              // Выбираем город
    }
}