using PinewoodDemo.Dto;

namespace PinewoodDemo.Pages;

/// <summary>
/// Index page.
/// </summary>
public sealed partial class Index
{
    private List<CustomerDto>? _customers;

    /// <summary>
    /// First initialize.
    /// </summary>
    /// <remarks>
    /// Populate the list of customers.
    /// </remarks>
    protected override async Task OnInitializedAsync()
    {
        _customers = await Api.GetCustomersAsync();
    }
}