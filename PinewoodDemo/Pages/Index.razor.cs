using PinewoodDemo.Dto;

namespace PinewoodDemo.Pages;

public partial class Index
{
    private List<CustomerDto>? _customers;

    protected override async Task OnInitializedAsync()
    {
        _customers = await Api.GetCustomersAsync();
    }
}