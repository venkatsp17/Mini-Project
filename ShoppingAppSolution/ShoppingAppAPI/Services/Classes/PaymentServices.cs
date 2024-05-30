using ShoppingAppAPI.Exceptions;
using ShoppingAppAPI.Mappers;
using ShoppingAppAPI.Models;
using ShoppingAppAPI.Models.DTO_s.Payment_DTO_s;
using ShoppingAppAPI.Repositories.Interfaces;
using ShoppingAppAPI.Services.Interfaces;

namespace ShoppingAppAPI.Services.Classes
{
    public class PaymentServices : IPaymentServices
    {
        private readonly IRepository<int, Payment> _paymentRepository;
        private readonly IRepository<int, Order> _orderRepository;
        public PaymentServices(IRepository<int, Payment> paymentRepository, IRepository<int, Order> orderRepository) { 
            _paymentRepository = paymentRepository;
            _orderRepository = orderRepository;
        }

        public async Task<PaymentReturnDTO> MakePayment(PaymentGetDTO paymentGetDTO)
        {
            try
            {
                var order = await _orderRepository.Get(paymentGetDTO.OrderID);
                if(order == null || order.Status == Enums.OrderStatus.Failed || order.Status == Enums.OrderStatus.Canceled || order.Status == Enums.OrderStatus.Refunded)
                {
                    throw new UnableToProcessOrder("Unable to Process Order at this moment!");
                }
                Payment payment = new Payment()
                {
                    OrderID = paymentGetDTO.OrderID,
                    Payment_Method = paymentGetDTO.Payment_Method,
                    Amount = paymentGetDTO.Amount,
                    Transaction_Date = DateTime.Now,
                    Payment_Status = Enums.PaymentStatus.Authorized,
                };

                var newPayment = await _paymentRepository.Add(payment);
                if (newPayment == null) {
                    throw new UnableToAddItemException("Unable to add payment at this moment!");
                }
                order.Success_PaymentID = newPayment.PaymentID;
                var updatedOrder = await _orderRepository.Update(order);

                return PaymentMapper.MapToPaymentReturnDTO(newPayment);
            }
            catch (Exception ex)
            {
                throw new UnableToAddItemException("Unable to add payment at this moment: " + ex.Message);
            }
        }
    }
}
