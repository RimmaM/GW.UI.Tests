using Microsoft.Playwright;
namespace GW.UI.Tests.Pages;              // Пространство имён проекта

public class GeoPage            // Класс страницы "Организации"
{
    private readonly IPage _page;           // Ссылка на открытую страницу браузера


    private ILocator AddCountryButton =>        // Кнопка "Добавить страну"
        _page.GetByRole(AriaRole.Button, new() { Name = "Добавить страну" });

    private ILocator CountryNameField =>        // Поле "Наименование страны"
        _page.Locator("#geo-name-input");

    private ILocator SaveButton =>               // Кнопка "Сохранить"
        _page.GetByRole(AriaRole.Button, new() { Name = "Сохранить" });

    public GeoPage(IPage page) // Конструктор страницы
    {
        _page = page;
    }


    public async Task OpenCatalogs()
    {
        await _page.GetByText("Справочники").ClickAsync();
    }

    public async Task OpenGeo()
    {
        await _page.GetByText("Населенные пункты").ClickAsync();
    }
    
    public async Task ClickAddCountry()                 // Открывает окно создания страны
    {
        await AddCountryButton.ClickAsync();
    }
    
    public async Task FillCountryName(string countryName) // Заполняет название страны
    {
        await CountryNameField.FillAsync(countryName);
    }
    
    public async Task SaveCountry()                         // Сохраняет страну
    {
        await SaveButton.ClickAsync();
    }

    public async Task OpenCountry(string countryName)
    {
        var countryButton = _page.GetByRole(
            AriaRole.Button,
            new() { Name = countryName });

        Console.WriteLine($"Кнопка найдена: {await countryButton.IsVisibleAsync()}");

        await countryButton.ClickAsync();
        Console.WriteLine("Клик выполнен");

        await _page.WaitForTimeoutAsync(3000);
    }

    public async Task OpenEditCountry(string countryName)
    {
        var countryRow = _page
            .Locator("div.list-item")
            .Filter(new()
            {
                HasText = countryName
            });

        await countryRow
            .Locator("button")
            .ClickAsync();
    }

    public async Task<string> AddOneToCountryName()     // Дописывает "1" к названию страны
    {
        string currentName = await CountryNameField.InputValueAsync();

        string newName = currentName + "1";

        await CountryNameField.FillAsync(newName);

        return newName;
    }

    public async Task AddRegionForCountry(string countryName)
    {
        var countryRow = _page
            .Locator("div.list-item")
            .Filter(new()               //оставляет только строку с Test-UI
            {
                HasText = countryName
            });

        await countryRow
            .Locator("button.button-icon-primary")
            .ClickAsync();
    }

    public async Task FillRegionName(string regionName)
    {
        await _page
            .Locator("input")
            .FillAsync(regionName);
    }

    public async Task SaveRegion()
    {
        await _page
            .GetByRole(AriaRole.Button, new()
            {
                Name = "Сохранить"
            })
            .ClickAsync();
    }

    public async Task<string> GetCountryName()
    {
        return await _page
            .Locator("ui-parameter-col")
            .Locator("button.button-text")
            .InnerTextAsync();
    }

}