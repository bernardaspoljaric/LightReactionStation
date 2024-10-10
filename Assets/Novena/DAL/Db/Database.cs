using System;
using System.Data;
using Cysharp.Threading.Tasks;
using Mono.Data.Sqlite;
using UnityEngine;

namespace Novena.DAL.Db
{
  public static class Database
  {
    private static String ConnectionString => _GetConnectionString();

    /// <summary>
    /// Create new IDbConnection
    /// </summary>
    /// <returns>IDbConnection</returns>
    public static IDbConnection GetConnection()
    {
      return new SqliteConnection(ConnectionString);
    }

    private static string _GetConnectionString()
    {
      UniTask.SwitchToMainThread();
      return "URI=file:" + NCore.Instance.PersistentDataPath + "/AppDb.db";    
    }
  }
}