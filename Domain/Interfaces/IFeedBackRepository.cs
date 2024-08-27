using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public interface IFeedBackRepository
    {
        Task Add(FeedBack feedback);
        Task<IEnumerable<FeedBack>> GetAll();
    }
}
