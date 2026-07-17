using GW.UI.Tests.Helpers;
using Microsoft.Playwright;
using static Microsoft.Playwright.Assertions;

using NUnit.Framework;
using GW.UI.Tests.Pages;
using GW.UI.Tests.TestData;

namespace GW.UI.Tests.Tests;                    // Пространство имен

public class CreateTravelTests                  // Класс с тестами
{

    private IPlaywright _playwright;         // Экземпляр Playwright
    private IBrowser _browser;               // Браузер
    private IPage _page;                     // Вкладка браузера

    private LoginPage _loginPage;            // Страница логина
    private TravelsPage _travelsPage;        // Страница поездок

    [SetUp]
    public async Task SetUp()
    {
        _playwright =
            await Playwright.CreateAsync();                      // Запускаем Playwright

        _browser =
            await _playwright.Chromium.LaunchAsync(
                new()
                {
                    Headless = false
                });

        _page =
            await _browser.NewPageAsync();                       // Создаем вкладку

        _loginPage =
            new LoginPage(_page);                               // Инициализируем LoginPage

        _travelsPage =
            new TravelsPage(_page);                             // Инициализируем TravelsPage

        await _loginPage.Open();                                // Открываем страницу логина

        await _loginPage.Login(
            Users.Email,
            Users.Password);                                    // Выполняем вход

        await _page.WaitForURLAsync("**/Travels");              // Ждем страницу поездок

    }

    [TearDown]
    public async Task TearDown()
    {
        await _browser.CloseAsync();                            // Закрываем браузер

        _playwright.Dispose();                                 // Освобождаем Playwright
    }




    [Test]                                                               // Тест закрытия диалога создания АО
    public async Task Dialog_Closes_By_Close_Button()                    // Проверяем закрытие по крестику
    {
        await _travelsPage.OpenCreateTravelForm();                        // Открываем диалог

        var cancelButton1 =
            _page.GetByRole(
                AriaRole.Button,
                new() { Name = "Отменить" });                            // Кнопка Отменить внутри диалога

        await Expect(cancelButton1)
            .ToBeVisibleAsync();                                         // Диалог открылся

        var modal =
        _page.Locator("#myModal");

        await modal
            .GetByRole(AriaRole.Button)
            .First
            .ClickAsync();                                           // Нажимаем крестик

        await Expect(cancelButton1)
            .Not.ToBeVisibleAsync();                                 // Диалог закрылся

        Console.WriteLine($"Закрытие диалога создания АО через иконку закрытия Х прошло успешно");
       
    }


     [Test]                                                               // Тест
    public async Task Dialog_Closes_By_Cancel_Button()                   // Проверяем кнопку Отменить
    {
        await _travelsPage.OpenCreateTravelForm();                        // Открываем диалог

        var cancelButton =
            _page.GetByRole(
                AriaRole.Button,
                new() { Name = "Отменить" });                            // Ищем кнопку Отменить

        await cancelButton.ClickAsync();                                 // Нажимаем Отменить

        await Expect(cancelButton)
            .Not.ToBeVisibleAsync();                                     // Диалог закрылся

        Console.WriteLine($"Закрытие диалога создания АО по кнопке Отменить прошло успешно");
    }


    [Test]                                                               // Тест
    public async Task Create_Travel_If_Not_Exists()                     // Проверяем создание поездки через кнопку Сохранить
    {
        Console.WriteLine(
            $"CanRunCreateTest = {TravelTestControl.CanRunCreateTest()}"); // Выводим результат проверки возможности создания поездки

        Console.WriteLine(
            Path.GetFullPath("LastTravelCreate.txt"));                   // Выводим полный путь к файлу с данными о последней поездке

        if (!TravelTestControl.CanRunCreateTest())                         // Проверяем, создавалась ли поездка за последние n часов
        {
            Console.WriteLine(
                "Поездка уже создавалась за последние 6 ч");

            return;                                                        // Пропускаем тест, если поездка уже была создана
        }

        await _travelsPage.OpenCreateTravelForm();                        // Открываем диалог

        var checkbox =
            _page.GetByText("Авансовый отчет по командировке");           // Находим чекбокс

        await checkbox.CheckAsync();                                      // Включаем чекбокс

        await Expect(checkbox)
            .ToBeCheckedAsync();                                           // Проверяем что включился

        var saveButton =
            _page.GetByRole(
                AriaRole.Button,
                new() { Name = "Сохранить" });                             // Кнопка Сохранить

        await saveButton.ClickAsync();                                     // Нажимаем Сохранить

        await _page.WaitForURLAsync(
            url => url.Contains("/Travel/"));                            // Ждем открытия карточки

        var travelId =
            _page.Url.Split('/').Last();                                  // Получаем ID из URL

        TravelTestControl.SaveCreatedTravel(travelId);                   // Сохраняем дату и ID поездки

        await Expect(saveButton)
            .Not.ToBeVisibleAsync();                                     // Диалог закрылся

        Console.WriteLine($"Создана поездка: {travelId}");

    }

