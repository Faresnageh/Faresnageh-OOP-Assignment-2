namespace SrpLab;

public sealed class KitchenTicket
{
    private readonly List<(string Item, List<string> Ingredients, int PrepMinutes)> _items = new();

    public void AddItem(string item, IEnumerable<string> ingredients, int prepMinutes)
    {
        _items.Add((item, ingredients.Select(i => i.Trim().ToLowerInvariant()).ToList(), prepMinutes));
    }

    public List<(string Item, List<string> Ingredients, int PrepMinutes)> GetItems()
    {
        return _items.Select(x => (x.Item, x.Ingredients.ToList(), x.PrepMinutes)).ToList();
    }
}

public sealed class AllergenDetector
{
    public IReadOnlyList<string> DetectAllergens(KitchenTicket kitchenTicket)
    {
        var hits = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        foreach (var item in kitchenTicket.GetItems())
        {
            foreach (var ing in item.Ingredients)
            {
                if (ing.Contains("milk") || ing.Contains("cheese") || ing.Contains("butter"))
                    hits.Add("dairy");
                if (ing.Contains("wheat") || ing.Contains("flour") || ing.Contains("bread"))
                    hits.Add("gluten");
                if (ing.Contains("peanut") || ing.Contains("almond") || ing.Contains("cashew"))
                    hits.Add("nuts");
                if (ing.Contains("shrimp") || ing.Contains("prawn") || ing.Contains("crab"))
                    hits.Add("shellfish");
            }
        }
        return hits.OrderBy(x => x).ToList();
    }
}

public sealed class ReadyTimeEstimator
{
    public int EstimatedReadyMinutes(KitchenTicket kitchenTicket, int openStations)
    {
        if (openStations <= 0)
            openStations = 1;

        var items = kitchenTicket.GetItems();
        var sequential = items.Sum(i => i.PrepMinutes);
        var parallel = (int)Math.Ceiling(sequential / (double)openStations);

        var allergenDetector = new AllergenDetector();
        if (allergenDetector.DetectAllergens(kitchenTicket).Count > 0)
            parallel += 3;

        var longest = items.Count == 0 ? 0 : items.Max(i => i.PrepMinutes);

        return Math.Max(parallel, longest);
    }
}

public sealed class ThermalTicketRenderer
{
    public string RenderThermalTicket(KitchenTicket kitchenTicket, int orderNumber)
    {
        var width = 32;
        var line = new string('=', width);
        var items = kitchenTicket.GetItems();

        var body = string.Join('\n', items.Select(i => $"* {i.Item.ToUpperInvariant()} ({i.PrepMinutes}m)"));

        var allergenDetector = new AllergenDetector();
        var allergens = allergenDetector.DetectAllergens(kitchenTicket);

        var allergyLine = allergens.Count == 0
            ? "ALLERGENS: none"
            : "ALLERGENS: " + string.Join(",", allergens);

        var readyTimeEstimator = new ReadyTimeEstimator();
        var eta = readyTimeEstimator.EstimatedReadyMinutes(kitchenTicket, 2);

        return $"{line}\nORDER #{orderNumber}\nETA {eta} MIN\n{body}\n{allergyLine}\n{line}\n";
    }
}

public sealed class ExpoLaneSelector
{
    public string ExpoLaneHint(KitchenTicket kitchenTicket)
    {
        var allergenDetector = new AllergenDetector();

        if (allergenDetector.DetectAllergens(kitchenTicket).Count > 0)
            return "LANE-ALLERGY";

        var readyTimeEstimator = new ReadyTimeEstimator();

        if (readyTimeEstimator.EstimatedReadyMinutes(kitchenTicket, 2) > 20)
            return "LANE-SLOW";

        return "LANE-FAST";
    }
}