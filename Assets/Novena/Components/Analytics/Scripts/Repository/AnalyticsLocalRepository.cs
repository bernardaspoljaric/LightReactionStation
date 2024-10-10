using Mono.Data.Sqlite;
using Novena.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class AnalyticsLocalRepository
{
  #region Private properties

  private string _persistentDataPath = String.Empty;
  private IDbConnection _connection { get; set; }

  #endregion

  public AnalyticsLocalRepository()
  {
    Initialize();
  }

  private void Initialize()
  {
    _persistentDataPath = Application.persistentDataPath;
    CreateConnection();
    CreateDatabase();
  }
  private string GetConnectionString()
  {
    return "URI=file:" + _persistentDataPath + "/AnalyticsDB.db";
  }

  /// <summary>
  /// Create and open connection.
  /// </summary>
  private void CreateConnection()
  {
    _connection = new SqliteConnection(GetConnectionString());
    _connection.Open();
  }

  //Create method to create database tabels
  private void CreateDatabase()
  {
    string sql = string.Empty;

    //Table for Analytics
    sql += "CREATE TABLE IF NOT EXISTS Events (Id INTEGER, Type INTEGER,GuideId INTEGER, ThemeId INTEGER, MediaId INTEGER, TimeStamp TEXT, Value INTEGER, PRIMARY KEY(Id AUTOINCREMENT));";

    IDbCommand dbcmd;
    dbcmd = _connection.CreateCommand();
    dbcmd.CommandText = sql;
    dbcmd.ExecuteReader();
  }

  public void Insert(AnalyticEvent e)
  {
    IDbCommand dbcmd;
    dbcmd = _connection.CreateCommand();
    dbcmd.CommandText = "INSERT INTO Events (Type, GuideId, ThemeId, MediaId, TimeStamp, Value) VALUES (@Type, @GuideId, @ThemeId, @MediaId, @TimeStamp, @Value)";
    dbcmd.Parameters.Add(new SqliteParameter("@Type", (int)e.Type));
    dbcmd.Parameters.Add(new SqliteParameter("@GuideId", e.GuideId));
    dbcmd.Parameters.Add(new SqliteParameter("@ThemeId", e.ThemeId));
    dbcmd.Parameters.Add(new SqliteParameter("@MediaId", e.MediaType != null ? (int)e.MediaType : null));
    dbcmd.Parameters.Add(new SqliteParameter("@TimeStamp", e.Time.ToString("yyyy-MM-ddTHH:mm:ss")));
    dbcmd.Parameters.Add(new SqliteParameter("@Value", e.Value));
    dbcmd.ExecuteNonQuery();

#if UNITY_EDITOR
    Debug.Log("AnalyticsLocalRepository => Insert: " + e.Type.ToString() + " inserted");
#endif
  }

  public List<AnalyticEventModel> GetAll()
  {
    List<AnalyticEventModel> ouput = new List<AnalyticEventModel>();
    IDbCommand dbcmd;
    dbcmd = _connection.CreateCommand();
    dbcmd.CommandText = "SELECT * FROM Events";
    IDataReader reader = dbcmd.ExecuteReader();
    while (reader.Read())
    {
      int id = reader.GetInt32(0);
      int type = reader.GetInt32(1);
      int guideId = reader.GetInt32(2);
      int? themeId = reader.IsDBNull(3) ? null : reader.GetInt32(3);
      int? mediaId = reader.IsDBNull(4) ? null : reader.GetInt32(4);
      string timeStamp = reader.GetString(5);
      int? value = reader.IsDBNull(6) ? null : reader.GetInt32(6);

      AnalyticEventModel analyticEventModel = new AnalyticEventModel(id, guideId, timeStamp, type, value, themeId, mediaId);
      ouput.Add(analyticEventModel);
    }

    return ouput;
  }

  public void DeleteAll()
  {
    IDbCommand dbcmd;
    dbcmd = _connection.CreateCommand();
    dbcmd.CommandText = "DELETE FROM Events";
    dbcmd.ExecuteNonQuery();

#if UNITY_EDITOR
    Debug.Log("AnalyticsLocalRepository => DeleteAll: ");
#endif
  }

  public void Dispose()
  {
    _connection.Close();
#if UNITY_EDITOR
    Debug.Log("AnalyticsLocalRepository => Dispose: disposed");
#endif
  }
}
