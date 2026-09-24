namespace SrpLab;

public sealed class CheckoutBasket
{
    private readonly List<(string Sku, decimal Price, int Qty)> _lines = new();
    private string? _couponRaw;
    private bool _giftWrap;

    public void AddLine(string sku, decimal price, int qty)
    {
        if (qty <= 0)
            throw new ArgumentOutOfRangeException(nameof(qty));

        _lines.Add((sku, price, qty));
    }

    public void ApplyCouponText(string? couponText)
    {
        _couponRaw = couponText;
    }

    public void EnableGiftWrap()
    {
        _giftWrap = true;
    }

    public List<(string Sku, decimal Price, int Qty)> GetLines()
    {
        return _lines.ToList();
    }

    public string? GetCouponText()
    {
        return _couponRaw;
    }

    public bool HasGiftWrap()
    {
        return _giftWrap;
    }

    public int GetLineCount()
    {
        return _lines.Count;
    }
}

public sealed class CouponDiscountPolicy
{
    public decimal DiscountAmount(string? couponText, decimal subTotal)
    {
        if (string.IsNullOrWhiteSpace(couponText))
            return 0m;

        var t = couponText.Trim().ToUpperInvariant();

        if (t.StartsWith("SAVE") &&
            int.TryParse(t[4..], out var pct) &&
            pct is > 0 and <= 50)
        {
            return Math.Round(subTotal * pct / 100m, 2);
        }

        if (t.Contains("FREESHIP"))
            return 0m;

        if (t == "WELCOME10")
            return Math.Min(10m, subTotal);

        return 0m;
    }
}

public sealed class GiftWrapFeePolicy
{
    public decimal GetFee(bool giftWrap)
    {
        if (giftWrap)
            return 4.99m;

        return 0m;
    }
}

public sealed class BasketTotalCalculator
{
    private readonly CouponDiscountPolicy _couponPolicy = new();
    private readonly GiftWrapFeePolicy _giftWrapPolicy = new();

    public decimal SubTotal(CheckoutBasket basket)
    {
        return basket.GetLines()
            .Sum(line => line.Price * line.Qty);
    }

    public decimal DiscountAmount(CheckoutBasket basket)
    {
        return _couponPolicy.DiscountAmount(
            basket.GetCouponText(),
            SubTotal(basket)
        );
    }

    public decimal GrandTotal(CheckoutBasket basket)
    {
        var total = SubTotal(basket) - DiscountAmount(basket);

        total += _giftWrapPolicy.GetFee(
            basket.HasGiftWrap()
        );

        return Math.Max(0m, total);
    }
}

public sealed class GiftMessageCardBuilder
{
    public string GiftMessageCard(
        string fromName,
        CheckoutBasket basket,
        BasketTotalCalculator totalCalculator)
    {
        var items = string.Join(
            ", ",
            basket.GetLines().Select(line => line.Sku)
        );

        return $"Dear friend,\nA gift from {fromName} awaits ({items}).\nTotal surprise value: {totalCalculator.GrandTotal(basket):C}\n";
    }
}

public sealed class PaymentAuthorizerStub
{
    public string AuthorizePaymentStub(
        string cardLast4,
        CheckoutBasket basket,
        BasketTotalCalculator totalCalculator)
    {
        var payload =
            $"{totalCalculator.GrandTotal(basket):0.00}|{cardLast4}|{basket.GetLineCount()}";

        var hash = payload.GetHashCode();

        return $"AUTH-{Math.Abs(hash):X8}";
    }
}