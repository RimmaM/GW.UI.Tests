using Microsoft.Playwright;
using System;
using System.IO;

namespace GW.UI.Tests.Helpers;

public static class GeoTestControl
{
    private const string DateFile = "LastCountryCreate.txt";
    private const string CountryFile = "LastCountry.txt";


    public static bool CanCreateCountry()           // Проверяет, можно ли создавать новую страну
    {
        if (!File.Exists(DateFile))
            return true;

        var lastCreate = DateTime.Parse(File.ReadAllText(DateFile));

        return DateTime.Now - lastCreate > TimeSpan.FromHours(6);
    }


    public static void SaveCreateTime()              // Запоминает время создания страны
    {
        File.WriteAllText(DateFile, DateTime.Now.ToString("O"));
    }


    public static void SaveCountry(string countryName, string countryId)   // Сохраняет название последней созданной страны
    {
        File.WriteAllText(CountryFile, countryName);
    }


    public static string GetCountryName()             // Получает название последней созданной страны
    {
        if (!File.Exists(CountryFile))
            return null;

        return File.ReadAllText(CountryFile);
    }
}