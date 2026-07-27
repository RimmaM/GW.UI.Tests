using Microsoft.Playwright;
using System.Threading.Tasks;       // Для работы с async/await

namespace GW.UI.Tests.Pages;              // Пространство имён проекта

public class PositionsPage            // Класс страницы "Организации"
{
    private readonly IPage _page;           // Ссылка на открытую страницу браузера

    public PositionsPage(IPage page) // Конструктор страницы
    {
        _page = page;
    }

    public async Task Open()    // Открывает страницу организаций
    {
        await _page.GotoAsync("https://gw-exp.dev.artintech.ru/Catalogs/Positions");
    }

    // Первая организация в списке
    private ILocator FirstPosition =>
    _page.Locator(".list-item a").First;

    public async Task OpenFirstPosition()
    {
        await FirstPosition.WaitForAsync(new()
        {
            State = WaitForSelectorState.Visible
        });

        await FirstPosition.ClickAsync();
    }

    public string GetCurrentUrl()               // Возвращает текущий URL
    {
        return _page.Url;
    }

}