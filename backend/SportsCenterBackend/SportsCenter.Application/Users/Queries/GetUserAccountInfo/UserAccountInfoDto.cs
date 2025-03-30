namespace SportsCenter.Application.Users.Queries.GetUserAccountInfo;
public class UserAccountInfoDto
{
    public string Name { get; set; }
    public string LastName { get; set; }
    public string Address { get; set; }
    public DateOnly? DateOfBirth { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
    public string Role { get; set; }
    public decimal? Balance { get; set; }
    public int? ClassesDiscount { get; set; } 
    public int? ProductsDiscount { get; set; } 
}
