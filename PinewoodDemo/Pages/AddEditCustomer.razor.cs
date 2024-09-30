using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using PinewoodDemo.Dto;

namespace PinewoodDemo.Pages;

/// <summary>
/// Add/edit a customer.
/// </summary>
public sealed partial class AddEditCustomer
{
    /// <summary>
    /// Customer Id (supplied from URL).
    /// </summary>
    [Parameter]
    public Guid CustomerId { get; set; }

    // Stores what parameters are currently populated.
    private Guid? _customerId;

    // Data sources for form.
    private CustomerDto? _customer;
    private EditContext? _editContext;

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

            await Populate();

            _editContext = new EditContext(_customer!);
        }
    }

    #endregion

    #region Save/Cancel Buttons

    /// <summary>
    /// Save any changes.
    /// </summary>
    private async Task SaveButton()
    {
        if (_editContext != null && !_editContext.Validate())
            return;

        if (await Save())
            NavigationManager.NavigateTo("./");
    }

    /// <summary>
    /// Cancel and do not save any changes.
    /// </summary>
    private void CancelButton() => NavigationManager.NavigateTo("./");

    #endregion

    #region Populate/Save/Helpers

    /// <summary>
    /// Populate the page.
    /// </summary>
    private async Task Populate()
    {
        try
        {
            if (NavigationManager.Uri.EndsWith("/add"))
            {
                _customer = new CustomerDto();
            }
            else
            {
                _customer = await Api.GetCustomerAsync(CustomerId);
            }
        }
        catch (Exception ex)
        {
            // Show error.
            _errorMessage = ex.Message;
        }
    }

    /// <summary>
    /// Save.
    /// </summary>
    /// <returns>Was save successful?</returns>
    private async Task<bool> Save()
    {
        if (_customer == null)
            return false;

        try
        {
            if (NavigationManager.Uri.EndsWith("/add"))
            {
                await Api.PostCustomer(_customer);
            }
            else
            {
                await Api.PutCustomer(_customer);
            }

            return true;
        }
        catch (Exception ex)
        {
            // Show error.
            _errorMessage = ex.Message;
        }

        return true;
    }

    #endregion
}