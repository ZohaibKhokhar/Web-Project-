using System.Collections.Generic;


namespace Domain.Entities
{
    public interface IFeedBackRepository
    {
        void Add(FeedBack feedback);
        IEnumerable<FeedBack> GetAll();
    }
}
