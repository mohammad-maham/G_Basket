using System.Net.Http.Json;
using System.Text;
using G_Basket_API.Models;
using G_Wallet_API.Common;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using NUnit.Framework;

namespace G_Basket_API.Tests.IntegrationTests;

// NUnit Integration Tests
[TestFixture]
public class CardControllerTests
{
    private HttpClient _client;
    private WebApplicationFactory<Program> _factory;
    private HttpResponseMessage createResponse;
    private GApiResponse<Card>? createdCard;
    private GApiResponse<Invoice>? createdInvoice;

    [SetUp]
    public void SetUp()
    {
        _factory = new WebApplicationFactory<Program>();
        _client = _factory.CreateClient();
    }

    [TearDown]
    public void TearDown()
    {
        _client.Dispose();
        _factory.Dispose();
    }

    [Test,Order(10)]
    public async Task CreateCard_ShouldReturnCard()
    {
         createResponse = await _client.GetAsync("/api/Card/CreateCard?userId=1");
        var resp = await createResponse.Content.ReadAsStringAsync();
        Assert.That(createResponse.IsSuccessStatusCode, Is.True);

        createdCard = await createResponse.Content.ReadFromJsonAsync<GApiResponse<Card>>();
        Assert.That(createdCard, Is.Not.Null);
        Assert.That(createdCard.Data.UserId, Is.EqualTo(1));
    }

    [Test,Order(20)]
    public async Task GetCard_ShouldReturnCard()
    {
        // Create a card first


        // Get the card
        var getResponse = await _client.GetAsync($"/api/Card/GetCard?cardId={createdCard.Data.Id}&userId=1&status=1");
        Assert.That(getResponse.IsSuccessStatusCode, Is.True);

        var card = await getResponse.Content.ReadFromJsonAsync<GApiResponse<Card>>();
        Assert.That(card, Is.Not.Null);
        Assert.That(card.Data.Id, Is.EqualTo(createdCard.Data.Id));
    }

    [Test,Order(30)]
    public async Task ChangeCardStatus_ShouldUpdateStatus()
    {
        // Create a card first


        // Change the status
        var changeResponse = await _client.GetAsync($"/api/Card/ChangeCardStatus?CardId={createdCard.Data.Id}&Status=2");
        Assert.That(changeResponse.IsSuccessStatusCode, Is.True);

        var updatedCard = await changeResponse.Content.ReadFromJsonAsync<GApiResponse<Card>>();
        Assert.That(updatedCard, Is.Not.Null);
        Assert.That(updatedCard.Data.Status, Is.EqualTo(2));
    }
    
    
    [Test,Order(30)]
    public async Task CreateInvoice_ShouldReturnInvoice()
    {
        // Create a card first

        var payload = new
        {
            CardId = createdCard.Data.Id,
            PeyEntityId = 1,
            InvoiceDetail = @"{""test"":""Test Delivery""}"
        };

        var CreateInvoiceResponse = await _client.PostAsJsonAsync("/api/Card/CreateInvoice", payload);
        Assert.That(CreateInvoiceResponse.IsSuccessStatusCode, Is.True);

        createdInvoice = await CreateInvoiceResponse.Content.ReadFromJsonAsync<GApiResponse<Invoice>>();

        // Change the status
        var changeResponse = await _client.GetAsync($"/api/Card/ChangeCardStatus?CardId={createdCard.Data.Id}&Status=2");
        Assert.That(changeResponse.IsSuccessStatusCode, Is.True);

        var updatedCard = await changeResponse.Content.ReadFromJsonAsync<GApiResponse<Card>>();
        Assert.That(updatedCard, Is.Not.Null);
        Assert.That(updatedCard.Data.Status, Is.EqualTo(2));
    }

    [Test,Order(40)]
    public async Task PaymentCard_ShouldUpdatePaymentDetails()
    {
        // Create a card first


        // Make a payment
        var payload = new
        {
            CardId = createdCard.Data.Id,
            Amount = 100.0m,
            AmountByVat = 20.0m,
            WalletId = 1,
            PaymentInfo = @"{""test"":""Test Delivery""}",
            PayInvoiceId = createdInvoice.Data.Id,
            DeliveryInfo = @"{""test"":""Test Delivery""}"
        };

        var paymentResponse = await _client.PostAsJsonAsync("/api/Card/PaymentCard", payload);
        // var paymentResponse = await _client.PostAsJsonAsync("/api/Card/PaymentCard", new StringContent(
        //     JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json"));
        var str = await paymentResponse.Content.ReadAsStringAsync();
        Assert.That(paymentResponse.IsSuccessStatusCode, Is.True);

        var updatedCard = await paymentResponse.Content.ReadFromJsonAsync<GApiResponse<Card>>();
        Assert.That(updatedCard, Is.Not.Null);
        Assert.That(updatedCard.Data.Amount, Is.EqualTo(100.0m));
    }
}
