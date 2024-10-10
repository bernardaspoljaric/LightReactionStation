using System;

namespace Assets.Novena.Cache
{
  public class CacheException : Exception
  {
    public CacheException(string message) : base(message) {}
  }
}
