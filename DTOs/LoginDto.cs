namespace DatingApp.DTOs;

public sealed record LoginDto(string Name, string Password)
{
    public bool IsInvalid()
    {
        return String.IsNullOrEmpty(Password) || String.IsNullOrEmpty(Name);
    }
}
