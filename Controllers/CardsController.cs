using G_Basket_API.Localization;
using G_Basket_API.Models;
using G_Basket_API.ViewModels;
using G_Wallet_API.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using NodaTime;

namespace G_Basket_API.Controllers;

// Controller Implementation
[ApiController]
[Route("api/[controller]")]
public class CardController : ControllerBase
{
    private readonly GBasketDbContext _context;
    private readonly IStringLocalizer<SharedResource> _localizer;

    public CardController(GBasketDbContext context, IStringLocalizer<SharedResource> localizer)
    {
        _context = context;
        _localizer = localizer;
    }

    [HttpGet("CreateCard")]
    public async Task<IActionResult> CreateCard(long userId)
    {
        if (userId==0)
        {
            throw new Exception("userId is 0");
        }

        if (_context.Cards.Any(s=>s.UserId==userId && s.Status==(short)Enums.CardStatus.Active))
        {
            throw new Exception(_localizer["HasActiveCardException"]);
        }
        
        var card = new Card
        {
            UserId = userId,
            Status = 1, // Assuming 1 is "Active"
            RegDate = SystemClock.Instance.GetCurrentInstant()
        };

        _context.Cards.Add(card);
        await _context.SaveChangesAsync();

        return Ok(new GApiResponse<Card>
        {
            Data = card
        });
    }

    [HttpGet("GetCard")]
    public async Task<IActionResult> GetCard(long? cardId, long? userId, short? status)
    {
        var query = _context.Cards
            .AsNoTracking();

        if (cardId.HasValue)
        {
            query = query.Where(s => s.Id == cardId);
        }
        
        if (userId.HasValue)
        {
            query = query.Where(s => s.UserId == userId);
        }
        
        if (status.HasValue)
        {
            query = query.Where(s => s.Status == status);
        }

        var card = query.FirstOrDefault();
        if (card == null)
            return NotFound();

        return Ok(new GApiResponse<Card>
        {
            Data = card
        });
    }
    
    [HttpGet("GetCardList")]
    public async Task<IActionResult> GetCardList(long? cardId, long? userId, short? status)
    {
        var query = _context.Cards
            .AsNoTracking();

        if (cardId.HasValue)
        {
            query = query.Where(s => s.Id == cardId);
        }
        
        if (userId.HasValue)
        {
            query = query.Where(s => s.UserId == userId);
        }
        
        if (status.HasValue)
        {
            query = query.Where(s => s.Status == status);
        }

        var cards = query.ToList();
        if (cards == null)
            return NotFound();

        return Ok(new GApiResponse<List<Card>>
        {
            Data = cards
        });
    }

    [HttpGet("ChangeCardStatus")]
    public async Task<IActionResult> ChangeCardStatus(long cardId, short status)
    {
        var card = await _context.Cards.FindAsync(cardId);
        if (card == null)
            return NotFound();

        card.Status = status;
        card.ModifyDate = SystemClock.Instance.GetCurrentInstant();

        await _context.SaveChangesAsync();

        return Ok(new GApiResponse<Card>
        {
            Data = card
        });
    }

    [HttpPost("PaymentCard")]
    public async Task<IActionResult> PaymentCard(PaymentCardRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var card = await _context.Cards.FindAsync(request.CardId);
        if (card == null)
            return NotFound("CardId یافت نشد");
       
        var invoice =await _context.Invoices.FindAsync(request.PayInvoiceId);
        if (invoice == null)
            return NotFound("PayInvoiceId یافت نشد");

        card.Amount = request.Amount;
        card.AmountByVat = request.AmountByVat;
        card.WalletId = request.WalletId;
        card.PeymentInfo = request.PaymentInfo;
        card.PayInvoiceId = request.PayInvoiceId;
        card.DiliveryInfo = request.DeliveryInfo;
        card.PayDate = SystemClock.Instance.GetCurrentInstant();
        card.Status = (short)Enums.CardStatus.Paid;

        var cardTransaction = new PeymentTransaction
        {
            CardId = card.Id,
            PeymentInfo = request.PaymentInfo,
            Status = null,
        };
        _context.PeymentTransactions.Add(cardTransaction);
        
       await _context.SaveChangesAsync();

       return Ok(new GApiResponse<Card>
       {
           Data = card
       });
        
    }

    [HttpPost("AddCardDetail")]
    public async Task<IActionResult> AddCardDetail(long cardId, long productId, int entityId)
    {
        var cardDetail = new CardDetail
        {
            CardId = cardId,
            ProductId = productId,
            EntityId = entityId
        };

        _context.CardDetails.Add(cardDetail);
        await _context.SaveChangesAsync();

        return Ok(new GApiResponse<CardDetail>
        {
            Data = cardDetail
        });
        
    }

    [HttpPost("RemoveCardDetail")]
    public async Task<IActionResult> RemoveCardDetail(long id, long cardId)
    {
        var cardDetail = await _context.CardDetails
            .FirstOrDefaultAsync(cd => cd.Id == id && cd.CardId == cardId);

        if (cardDetail == null)
            return NotFound();

        _context.CardDetails.Remove(cardDetail);
        await _context.SaveChangesAsync();

        return Ok();
    }

    [HttpGet("GetCardDetails")]
    public async Task<IActionResult> GetCardDetails(long cardId)
    {
        var cardDetails = await _context.CardDetails
            .Where(cd => cd.CardId == cardId)
            .ToListAsync();

        return Ok(new GApiResponse<List<CardDetail>>
        {
            Data = cardDetails
        });
    }

    [HttpPost("CreateInvoice")]
    public async Task<IActionResult> CreateInvoice(CreateInvoiceRequest request)
    {
        var invoice = new Invoice
        {
            CardId = request.cardId,
            PeyEntityId = request.peyEntityId,
            InvoiceDetail = request.invoiceDetail
        };

        _context.Invoices.Add(invoice);
        await _context.SaveChangesAsync();

        
        return Ok(new GApiResponse<Invoice>
        {
            Data = invoice
        });
        
    }

    [HttpGet("GetInvoice")]
    public async Task<IActionResult> GetInvoice(long cardId, long peyEntityId)
    {
        var invoice = await _context.Invoices
            .FirstOrDefaultAsync(i => i.CardId == cardId && i.PeyEntityId == peyEntityId);

        if (invoice == null)
            return NotFound();

        return Ok(new GApiResponse<Invoice>
        {
            Data = invoice
        });
    }

}

// Integration Tests
