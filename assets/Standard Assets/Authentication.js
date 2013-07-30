	import System;
	import System.Data;
	import System.Data.SqlClient;
	
	var dbcmdInsert : IDbCommand;
	var dbcmdSelect : IDbCommand;
	var dbcmdDelete : IDbCommand;
	
	var cmdSqlInsert : String;
	var cmdSqlSelect : String;
	var cmdSqlDelete : String;
	
	
	function Start () {
	
	}
 
    function NewUser(UserName, Email, Password, FirstName, LastName){
    	
    	if(!Available(UserName)){
    		//print("Username: " + UserName + " already exists, please choose a different username.");
    		return false;
    	}
    	
    	else{
	    	//DATABASE ACCESS INFO: DO NOT MODIFY
		    var dbcon : IDbConnection;
			
		    var connectionString : String =
		        "Server=sql2104.shared-servers.com,1089;" + // put the ip here!
		        "Database=ForgottenLand;" +
		        "User ID=guard;" +
		        "Password=Passnew1994!;";
		        
		    dbcon = new SqlConnection(connectionString);
		    dbcon.Open();
		    
		    Password = Md5Sum(Password);
		    
		    //create the class EXECUTIONER, that execute SQL commands
		    dbcmdInsert = dbcon.CreateCommand();  
		    cmdSqlInsert = "INSERT INTO [ForgottenLand].[guard].[Registry] (UserName, Email, Password, FirstName, LastName) VALUES ('" + 
		    		UserName + "','" + Email + "','" + Password + "','" + FirstName + "','" + LastName + "');";
		    dbcmdInsert.CommandText = cmdSqlInsert; 
		    var reader : IDataReader; 
		    
		    //we add the command, as string, to the executor, to shot it!
		    reader = dbcmdInsert.ExecuteReader();
		    reader.Close();
		    reader = null;
		    dbcon.Close();
		    dbcon = null;
		    //print("Username: " + UserName + " is added to Database Registry Table");
		    return true;
	    }
	}    
	   	    
	function Delete(UserName){    
	    
	    if(Available(UserName)){
	    	//print("Atempt to delete a username: " + UserName + " that does not exist");
	    	return false;
	    } else {
		    //DATABASE ACCESS INFO: DO NOT MODIFY
		    var dbcon : IDbConnection;
			
		    var connectionString : String =
		        "Server=sql2104.shared-servers.com,1089;" + // put the ip here!
		        "Database=ForgottenLand;" +
		        "User ID=guard;" +
		        "Password=Passnew1994!;";
		        
		    dbcon = new SqlConnection(connectionString);
		    dbcon.Open();
		    
		    //create the class EXECUTIONER, that execute SQL commands
		    dbcmdDelete = dbcon.CreateCommand();  
		    cmdSqlDelete = "DELETE FROM [ForgottenLand].[guard].[Registry] WHERE UserName = '" + UserName + "';";
			dbcmdDelete.CommandText = cmdSqlDelete;
		    var reader : IDataReader;  
		    
		    //we add the command, as string, to the executor, to shot it!  
		    reader = dbcmdDelete.ExecuteReader();
		    reader.Close();
		    reader = null;
		    dbcon.Close();
		    dbcon = null;
		    	    
		    //print("Username: " + UserName + " is deleted from Database Registry Table");
		    return true;
	    }	     
    } 
    
    function Authenticate(UserName, Password){
    	
    	//DATABASE ACCESS INFO: DO NOT MODIFY
    	var dbcon : IDbConnection;
		
	    var connectionString : String =
	        "Server=sql2104.shared-servers.com,1089;" + // put the ip here!
	        "Database=ForgottenLand;" +
	        "User ID=guard;" +
	        "Password=Passnew1994!;";
	        
	    dbcon = new SqlConnection(connectionString);
	    dbcon.Open();
	    
	    Password = Md5Sum(Password);
	    
	    //create the class EXECUTIONER, that execute SQL commands
	    dbcmdSelect = dbcon.CreateCommand();
	    cmdSqlSelect = "SELECT UserName, Password FROM [ForgottenLand].[guard].[Registry]";
	    dbcmdSelect.CommandText = cmdSqlSelect;
	    var reader : IDataReader; 
	    
	    //we add the command, as string, to the executor, to shot it!
	    reader = dbcmdSelect.ExecuteReader();
	    
	    //and then, we create a table, like a normal db table, to use it on unity, and we use the function that "plays" the command
	    while(reader.Read()) {
	        var UserNameDB : String = reader ["UserName"].ToString();
	        var PasswordDB : String = reader ["Password"].ToString();
	        
	        if(UserName == UserNameDB)
	        {
	        	if(Password == PasswordDB)
	        	{	
	        		reader.Close();
	   				reader = null;
	   				dbcon.Close();
	   				dbcon = null;
	   				
	   				//print ("Username: " + UserName + " is authenticated, username is found and password match");
	        		return true;   		
	        	}
	        	//print ("Username: " + UserName + " is not authenticated, username is found but password does not match");
	        	break;
	        }
	    }
         
	    reader.Close();
	    reader = null;
	    dbcon.Close();
	    dbcon = null;
    	
    	//print ("Username: " + UserName + " is not found, user is not authenticated");
    	return false;
    }
    
    function Available(UserName){
    
    	//DATABASE ACCESS INFO: DO NOT MODIFY
    	var dbcon : IDbConnection;
		
	    var connectionString : String =
	        "Server=sql2104.shared-servers.com,1089;" + // put the ip here!
	        "Database=ForgottenLand;" +
	        "User ID=guard;" +
	        "Password=Passnew1994!;";
	        
	    dbcon = new SqlConnection(connectionString);
	    dbcon.Open();
	    
	    //create the class EXECUTIONER, that execute SQL commands
	    dbcmdSelect = dbcon.CreateCommand();
	    cmdSqlSelect = "SELECT UserName FROM [ForgottenLand].[guard].[Registry]";
	    dbcmdSelect.CommandText = cmdSqlSelect;
	    var reader : IDataReader; 
	    
	    //we add the command, as string, to the executor, to shot it!
	    reader = dbcmdSelect.ExecuteReader();
	    
	    //and then, we create a table, like a normal db table, to use it on unity, and we use the function that "plays" the command
	    while(reader.Read()) {
	        var UserNameDB : String = reader ["UserName"].ToString();
	        if(UserName == UserNameDB)
	        {
	        	reader.Close();
	    		reader = null;
	    		dbcon.Close();
	    		dbcon = null;
	    		
	    		//print("Username: " + UserName + " already exists, please choose a different username.");
	        	return false;
	        }
	    }
         
	    reader.Close();
	    reader = null;
	    dbcon.Close();
	    dbcon = null;
	    
    	//print ("Username: " + UserName + " is available");
    	return true;  	
    }
    
static function Md5Sum(strToEncrypt: String)
{
	var encoding = System.Text.UTF8Encoding();
	var bytes = encoding.GetBytes(strToEncrypt);
 
	// encrypt bytes
	var md5 = System.Security.Cryptography.MD5CryptoServiceProvider();
	var hashBytes:byte[] = md5.ComputeHash(bytes);
 
	// Convert the encrypted bytes back to a string (base 16)
	var hashString = "";
 
	for (var i = 0; i < hashBytes.Length; i++)
	{
		hashString += System.Convert.ToString(hashBytes[i], 16).PadLeft(2, "0"[0]);
	}
 
	return hashString.PadLeft(32, "0"[0]);
}
    