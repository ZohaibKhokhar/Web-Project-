using System.Collections.Generic;
using System.Threading.Tasks;
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

        public async Task Add(FeedBack feedback)
        {
            await _feedBackRepository.Add(feedback);
        }

        public async Task<IEnumerable<FeedBack>> GetAll()
        {
            return await _feedBackRepository.GetAll();
        }
    }
}
