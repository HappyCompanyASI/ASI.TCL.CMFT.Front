namespace ASI.TCL.CMFT.WPF.Web;

public class AuthService : IAuthService
{
    private string _token;

    public string GetToken()
    {
        return _token;
    }

    public void SetToken(string token)
    {
        _token = token;
    }

    public void ClearToken()
    {
        _token = null;
    }
}