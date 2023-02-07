namespace Project.Web.Requests.CreateRequests.User;

public class RegisterRequest
{
    public string UserName { get; set; }
    public string Password { get; set; }
    public string FullName { get; set; }
    public DateTime? Dob { get; set; }
    public DateTime? CreateDate { get; set; }
    public DateTime? EditDate { get; set; }
    public bool IsActive { get; set; }
    public string PhoneNumber { get; set; }
}
