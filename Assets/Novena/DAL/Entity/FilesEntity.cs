using System;
using System.Collections.Generic;
using System.Data;
using Novena.DAL.Db;
using Novena.DAL.Model;

namespace Novena.DAL.Entity
{
  /// <summary>
  /// Entity for Files table.
  /// <para> Dispose of this object is a MUST </para>
  /// </summary>
  public class FilesEntity : IDisposable
  {
    private IDbCommand _cmnd;
    private IDbConnection _connection;

    public FilesEntity()
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

      sql += "CREATE TABLE IF NOT EXISTS Files (Id INTEGER , GuideId INTEGER, FilePath TEXT, LocalPath TEXT, TimeStamp TEXT, PRIMARY KEY(Id AUTOINCREMENT));";
      _cmnd.CommandText = sql;
      _cmnd.ExecuteNonQuery();
    }

    /// <summary>
    /// Get all files from db.
    /// </summary>
    /// <returns>List of files or empty list if nothing found!</returns>
    public List<File> GetAll()
    {
      List<File> files = new List<File>();

      string sql = "SELECT * FROM Files";

      _cmnd.CommandText = sql;

      IDataReader reader = _cmnd.ExecuteReader();

      while (reader.Read())
      {
        var file = new File();

        file.Id = Convert.ToInt32(reader[0]);
        file.GuideId = Convert.ToInt32(reader[1]);
        file.FilePath = reader[2].ToString();
        file.LocalPath = reader[3].ToString();
        file.TimeStamp = reader[4].ToString();

        files.Add(file);
      }

      reader.Close();

      return files;
    }

    public File? GetByFilePath(string filePath)
    {
      File file = null;

      string sql = $"SELECT * FROM Files WHERE FilePath = '{filePath}'";

      _cmnd.CommandText = sql;

      IDataReader reader = _cmnd.ExecuteReader();

      while (reader.Read())
      {
        file = new File();

        file.Id = Convert.ToInt32(reader[0]);
        file.GuideId = Convert.ToInt32(reader[1]);
        file.FilePath = reader[2].ToString();
        file.LocalPath = reader[3].ToString();
        file.TimeStamp = reader[4].ToString();
      }

      reader.Close();

      return file;
    }

    /// <summary>
    /// Insert new file to database 
    /// </summary>
    /// <param name="file"></param>
    public void Insert(File file)
    {
      string sql =
        "INSERT INTO Files (GuideId, FilePath, LocalPath, TimeStamp)" +
        "VALUES ( '" + file.GuideId + "', '"
        + file.FilePath + "', '"
        + file.LocalPath + "', '"
        + file.TimeStamp + "' )";

      _cmnd.CommandText = sql;
      _cmnd.ExecuteNonQuery();
    }

    /// <summary>
    /// Update existing record.
    /// </summary>
    /// <param name="file"></param>
    public void Update(File file)
    {
      string sql = $"UPDATE Files SET TimeStamp='{file.TimeStamp}' WHERE FilePath='{file.FilePath}';";

      _cmnd.CommandText = sql;
      _cmnd.ExecuteNonQuery();
    }

    public int DeleteById(int id)
    {
      string sql = $"DELETE FROM Files WHERE Id = {id}";

      _cmnd.CommandText = sql;

      return _cmnd.ExecuteNonQuery();
    }

    public void DeleteAll()
    {
      string sql = "DELETE FROM Files";
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