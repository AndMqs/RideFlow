public static class PriceRules
{
    public static decimal GetPricePerKm(string category, decimal km)
    {
        decimal basePrice;
        decimal multiplier;

        switch (category.ToLower())
        {
            case "basic":
                basePrice = 5m;
                multiplier = 1.5m;
                break;

            case "premium":
                basePrice = 8m;
                multiplier = 2.0m;
                break;

            case "vip":
                basePrice = 10m;
                multiplier = 3.0m;
                break;

            default:
                throw new Exception("Categoria inválida");
        }

        return basePrice + (multiplier * km);
    }
}