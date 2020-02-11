using System.ComponentModel.DataAnnotations;

namespace Web.Models.Login
{
    public class RegisterModel
    {
        public string UserName { set; get; }
        [DataType(DataType.Password)]
        public string Password { set; get; }
        [DataType(DataType.Password)]
        public string ReEnterPassword{set;get;}
    }
}
