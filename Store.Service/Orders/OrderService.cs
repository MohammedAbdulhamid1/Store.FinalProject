using Store.Core.Entites;
using Store.Core.Entites.Orders;
using Store.Core.Repositories.Contract;
using Store.Core.Services.Contract;
using Store.Repository.Specification.Orders;
using Store.Repository.UnirOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Service.Orders
{
    public class OrderService : IOrderService
    {
        private readonly IUnitofwork unitofwork;
        private readonly IBasketRepository basketRepository;
        

        public OrderService(IUnitofwork unitofwork,IBasketRepository basketRepository)
        {
            this.unitofwork = unitofwork;
            this.basketRepository = basketRepository;
        }
        public async Task<Order> CreateOrderAsync(string buyerEmail, string basketId, int deliveryMethodId, Address shippingAddress)
        {
            var basket = await basketRepository.GetBasketAsync(basketId);
            if (basket is null) return null;

            var orderItems = new List<OrderItem>();

            if (basket.Items.Count() > 0)
            {
                foreach (var item in basket.Items)
                {
                    var product = await unitofwork.Repositories<Product, int>().GetByIdAsync(item.Id);

                    var ProductOrderedItem = new ProductItemOrder(product.Id, product.Name, product.PictureUrl);

                    var orderItem = new OrderItem(ProductOrderedItem, product.Price, item.Quantity);

                    orderItems.Add(orderItem);
                }
            }

            var deliveryMethod = await unitofwork.Repositories<DeliveryMethod, int>()
                                                  .GetByIdAsync(deliveryMethodId);

            var subTotal = orderItems.Sum(I => I.Price * I.Quantity);

            var order = new Order(buyerEmail, shippingAddress, deliveryMethod, orderItems, subTotal,"");

            await unitofwork.Repositories<Order, int>().AddAsync(order);

            var result = await unitofwork.completeAsync();

            if (result <= 0) return null;

            return order;
        }

        public Task<Order?> GetOrderByIdForSpecificUserAsync(string buyerEmail, int orderId)
        {
            var spec = new OrderSpecification(buyerEmail, orderId);
            var order = unitofwork.Repositories<Order, int>().GetWithSpecAsync(spec);
            return order;
        }

        public Task<IEnumerable<Order>?> GetOrdersForSpecificUserAsync(string buyerEmail)
        {
            var spec = new OrderSpecification(buyerEmail);
            var order = unitofwork.Repositories<Order, int>().GetAllWithSpecAsync(spec);
            return order;
        }
    }
}
