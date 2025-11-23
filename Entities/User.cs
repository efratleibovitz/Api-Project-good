using System.ComponentModel.DataAnnotations;

namespace Entities
{
    public class User
    {
        [Required(ErrorMessage = "חובה להזין אימייל")]
        [EmailAddress(ErrorMessage = "כתובת אימייל לא חוקית")]
        public string userName { get; set; } = string.Empty;



        public string fName { get; set; } = string.Empty;


        public string lName { get; set; } = string.Empty;

        public string passWord { get; set; } = string.Empty;
        public int id { get; set; }
    }
}
