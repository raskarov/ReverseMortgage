using System;
using System.Collections.Generic;
//********************************************************************************************************************************************
[Serializable]
public class UsersData
{
    private int _user_id;
    private string _email;
    //private string _first_name;
    //private string _last_name;
    private string _display_name;
    //----------------------------------------------------------------------------------------------------------------------------------------
    public int UserID
    {
        get { return _user_id; }
        set { _user_id = value; }
    }
    //----------------------------------------------------------------------------------------------------------------------------------------
    public string Email
    {
        get { return _email; }
        set { _email = value; }
    }
    //----------------------------------------------------------------------------------------------------------------------------------------
    //public string FirstName
    //{
    //    get { return _first_name; }
    //    set { _first_name = value; }
    //}
    ////----------------------------------------------------------------------------------------------------------------------------------------
    //public string LastName
    //{
    //    get { return _last_name; }
    //    set { _last_name = value; }
    //}
    public string DisplayName
    {
        get { return _display_name; }
        set { _display_name = value; }
    }
}
//********************************************************************************************************************************************