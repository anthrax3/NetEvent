# NetEvent

NetEvent is a small scatter gather library.
It allows modules/components of an application to respond to requests in a decoupled manner.


```csharp
//inside module 1
ScatterGather<DiscountRequest, DiscountResponse>.Subscribe(msg => new DiscountResponse(0.5));

//insode module 2
ScatterGather<DiscountRequest, DiscountResponse>.SubscribeAsync(async msg => {
    if (msg.ProductId == "banana")
    {
        await Task.Delay(1234); //simulate work
        return new DiscountResponse(discount: 5.0);
    }
    return DiscountResponse(0.0);
});

//inside module3
var discountRequest = new DiscountRequest(productId: "banana");
var allDiscounts = await ScatterGather<DiscountRequest, DiscountResponse>.Request(discountRequest, TimeSpan.FromSeconds(2));

```