using Microsoft.Playwright;                                      // Playwright

namespace GW.UI.Tests.Pages;                                     // Пространство имен

public class TravelsPage                                         // Страница списка поездок
{
    private readonly IPage _page;                                // Ссылка на страницу

    public TravelsPage(IPage page)
    {
        _page = page;                                            // Сохраняем страницу
    }

    public ILocator CreateTravelButton =>                        // Локатор кнопки создания отчета
        _page.GetByRole(
            AriaRole.Button,
            new()
            {
                Name = "Оформить авансовый отчет"
            });

    public async Task OpenCreateTravelForm()                     // Нажать кнопку создания отчета
    {
        await CreateTravelButton.ClickAsync();
    }
}