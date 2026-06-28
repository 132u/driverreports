namespace DriverReports.Application.Validators;

public static class EmailValidator
{
    private static readonly HashSet<string> ForbiddenDomains = new()
    {
        "gmail.com",
        "googlemail.com",
        "outlook.com",
        "hotmail.com",
        "live.com",
        "msn.com",
        "yahoo.com",
        "icloud.com",
        "me.com",
        "mac.com",
        "proton.me",
        "protonmail.com",
        "tuta.com",
        "tutanota.com",
        "aol.com",
        "mail.com",
        "gmx.com",
        "gmx.de",
        "gmx.net",
        "zoho.com",
        "fastmail.com",
        "hey.com",
        "runbox.com",
        "posteo.de",
        "mailfence.com",
        "startmail.com",
        "migadu.com",
        "hushmail.com",
    };

    public static bool IsRussianEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return false;

        var index = email.LastIndexOf('@');

        if (index < 0)
            return false;

        var domain = email[(index + 1)..].ToLowerInvariant();

        return !ForbiddenDomains.Contains(domain);
    }
}