using Microsoft.Playwright;
using System.Threading.Tasks;       // Для работы с async/await

namespace GW.UI.Tests.Pages;              // Пространство имён проекта

public class OrganizationsPage            // Класс страницы "Организации"
{
    private readonly IPage _page;           // Ссылка на открытую страницу браузера

    public OrganizationsPage(IPage page) // Конструктор страницы
    {
        _page = page;
    }

    public async Task OpenCatalogs()
    {
        await _page.GetByText("Справочники").ClickAsync();
    }

    public async Task OpenOrganizations()
    {
        await _page.GetByText("Организации").ClickAsync();
    }

    public async Task Open()    // Открывает страницу организаций
    {
        await _page.GotoAsync("https://gw-exp.dev.artintech.ru/Catalogs/Company/Divisions/Organizations");
    }

    // Первая организация в списке
    private ILocator FirstOrganization =>
    _page.Locator(".list-item a").First;

    public async Task OpenFirstOrganization()
    {
        await FirstOrganization.WaitForAsync(new()
        {
            State = WaitForSelectorState.Visible
        });

        await FirstOrganization.ClickAsync();
    }

    public string GetCurrentUrl()               // Возвращает текущий URL
    {
        return _page.Url;
    }

}