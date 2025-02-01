const axios = require("axios");

const BASE_URL = "https://localhost:5031"; // Replace with your API base URL

describe("CardController API Tests", () => {
  let testCardId;
  const testUserId = 12345; // Replace with a valid user ID

  test("CreateCard: should create a new card for the user", async () => {
    const response = await axios.get(`${BASE_URL}/CreateCard`, {
      params: { userId: testUserId },
    });

    expect(response.status).toBe(200);
    expect(response.data).toHaveProperty("data");
    expect(response.data.data).toHaveProperty("userId", testUserId);

    // Store card ID for later tests
    testCardId = response.data.data.id;
  });

  test("GetCard: should retrieve the created card by ID", async () => {
    const response = await axios.get(`${BASE_URL}/GetCard`, {
      params: { cardId: testCardId },
    });

    expect(response.status).toBe(200);
    expect(response.data).toHaveProperty("data");
    expect(response.data.data).toHaveProperty("id", testCardId);
  });

  test("GetCardList: should retrieve a list of cards for the user", async () => {
    const response = await axios.get(`${BASE_URL}/GetCardList`, {
      params: { userId: testUserId },
    });

    expect(response.status).toBe(200);
    expect(response.data).toHaveProperty("data");
    expect(Array.isArray(response.data.data)).toBe(true);
  });

  test("ChangeCardStatus: should update the status of the card", async () => {
    const newStatus = 2; // Example status
    const response = await axios.get(`${BASE_URL}/ChangeCardStatus`, {
      params: { cardId: testCardId, status: newStatus },
    });

    expect(response.status).toBe(200);
    expect(response.data).toHaveProperty("data");
    expect(response.data.data).toHaveProperty("status", newStatus);
  });

  test("AddCardDetail: should add a new card detail", async () => {
    const productId = 67890; // Replace with a valid product ID
    const entityId = 1; // Example entity ID
    const response = await axios.post(`${BASE_URL}/AddCardDetail`, {
      cardId: testCardId,
      productId,
      entityId,
    });

    expect(response.status).toBe(200);
    expect(response.data).toHaveProperty("data");
    expect(response.data.data).toHaveProperty("cardId", testCardId);
    expect(response.data.data).toHaveProperty("productId", productId);
  });

  test("RemoveCardDetail: should remove a card detail", async () => {
    const cardDetailId = 1; // Replace with a valid card detail ID
    const response = await axios.delete(`${BASE_URL}/RemoveCardDetail`, {
      params: { id: cardDetailId, cardId: testCardId },
    });

    expect(response.status).toBe(200);
  });

  test("GetCardDetails: should retrieve all details of the card", async () => {
    const response = await axios.get(`${BASE_URL}/GetCardDetails`, {
      params: { cardId: testCardId },
    });

    expect(response.status).toBe(200);
    expect(response.data).toHaveProperty("data");
    expect(Array.isArray(response.data.data)).toBe(true);
  });

  test("CreateInvoice: should create an invoice for the card", async () => {
    const requestBody = {
      cardId: testCardId,
      peyEntityId: 1001, // Example entity ID
      invoiceDetail: "Test invoice details",
    };

    const response = await axios.post(`${BASE_URL}/CreateInvoice`, requestBody);

    expect(response.status).toBe(200);
    expect(response.data).toHaveProperty("data");
    expect(response.data.data).toHaveProperty("cardId", testCardId);
  });

  test("GetInvoice: should retrieve an invoice for the card", async () => {
    const peyEntityId = 1001; // Example entity ID
    const response = await axios.get(`${BASE_URL}/GetInvoice`, {
      params: { cardId: testCardId, peyEntityId },
    });

    expect(response.status).toBe(200);
    expect(response.data).toHaveProperty("data");
    expect(response.data.data).toHaveProperty("cardId", testCardId);
  });
});
