using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PinewoodDemo.Data;
using PinewoodDemo.Data.Models;
using PinewoodDemo.Dto;

namespace PinewoodDemo.Api.Controllers;

/// <summary>
/// Customers controller.
/// </summary>
[ApiController]
[Route("customers")]
public class CustomerController : ControllerBase
{
    private readonly PinewoodContext _context;

    public CustomerController(IServiceProvider serviceProvider)
    {
        _context = serviceProvider.GetService<PinewoodContext>()!;
    }

    /// <summary>
    /// Get all customers.
    /// </summary>
    /// <returns>The list of customers.</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CustomerDto>>> Get()
    {
        List<Customer> customerList = await _context.Customers
            .OrderBy(c => c.FirstName)
            .ThenBy(c => c.LastName)
            .ToListAsync();

        // Return a DTO and not the EF entity.
        List<CustomerDto> response = customerList.Select(c => new CustomerDto
        {
            CustomerId = c.CustomerId,
            FirstName = c.FirstName,
            LastName = c.LastName,
            Email = c.Email
        }).ToList();

        return response;
    }

    /// <summary>
    /// Get a customer.
    /// </summary>
    /// <param name="customerId">The Id of the customer.</param>
    /// <returns>The customer.</returns>
    [HttpGet("{customerId:guid}")]
    public async Task<ActionResult<object>> Get(Guid customerId)
    {
        Customer? customer = await _context.Customers.AsNoTracking()
            .SingleOrDefaultAsync(c => c.CustomerId == customerId);

        // Return 404 when the resource in the URL is not found.
        if (customer == null)
            return NotFound();

        // Return a DTO and not the EF entity.
        CustomerDto response = new()
        {
            CustomerId = customer.CustomerId,
            FirstName = customer.FirstName,
            LastName = customer.LastName,
            Email = customer.Email
        };

        return response;
    }

    /// <summary>
    /// Insert a new customer.
    /// </summary>
    /// <param name="customerDto">The customer.</param>
    /// <returns>The Id of the new customer.</returns>
    [HttpPost]
    public async Task<ActionResult> Post(CustomerDto customerDto)
    {
        Customer customer = new()
        {
            CustomerId = customerDto.CustomerId,
            FirstName = customerDto.FirstName,
            LastName = customerDto.LastName,
            Email = customerDto.Email
        };

        await _context.Customers.AddAsync(customer);

        int recordsAffected = await _context.SaveChangesAsync();

        return recordsAffected > 0
            ? Created(Url.Action("Get", "Customer", new { customerId = customer.CustomerId })!, "")
            : throw new ApplicationException("Customer was not created.");
    }

    /// <summary>
    /// Update a customer.
    /// </summary>
    /// <param name="customerDto">The customer.</param>
    [HttpPut]
    public async Task<ActionResult> Put(CustomerDto customerDto)
    {
        // Get the existing entity.
        Customer? existingCustomer = await _context.Customers.SingleOrDefaultAsync(c => c.CustomerId == customerDto.CustomerId);

        if (existingCustomer == null)
            return NotFound();

        // Update all values from the DTO to the EF entity.
        existingCustomer.CustomerId = customerDto.CustomerId;
        existingCustomer.FirstName = customerDto.FirstName;
        existingCustomer.LastName = customerDto.LastName;
        existingCustomer.Email = customerDto.Email;

        int recordsAffected = await _context.SaveChangesAsync();

        return recordsAffected > 0
            ? Ok()
            : throw new ApplicationException("Customer was not updated.");
    }

    /// <summary>
    /// Delete a customer.
    /// </summary>
    /// <param name="customerId">The Id of the customer.</param>
    [HttpDelete("{customerId:guid}")]
    public async Task<ActionResult> Delete(Guid customerId)
    {
        Customer? customer = await _context.Customers
            .SingleOrDefaultAsync(c => c.CustomerId == customerId);

        // Return 404 when the resource in the URL is not found.
        if (customer == null)
            return NotFound();

        _context.Customers.Remove(customer);

        int recordsAffected = await _context.SaveChangesAsync();

        return recordsAffected > 0
            ? Ok()
            : throw new ApplicationException("Customer was not deleted.");
    }
}