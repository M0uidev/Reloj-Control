using Reloj_Control.Models;

namespace Reloj_Control.Services
{
    public class SecurityService
    {
        UsersDAO usersDAO = new UsersDAO();

        public SecurityService()
        {
            
        }

        public bool IsValid(UserModel user)
        {
            // return true if found in the list
            return usersDAO.FindUserByNameAndPassword(user);
        }
    }
}
