namespace PinewoodDemo.Data.Models;

/// <summary>
/// Customer database entity.
/// </summary>
public class Customer
{
    public Guid CustomerId { get; set; }

    public string FirstName { get; set; } = "";

    public string LastName { get; set; } = "";

    public string? Email { get; set; }
}