
using OA.DataDomain;
using OA.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OA.Infrastructure
{
    public class UserProfileService: IUserProfileService
    {
        private IRepository<UserProfile> userProfileRepository;

        public UserProfileService(IRepository<UserProfile> userProfileRepository)
        {           
            this.userProfileRepository = userProfileRepository;
        }

        public UserProfile GetUserProfile(long id)
        {
            return userProfileRepository.Get(id);
        }
    }
}
