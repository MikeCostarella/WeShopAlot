using WeShopAlot.Data.Models;
using WeShopAlot.Infrastructure.Services.Interfaces;
using WeShopAlot.WebAPI.Errors;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stripe;

namespace WeShopAlot.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : BaseApiController
    {
        #region Member Variables

        private readonly string _whSecret;
        private readonly IPaymentService _paymentService;
        private readonly ILogger<PaymentController> _logger;

        #endregion Member Variables

        #region Constructors

        public PaymentController(IPaymentService paymentService, ILogger<PaymentController> logger, IConfiguration config)
        {
            _logger = logger;
            _paymentService = paymentService;
            _whSecret = config.GetSection("StripeSettings:WhSecret").Value;
        }

        #endregion Constructors

        #region Public Actions

        // No antiforgery attribute: this API authenticates with a bearer token, not a cookie, so CSRF
        // does not apply, and AddControllers() never registers the antiforgery filter it needs (it was a 500).
        [Authorize]
        [HttpPost("{basketId}")]
        public async Task<ActionResult<CustomerBasket>> CreateOrUpdatePaymentIntent(string basketId)
        {
            var basket = await _paymentService.CreateOrUpdatePaymentIntent(basketId);
            if (basket == null) return BadRequest(new ApiResponse(400, "Problem with your basket"));
            return basket;
        }

        // Stripe calls this endpoint; the Stripe-Signature header, checked against the webhook secret, is what proves it.
        [HttpPost("webhook")]
        public async Task<ActionResult> StripeWebhook()
        {
            var json = await new StreamReader(Request.Body).ReadToEndAsync();
            Event stripeEvent;
            try
            {
                stripeEvent = EventUtility.ConstructEvent(json, Request.Headers["Stripe-Signature"], _whSecret);
            }
            catch (StripeException ex)
            {
                _logger.LogWarning("Rejected webhook call: {Reason}", ex.Message);
                return BadRequest(new ApiResponse(400, "Invalid Stripe signature"));
            }
            PaymentIntent intent;
            Order order;
            switch (stripeEvent.Type)
            {
                case "payment_intent.succeeded":
                    intent = (PaymentIntent)stripeEvent.Data.Object;
                    _logger.LogInformation("Payment succeeded: {PaymentIntentId}", intent.Id);
                    order = await _paymentService.UpdateOrderPaymentSucceeded(intent.Id);
                    _logger.LogInformation("Order updated to payment received: {OrderId}", order?.Id);
                    break;
                case "payment_intent.payment_failed":
                    intent = (PaymentIntent)stripeEvent.Data.Object;
                    _logger.LogInformation("Payment failed: {PaymentIntentId}", intent.Id);
                    order = await _paymentService.UpdateOrderPaymentFailed(intent.Id);
                    _logger.LogInformation("Order updated to payment failed: {OrderId}", order?.Id);
                    break;
            }
            return new EmptyResult();
        }

        #endregion Public Actions
    }
}
