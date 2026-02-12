public static class DriverCategoryRules
{
    public static string GetCategoryByCarYear(int YearCar)
    {
             if (YearCar <= 2010)
            return "basic";

        if (YearCar >= 2011 && YearCar <= 2020)
            return "premium";

        return "vip";
    }
}