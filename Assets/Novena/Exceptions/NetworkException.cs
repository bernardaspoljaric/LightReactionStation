using System;

namespace Novena.Exceptions
{
  public class NetworkException : Exception
  {
    public override string Message { get; }

    public NetworkException(string message)
    {
      Message = message;
    }
  }
}