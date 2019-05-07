using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace ToDoList.Models
{
  public class Category
  {
      private static List<Category> _instances = new List<Category> {};
      private string _name;
      private int _id;

      public Category(string categoryName)
      {
          _name = categoryName;
      }

      public string GetName()
      {
          return _name;
      }

      public static void ClearAll()
      {
          _instances.Clear();
      }

      public void Save()
      {
        MySqlConnection conn = DB.Connection();
        conn.Open();
        var cmd = conn.CreateCommand() as MySqlCommand;
        cmd.CommandText = @"INSERT INTO categories (name) VALUES (@CategoryDescription);";
        MySqlParameter name = new MySqlParameter();
        name.ParameterName = "@CategoryDescription";
        name.Value = this._name;
        MySqlParameter dueDate = new MySqlParameter();
        cmd.Parameters.Add(name);
        cmd.ExecuteNonQuery();
        _id = (int) cmd.LastInsertedId;
        conn.Close();
        if (conn != null)
        {
          conn.Dispose();
        }
      }

      public static List<Category> GetAll()
      {
        //return _instances;
        List<Category> allCategories = new List<Category> {};
        MySqlConnection conn = DB.Connection();
        conn.Open();
        MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
        cmd.CommandText = @"SELECT name FROM categories ORDER BY id;";
        MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
        while(rdr.Read())
        {
          string categoryName = rdr.GetString(0);
          Category newCategory = new Category(categoryName);
          allCategories.Add(newCategory);
        }
        return allCategories;
      }

      public static Category Find(int id)
      {
        MySqlConnection conn = DB.Connection();
        conn.Open();
        var cmd = conn.CreateCommand() as MySqlCommand;
        cmd.CommandText = @"SELECT * FROM `categories` WHERE id = @thisId;";
        MySqlParameter thisId = new MySqlParameter();
        thisId.ParameterName = "@thisId";
        thisId.Value = id;
        cmd.Parameters.Add(thisId);
        var rdr = cmd.ExecuteReader() as MySqlDataReader;
        int categoryId = 0;
        string categoryName = "";
        rdr.Read();
        categoryName = rdr.GetString(0);
        categoryId = rdr.GetInt32(1);
        Category foundCategory= new Category(categoryName);
        conn.Close();
        if (conn != null)
        {
          conn.Dispose();
        }
        return foundCategory;
      }
        //return _instances[searchId-1];

    }
}