  //  [Test]
    public async Task Open_Last_Created_Travel()
    {
        var travelId =
            TravelTestControl.GetLastTravelId();                // Получаем ID поездки

        Assert.That(
            travelId,
            Is.Not.Empty,
            "Не найден ID ранее созданной поездки");

        await _page.GotoAsync(
            $"https://gw-exp.dev.artintech.ru/Travel/{travelId}"); // Открываем карточку

        await _page.WaitForURLAsync(
            $"**/Travel/{travelId}");

        Console.WriteLine(
            $"Открыта поездка {travelId}");
    }

    [Test]
    public async Task Edit_Last_Created_Travel()                    // Редактируем последнюю созданную поездку
    {
        var travelId =
            TravelTestControl.GetLastTravelId();                    // Получаем ID последней созданной поездки

        Assert.That(
            travelId,
            Is.Not.Empty,
            "Не найден ID ранее созданной поездки");

        await _page.GotoAsync(
            $"https://gw-exp.dev.artintech.ru/Travel/{travelId}");  // Открываем карточку поездки

        await _page.WaitForURLAsync(
            $"**/Travel/{travelId}");                               // Ожидаем загрузки карточки

        var travelCardPage =
            new TravelCardPage(_page);                              // Создаем объект страницы

        await travelCardPage.SetDepartureDate();                    // Устанавливаем дату выезда = сегодня

        await travelCardPage.SetArrivalDate();                      // Устанавливаем дату приезда = сегодня +14 дней

        await travelCardPage.SelectFirstCity();                     // Выбираем город пребывания сотрудника

        var cityValue = await _page
            .Locator("div.field-base")
            .Filter(new() { HasText = "Город пребывания сотрудника" })
            .Locator(".selector-field")
            .TextContentAsync();

        Console.WriteLine($"Выбран город: {cityValue}");


      /**  var cities = new[]
         {
                 new { Name = "Москва",      Type = "Целевой город",    Date = DateTime.Today.AddDays(2) },
                 new { Name = "Казань",      Type = "Транзитный город", Date = DateTime.Today.AddDays(5) },
                 new { Name = "Нижний Новгород",      Type = "Целевой город", Date = DateTime.Today.AddDays(7) },
         };

         for (int i = 0; i < cities.Length; i++)
         {
             await _page
                 .Locator("button.button-icon-primary")
                 .ClickAsync();                                      // Добавляем новую строку города

             await _page
                 .Locator(".ct-list-item")
                 .Nth(i)
                 .WaitForAsync();                                    // Ждем появления строки

             var cityRow = _page
                 .Locator(".ct-list-item")
                 .Nth(i);                                            // Получаем текущую строку
            
            var cityText = await cityRow
                .Locator(".field-base-text")
                .First
                .TextContentAsync();

            Console.WriteLine(
                $"Строка {i}: выбран город {cityText}");

            await cityRow
                 .Locator(".selector-field")
                 .Nth(0)
                 .ClickAsync();                                      // Открываем выбор города

             var citySearch = _page
                 .Locator(".selector-popup:visible input[placeholder='Поиск']")
                 .Last;

             await citySearch.FillAsync(cities[i].Name);             // Вводим название города

             var cityOption = _page
                 .Locator(".selector-popup:visible .option-item")
                 .Filter(new() { HasText = cities[i].Name })
                 .First;

             await cityOption.WaitForAsync();                        // Ждем появления города в списке

             await cityOption.ClickAsync();                          // Выбираем город

             await cityRow
                 .Locator(".selector-field")
                 .Nth(1)
                 .ClickAsync();                                      // Открываем выбор типа города

             var cityType = _page
                 .Locator(".selector-popup:visible .option-item")
                 .Filter(new() { HasText = cities[i].Type })
                 .First;

             await cityType.WaitForAsync();                          // Ждем появления типа города

             await cityType.ClickAsync();                            // Выбираем тип города

             await cityRow
                 .Locator("input.clear-input")
                 .Last
                 .FillAsync(cities[i].Date.ToString("dd.MM.yyyy"));  // Заполняем дату пребывания

            var inputs = cityRow.Locator("input.clear-input");

            var dateValue = await cityRow
                .Locator("input.clear-input")
                .Last
                .InputValueAsync();

            Console.WriteLine(
                $"Строка {i}: дата = {dateValue}");

            Console.WriteLine(
                $"Строка {i}: найдено {await inputs.CountAsync()} input");
        }*/

        Console.WriteLine(
            $"Поездка {travelId} успешно отредактирована");         // Выводим результат в лог
    }

}