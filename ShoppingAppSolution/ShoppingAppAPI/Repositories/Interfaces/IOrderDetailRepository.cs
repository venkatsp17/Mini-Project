using ShoppingAppAPI.Models;

namespace ShoppingAppAPI.Repositories.Interfaces
{
    public interface IOrderDetailRepository : IRepository<int, OrderDetail>
    {
        Task<IEnumerable<OrderDetail>> GetSellerOrderDetails(int SellerID);
    }
}
