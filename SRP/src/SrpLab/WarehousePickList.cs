namespace SrpLab;

public sealed class WarehousePickList
{
    private readonly List<(string Sku, string Aisle, int Bin, int QtyNeeded, int QtyOnHand)> _lines = new();

    public IReadOnlyList<(string Sku, string Aisle, int Bin, int QtyNeeded, int QtyOnHand)> Lines()
    {
        return _lines.ToList();
    }

    public void AddNeed(string sku, string aisle, int bin, int qtyNeeded, int qtyOnHand)
    {
        _lines.Add((sku, aisle, bin, qtyNeeded, qtyOnHand));
    }
}

public sealed class StockAllocator
{
    public IReadOnlyList<(string Sku, int Allocated)> Allocate(WarehousePickList warehouse)
    {
        var result = new List<(string, int)>();

        foreach (var line in warehouse.Lines())
        {
            var alloc = Math.Min(line.QtyNeeded, line.QtyOnHand);
            result.Add((line.Sku, alloc));
        }

        return result;
    }
}

public sealed class WalkingOrderPlanner
{
    public IReadOnlyList<(string Aisle, int Bin, string Sku, int Qty)> WalkingOrder(WarehousePickList warehouse, StockAllocator stock)
    {
        var lines = warehouse.Lines();
        var allocated = stock.Allocate(warehouse);

        return lines
            .Select((l, i) => (l.Aisle, l.Bin, l.Sku, allocated[i].Allocated))
            .Where(x => x.Item4 > 0)
            .OrderBy(x => x.Aisle)
            .ThenBy(x => x.Bin)
            .ToList();
    }
}

public sealed class PickerScriptBuilder
{
    public string PickerScript(WalkingOrderPlanner walking, WarehousePickList warehouse, StockAllocator stock)
    {
        var steps = walking.WalkingOrder(warehouse, stock)
            .Select((s, i) => $"{i + 1}. Go aisle {s.Aisle} bin {s.Bin}: pick {s.Qty} × {s.Sku}");

        var shortfalls = stock.Allocate(warehouse).Where(a =>
        {
            var need = warehouse.Lines().First(l => l.Sku == a.Sku).QtyNeeded;
            return a.Allocated < need;
        });

        var warn = shortfalls.Any()
            ? "SHORTAGES: " + string.Join(", ", shortfalls.Select(s => s.Sku))
            : "SHORTAGES: none";

        return string.Join('\n', steps) + "\n" + warn;
    }
}

public sealed class WmsXmlExporter
{
    public string WmsXmlBatch(string batchId, StockAllocator stock, WarehousePickList warehouse)
    {
        var parts = stock.Allocate(warehouse)
            .Select(a => $"<line sku=\"{a.Sku}\" qty=\"{a.Allocated}\" />");

        return $"<batch id=\"{batchId}\">{string.Join("", parts)}</batch>";
    }
}