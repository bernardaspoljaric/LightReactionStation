using System;

namespace Novena.Admin.Pages.Login
{
  public class LoginController
  {
    private const String PASSWORD = "novena";

    public bool IsPasswordValid(String password)
    {
      password = password.ToLower();
      return password == PASSWORD;
    }
    
  }
}
