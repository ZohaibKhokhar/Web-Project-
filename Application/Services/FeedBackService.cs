using System.Collections.Generic;
using Domain.Entities;
using Domain.ServiceInterfaces;

namespace Application.Services
{
    public class FeedBackService : IFeedBackService
    {
        private readonly IFeedBackRepository _feedBackRepository;

        public FeedBackService(IFeedBackRepository feedBackRepository)
        {
            _feedBackRepository = feedBackRepository;
        }

        public void Add(FeedBack feedback)
        {
            _feedBackRepository.Add(feedback);
        }

        public IEnumerable<FeedBack> GetAll()
        {
            return _feedBackRepository.GetAll();
        }
    }
}
