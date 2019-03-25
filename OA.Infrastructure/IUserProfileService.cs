
using OA.DataDomain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OA.Infrastructure
{
    public interface IUserProfileService
    {
        UserProfile GetUserProfile(long id);
    }
}
