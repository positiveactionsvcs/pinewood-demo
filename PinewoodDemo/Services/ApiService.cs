using System.Net;
using System.Text.Json;
using PinewoodDemo.Dto;

namespace PinewoodDemo.Services;

/// <summary>
/// API service.
/// </summary>
public class ApiService
{
    private readonly HttpClient _httpClient;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="httpClient">HTTP client.</param>
    public ApiService(HttpClient httpClient) => _httpClient = httpClient;

    /// <summary>
    /// Get all customers.
    /// </summary>
    /// <returns>The list of customers.</returns>
    public async Task<List<CustomerDto>?> GetCustomersAsync()
    {
        HttpResponseMessage response = await Get("customers");

        List<CustomerDto> list = (await response.Content.ReadFromJsonAsync<List<CustomerDto>>())!;

        return list;
    }

    /// <summary>
    /// Get a customer.
    /// </summary>
    /// <param name="customerId">Customer Id.</param>
    public async Task<CustomerDto?> GetCustomerAsync(Guid customerId)
    {
        HttpResponseMessage response = await Get($"customers/{customerId}");

        if (response.StatusCode == HttpStatusCode.NotFound)
            return null;

        CustomerDto? customer = await response.Content.ReadFromJsonAsync<CustomerDto>();

        return customer;
    }

    /// <summary>
    /// Insert a new customer.
    /// </summary>
    /// <param name="customer">The customer.</param>
    public async Task PostCustomer(CustomerDto customer) => await SendJson("customers", HttpMethod.Post, customer);

    /// <summary>
    /// Update a customer.
    /// </summary>
    /// <param name="customer">The customer.</param>
    public async Task PutCustomer(CustomerDto customer) => await SendJson("customers", HttpMethod.Put, customer);

    /// <summary>
    /// Delete a customer.
    /// </summary>
    /// <param name="customerId">Customer Id.</param>
    public async Task DeleteCustomer(Guid customerId) => await Delete($"customers/{customerId}");

    #region Helpers

    /// <summary>
    /// Get from a resource.
    /// </summary>
    /// <param name="requestUri"></param>
    /// <returns>The response.</returns>
    private async Task<HttpResponseMessage> Get(string requestUri)
    {
        HttpRequestMessage message = new(HttpMethod.Get, requestUri);

        return await _httpClient.SendAsync(message);
    }

    /// <summary>
    /// Send JSON to a resource.
    /// </summary>
    /// <param name="requestUri">The address.</param>
    /// <param name="httpMethod">HTTP method.</param>
    /// <param name="value">The value.</param>
    /// <returns>The response.</returns>
    private Task<HttpResponseMessage> SendJson(string requestUri, HttpMethod httpMethod, object value)
    {
        return _httpClient.SendAsync(new HttpRequestMessage(httpMethod, requestUri)
        {
            Content = JsonContent.Create(value, null, StandardSerializerOptions)
        });
    }

    /// <summary>
    /// Delete a resource.
    /// </summary>
    /// <param name="requestUri">The address.</param>
    private Task Delete(string requestUri) => _httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Delete, requestUri));

    /// <summary>
    /// JSON serialization options.
    /// </summary>
    /// <returns>JSON serialization options for the web.</returns>
    private static JsonSerializerOptions StandardSerializerOptions => new(JsonSerializerDefaults.Web);

    #endregion
}