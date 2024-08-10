using System.Collections.Generic;
using Domain.Entities;

namespace Domain.ServiceInterfaces
{
    public interface IFeedBackService
    {
        void Add(FeedBack feedback);
        IEnumerable<FeedBack> GetAll();
    }
}
