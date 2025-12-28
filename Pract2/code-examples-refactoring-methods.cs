// Replace Nested Conditional with Guard Clauses (до рефлакторингу)
public void ProcessOrder(Order order)
{
    if (order != null)
    {
        if (!order.IsCancelled)
        {
            if (order.Items.Any())
            {
                if (_stockService.HasEnoughStock(order))
                {
                    if (_paymentService.Charge(order))
                    {
                        order.Status = OrderStatus.Completed;
                        _notificationService.NotifyCustomer(order);
                    }
                    else
                    {
                        order.Status = OrderStatus.PaymentFailed;
                    }
                }
                else
                {
                    order.Status = OrderStatus.OutOfStock;
                }
            }
        }
    }
}
// після рефлакторингу
public void ProcessOrder(Order order)
{
    if (order == null)
        return;

    if (order.IsCancelled)
    {
        order.Status = OrderStatus.Cancelled;
        return;
    }

    if (!order.Items.Any())
    {
        order.Status = OrderStatus.Empty;
        return;
    }

    if (!_stockService.HasEnoughStock(order))
    {
        order.Status = OrderStatus.OutOfStock;
        return;
    }

    if (!_paymentService.Charge(order))
    {
        order.Status = OrderStatus.PaymentFailed;
        return;
    }

    order.Status = OrderStatus.Completed;
    _notificationService.NotifyCustomer(order);
}
// -----------------------------------------------------
// Decompose Conditional (до рефлакторингу)
public decimal CalculateDiscount(Order order, Customer customer)
{
    decimal discount = 0m;

    if (customer.IsVip &&
        order.TotalAmount > 1000 &&
        order.PromoCode == "NEWYEAR" &&
        DateTime.Now >= _promoPeriod.Start &&
        DateTime.Now <= _promoPeriod.End)
    {
        discount = order.TotalAmount * 0.15m;
    }

    return discount;
}
// після рефлакторингу
public decimal CalculateDiscount(Order order, Customer customer)
{
    if (!IsCustomerEligible(customer))
        return 0m;

    if (!IsOrderEligible(order))
        return 0m;

    if (!IsPromoActive(order))
        return 0m;

    return order.TotalAmount * 0.15m;
}

private bool IsCustomerEligible(Customer customer)
{
    return customer.IsVip;
}

private bool IsOrderEligible(Order order)
{
    return order.TotalAmount > 1000;
}

private bool IsPromoActive(Order order)
{
    bool hasPromoCode = order.PromoCode == "NEWYEAR";
    bool inPeriod = DateTime.Now >= _promoPeriod.Start &&
                    DateTime.Now <= _promoPeriod.End;
    return hasPromoCode && inPeriod;
}
// -----------------------------------------------------
// Replace Temp with Query (до рефлакторингу)
public decimal GetFinalAmount(Order order)
{
    decimal total = 0m;

    foreach (var item in order.Items)
    {
        total += item.Price * item.Quantity;
    }

    if (order.Delivery != null)
    {
        total += order.Delivery.Cost;
    }

    if (order.DiscountPercent > 0)
    {
        total -= total * order.DiscountPercent / 100m;
    }

    return total;
}
// після рефлакторингу
public decimal GetFinalAmount(Order order)
{
    decimal total = CalculateOrderBaseTotal(order);

    if (order.DiscountPercent > 0)
    {
        total -= total * order.DiscountPercent / 100m;
    }

    return total;
}

private decimal CalculateOrderBaseTotal(Order order)
{
    decimal total = 0m;

    foreach (var item in order.Items)
    {
        total += item.Price * item.Quantity;
    }

    if (order.Delivery != null)
    {
        total += order.Delivery.Cost;
    }

    return total;
}
// -----------------------------------------------------

