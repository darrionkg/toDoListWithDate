using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System;

namespace ToDoList.Models
{

  public class Item
  {
    private string _description;
    private int _id;
    private DateTime _dueDate;
    private int _categoryId;

    public Item (string description, int id = 0)
    {
      _description = description;
      _id = id;
    }

    public Item (string description, DateTime dueDate, int categoryId, int id = 0)
    {
      _description = description;
      _id = id;
      _dueDate = dueDate;
      _categoryId = categoryId;
    }

    public string GetDescription()
    {
      return _description;
    }

    public void SetDescription(string newDescription)
    {
      _description = newDescription;
    }

    public int GetId()
    {
      return _id;
    }

    public DateTime GetDueDate()
    {
      return _dueDate;
    }

    public void SetDueDate(DateTime dueDate)
    {
      _dueDate = dueDate;
    }

    public override bool Equals(System.Object otherItem)
    {
      if (!(otherItem is Item))
      {
        return false;
      }
      else
      {
        Item newItem = (Item) otherItem;
        bool idEquality = (this.GetId() == newItem.GetId());
        bool descriptionEquality = (this.GetDescription() == newItem.GetDescription());
        bool dueDateEquality = (this.GetDueDate() == newItem.GetDueDate());
        return (idEquality && descriptionEquality);
      }
    }

    public void Save()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO items (description, dueDate) VALUES (@ItemDescription, @ItemDueDate);";
      MySqlParameter description = new MySqlParameter();
      description.ParameterName = "@ItemDescription";
      description.Value = this._description;
      MySqlParameter dueDate = new MySqlParameter();
      dueDate.ParameterName = "@ItemDueDate";
      dueDate.Value = this._dueDate;
      cmd.Parameters.Add(description);
      cmd.Parameters.Add(dueDate);
      cmd.ExecuteNonQuery();
      _id = (int) cmd.LastInsertedId;
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

    public static List<Item> GetAll()
    {
      List<Item> allItems = new List<Item> {};
      MySqlConnection conn = DB.Connection();
      conn.Open();
      MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM items ORDER BY dueDate DESC;";
      MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
      while(rdr.Read())
      {
        int itemId = rdr.GetInt32(0);
        string itemDescription = rdr.GetString(1);
        DateTime itemDueDate = rdr.GetDateTime(2);
        int categoryId = rdr.GetInt32(3);
        Item newItem = new Item(itemDescription, itemDueDate, itemId, categoryId);
        allItems.Add(newItem);
      }
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return allItems;
    }

    public static void ClearAll()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"DELETE FROM items;";
      cmd.ExecuteNonQuery();
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

    public static Item Find(int id)
    {
        MySqlConnection conn = DB.Connection();
        conn.Open();
        var cmd = conn.CreateCommand() as MySqlCommand;
        cmd.CommandText = @"SELECT * FROM `items` WHERE id = @thisId;";
        MySqlParameter thisId = new MySqlParameter();
        thisId.ParameterName = "@thisId";
        thisId.Value = id;
        cmd.Parameters.Add(thisId);
        var rdr = cmd.ExecuteReader() as MySqlDataReader;
        int itemId = 0;
        string itemDescription = "";
        rdr.Read();
        itemId = rdr.GetInt32(0);
        itemDescription = rdr.GetString(1);
        DateTime itemDueDate = rdr.GetDateTime(2);
        Item foundItem= new Item(itemDescription, itemDueDate, itemId);
        conn.Close();
        if (conn != null)
        {
          conn.Dispose();
        }
        return foundItem;
    }
  }

}
