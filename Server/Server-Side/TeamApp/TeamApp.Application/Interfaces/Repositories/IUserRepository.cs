using System.Collections.Generic;
using System.Threading.Tasks;
using TeamApp.Application.DTOs.User;

namespace TeamApp.Application.Interfaces.Repositories
{
    public interface IUserRepository
    {       
        Task<List<UserResponse>> SearchUser(string userId, string keyWord);
    }
}
