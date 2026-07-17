using System;                    // DateTime, TimeSpan
using System.IO;                 // Работа с файлами

namespace GW.UI.Tests.Helpers;

/*
    Хранит информацию о последней созданной тестовой поездке:
    сохраняет её ID и дату создания, а также определяет,
    нужно ли создавать новую поездку при запуске тестов.
 */
public static class TravelTestControl
{
    private const string DateFile =
        "LastTravelCreate.txt";  // Дата последнего создания поездки

    private const string IdFile =
        "LastTravelId.txt";      // ID последней созданной поездки


    public static bool CanRunCreateTest(bool travelExists = true)
    {
        if (!File.Exists(DateFile))      // Поездка еще ни разу не создавалась
            return true;                 // Можно создавать новую

        var lastRun =
            DateTime.Parse(File.ReadAllText(DateFile)); // Читаем дату создания

        var moreThan24HoursPassed =
            DateTime.Now - lastRun >
            TimeSpan.FromHours(6);      // Проверяем прошло ли 6 часа

        return moreThan24HoursPassed     // Не создавать поездки чаще 1 раза в 6 часа
               || !travelExists;         // Создать новую, если предыдущая поездка удалена/не найдена
    }

    public static void SaveCreatedTravel(string id)
    {
        File.WriteAllText(
            DateFile,
            DateTime.Now.ToString("O")); // Сохраняем текущее время создания

        File.WriteAllText(
            IdFile,
            id);                         // Сохраняем ID созданной поездки
    }

    public static string GetLastTravelId()
    {
        if (!File.Exists(IdFile))        // Если ID не сохранен
            return string.Empty;         // Возвращаем пустую строку

        return File.ReadAllText(IdFile); // Возвращаем последний сохраненный ID
    }
}