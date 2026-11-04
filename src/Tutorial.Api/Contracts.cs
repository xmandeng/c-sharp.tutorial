namespace Tutorial.Api;

// The JSON body of POST /orders. JSON {"customer": ...} maps to Customer.
public record LineRequest(string Sku, int Quantity);

public record OrderRequest(string Customer, List<LineRequest> Lines);
