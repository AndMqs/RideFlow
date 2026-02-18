public static class DriverCategoryRules
{
    public static string GetCategoryByCarYear(int yearCar)
    {
        if (yearCar >= 2023)
            return "vip";
            
        if (yearCar >= 2016)
            return "premium";
            
        if (yearCar <= 2015)
            return "basic";

        return "Categoria desconhecida";
    }
}