using System.ComponentModel.DataAnnotations;

namespace PinewoodDemo.Dto;

/// <summary>
/// Customer.
/// </summary>
/// <remarks>
/// Data transfer object.
/// </remarks>
public class CustomerDto
{
    public Guid CustomerId { get; set; }

    [Required(ErrorMessage = "First Name is required.")]
    public string FirstName { get; set; } = "";

    [Required(ErrorMessage = "Last Name is required.")]
    public string LastName { get; set; } = "";

    public string? Email { get; set; }
}