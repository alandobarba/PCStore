namespace P_CStore.Models
{
    public class UserModel
    {
        private string email;
        private string password;
        private string confirmPassword;
        private string role;
        public string Email
        {
            get { return email; }
            set { email = value; }
        }

        public string Password
        {
            get { return password; }
            set { password = value; }
        }
        public string ConfirmPassword
        {
            get { return confirmPassword; }
            set { confirmPassword = value; }
        }

        public string Role
        {
            get { return role; }
            set { role = value; }
        }
    }
}
