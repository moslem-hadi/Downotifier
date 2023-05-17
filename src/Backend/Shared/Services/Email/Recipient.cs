namespace Shared.Services.Email;

public class Recipient
{
    public Recipient(string email, string? name="")
    {
        Email = email;
        Name = name;
    }
    public string Email { get; set; }
    public string? Name { get; set; }
}
