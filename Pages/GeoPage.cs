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
        await _page
            .GetByRole(AriaRole.Button, new()
            {
                Name = countryName,
                Exact = true
            })
            .First
            .ClickAsync();
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

    public async Task<string> AddOneToCountryName()
    {
        await CountryNameField.WaitForAsync(new()
        {
            State = WaitForSelectorState.Visible
        });

        string currentName = await CountryNameField.InputValueAsync();

        string newName = currentName + "1";

        await CountryNameField.FillAsync(newName);

        return newName;
    }

    public async Task AddRegionForCountry(string countryName)
    {
        var countryRow = _page
            .Locator("div.list-item")
            .Filter(new()
            {
                Has = _page.GetByRole(AriaRole.Button, new()
                {
                    Name = countryName,
                    Exact = true
                })
            })
            .First;

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