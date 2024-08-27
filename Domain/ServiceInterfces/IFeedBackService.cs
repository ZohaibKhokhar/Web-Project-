using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.ServiceInterfaces
{
    public interface IFeedBackService
    {
        Task Add(FeedBack feedback);
        Task<IEnumerable<FeedBack>> GetAll();
    }
}
