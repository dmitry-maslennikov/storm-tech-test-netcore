using Todo.Services;

namespace Todo.Models.TodoItems
{
    public class UserSummaryViewmodel
    {
        public string UserName { get; }
        public string Email { get; }
        public string GravatarHash { get; }

        public UserSummaryViewmodel(string userName, string email)
        {
            UserName = userName;
            Email = email;
            GravatarHash = Gravatar.GetHash(email);
        }
    }
}