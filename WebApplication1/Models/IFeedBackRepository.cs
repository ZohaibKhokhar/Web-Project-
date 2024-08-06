using System.Collections.Generic;
using WebApplication1.Models;

namespace WebApplication1.Models
{
    public interface IFeedBackRepository
    {
        void Add(FeedBack feedback);
        IEnumerable<FeedBack> GetAll();
    }
}
