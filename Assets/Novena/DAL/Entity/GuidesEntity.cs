using System;
using System.Collections.Generic;
using System.Data;
using Mono.Data.Sqlite;
using Novena.Admin;
using Novena.DAL.Db;
using Novena.DAL.Model;
using Novena.Domain.Entities;
using UnityEngine;

namespace Novena.DAL.Entity
{
  /// <summary>
  /// Entity for Guides table.
  /// <para> Dispose of this object is a MUST </para>
  /// </summary>
  public class GuidesEntity : IDisposable
  {
    private string _tableName = "Guides";
    private IDbCommand _cmnd;
    private IDbConnection _connection;

    public GuidesEntity()
    {
      _connection = Database.GetConnection();
      _connection.Open();
      _cmnd = _connection.CreateCommand();
      CreateTable();
    }

    /// <summary>
    /// Create table if not exists.
    /// </summary>
    private void CreateTable()
    {
      string sql = string.Empty;

      sql += "CREATE TABLE IF NOT EXISTS Guides (Id INTEGER, GuideId INTEGER, Json TEXT, Active INTEGER , PRIMARY KEY (Id AUTOINCREMENT));";
      _cmnd.CommandText = sql;
      _cmnd.ExecuteNonQuery();  
    }

    /// <summary>
    /// Insert new or update existing.
    /// </summary>
    /// <param name="guideInfo"></param>
    public void InsertUpdate(GuideInfo guideInfo)
    {
      string sql =
        $"INSERT OR REPLACE INTO Guides (Id, GuideId, Json, Active) " +
        $"VALUES ((SELECT Id FROM Guides WHERE GuideId = {guideInfo.Guide.Id}), {guideInfo.Guide.Id}, @pJson, " +
        $"{guideInfo.IsActive})";

      _cmnd.CommandText = sql;

      SqliteParameter pJson = new SqliteParameter("@pJson", DbType.String);
      pJson.Value = guideInfo.GuideJson;

      _cmnd.Parameters.Add(pJson);

      _cmnd.ExecuteNonQuery();
    }

    /*/// <summary>
    /// Insert new or update existing.
    /// </summary>
    /// <param name="guideData"></param>
    public void InsertUpdate(GuideData guideData)
    {
      string sql =
        $"INSERT OR REPLACE INTO Guides (Id, GuideId, Json, Active) " +
        $"VALUES ((SELECT Id FROM Guides WHERE GuideId = {guideData.Guide.Id}), {guideData.Guide.Id}, @pJson,{guideData.IsActive})";

      _cmnd.CommandText = sql;

      SqliteParameter pJson = new SqliteParameter("@pJson", DbType.String);
      pJson.Value = guideData.GuideJson;
      
      _cmnd.Parameters.Add(pJson);
      
      _cmnd.ExecuteNonQuery();
    }*/

    /// <summary>
    /// Get guide by GuideId
    /// </summary>
    /// <param name="guideId"></param>
    /// <returns></returns>
    public LocalGuide? GetGuideById(int guideId)
    {
      LocalGuide output = null;

      string sql = $"SELECT * FROM Guides WHERE GuideId = {guideId}";

      _cmnd.CommandText = sql;

      IDataReader reader = _cmnd.ExecuteReader();

      while (reader.Read())
      {
        try
        {
          output = new LocalGuideModel(Id: reader.GetInt32(0), GuideId: reader.GetInt32(1),
            Json: reader.GetString(2), Active: reader.GetInt32(3) == 1);
        }
        catch (Exception e)
        {
          Debug.LogException(e);
        }
      }

      reader.Close();

      return output;
    }

    /// <summary>
    /// Get all guides.
    /// </summary>
    /// <returns>List of LocalGuide. Empty list if nothing found!</returns>
    public List<LocalGuide> GetAll()
    {
      List<LocalGuide> output = new List<LocalGuide>();

      string sql = "SELECT * FROM Guides";

      _cmnd.CommandText = sql;

      IDataReader reader = _cmnd.ExecuteReader();

      while (reader.Read())
      {
        LocalGuide localGuide = new LocalGuideModel(Id: reader.GetInt32(0), GuideId: reader.GetInt32(1),
          Json: reader.GetString(2), Active: reader.GetInt32(3) == 1);

        output.Add(localGuide);
      }

      reader.Close();

      return output;
    }

    public void DeleteAll()
    {
      string sql = "DELETE FROM Guides";
      _cmnd.CommandText = sql;
      _cmnd.ExecuteNonQuery();
    }

    public void Dispose()
    {
      _connection.Close();
      _connection.Dispose();
      _cmnd.Dispose();
    }
  }
}