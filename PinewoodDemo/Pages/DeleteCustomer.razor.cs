using Microsoft.AspNetCore.Components;
using PinewoodDemo.Dto;

namespace PinewoodDemo.Pages;

/// <summary>
/// Delete a customer.
/// </summary>
public sealed partial class DeleteCustomer
{
    /// <summary>
    /// Customer Id (supplied from URL).
    /// </summary>
    [Parameter]
    public Guid CustomerId { get; set; }

    // Stores what parameters are currently populated.
    private Guid? _customerId;

    // Data sources.
    private CustomerDto? _customer;

    // Error message
    private string _errorMessage = "";

    #region Initialize

    /// <summary>
    /// Responding to when parameters are set.
    /// </summary>
    /// <remarks>
    /// Called by Blazor on first initialize with initial parameters, and also when the parent
    /// re-renders and receives any changed parameters.  Because parameters might influence what
    /// data appears in this control, do all parameter change detection and data population here.
    /// </remarks>
    protected override async Task OnParametersSetAsync()
    {
        if (CustomerId != _customerId)
        {
            _customerId = CustomerId;
            _customer = await Api.GetCustomerAsync(_customerId.Value);
        }
    }

    #endregion

    #region Delete/Cancel Buttons

    /// <summary>
    /// Delete the customer.
    /// </summary>
    private async Task DeleteButton()
    {
        if (_customerId != null)
        {
            try
            {
                await Api.DeleteCustomer(_customerId.Value);

                NavigationManager.NavigateTo("./");
            }
            catch (Exception ex)
            {
                // Show error.
                _errorMessage = ex.Message;
            }
        }
    }

    /// <summary>
    /// Cancel and do not save any changes.
    /// </summary>
    private void CancelButton() => NavigationManager.NavigateTo("./");

    #endregion
}