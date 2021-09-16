
# Database.cs
#### Small note
In the examples right below you'll see that i use a lot of strings variables starting with `$"my string"` it is because it allow me to easily concat variables into my string, it also automatically convert the type of the variable into a string.

Also please note that english is not my main language, i'm a computer science student and as i use this library a lot i've choosed to share it with you :3
## Depedencies
This packages is using `mysql.data`
## How to use it
After downloading the project you just have to include it in your usings using -> `using DatabaseCS`.

After that you'll have to establish the connection with the database using the  `Database.Connect()` (do not forget to surround it with a **try-catch** because an error will be thrown if the connection failed)
The connection method has an overloard containing a password in case if your database needs a password.
Argument -> `(      (Server, Database, Username, (Password)     )`
##### Example without password
```cs
//Establish a connection without a password
Database.Connect("localhost", "MyDb", "root");
``` 
##### Exeample with a password
```cs
//Establish a connection with a password
Database.Connect("localhost", "MyDb", "root", "password");
``` 
### Utilisation
Since the 1.0.2 the usage of this library is way easier, and also faster.
<sub>**\*Before attempting to execute ANY request you HAVE to establish a connection with the server.**</sub>
- Select
The Select method return a list of dictionnary containing your data, the keys to the date is the column name. 
It might look a bit scary but using a **var** type variable makes it easier. 

There are two ways of executing a select request.
##### Exemple standard

```cs
var Data = Database.Select($"SELECT prenom, nom, age FROM tbl_users WHERE id={this.Id}");
```
<sub>(The argument of the method is a String, you can also put a string variable containing your request in here)</sub>
##### Example using the extension method

```cs
var Data = strRequest.Select();
```
or
```cs
var Data = "SELECT * FROM tbl_users".Select();
```
_________________

- How to use the fetched datas
Once the select done you can check all of the datas using a foreach.


##### If you know that your request only return one record you can do something like this
```cs
var Data = strRequete.Select();
Console.WriteLine(Data[0]["firstname"]);
```
<sub>This will display in the console the firstname of the first fetched record.</sub>
- This example will display the names of all of the user in my db, user by user

```cs
var Data = "SELECT * FROM tbl_users".Select();
foreach(Dictionnary<string, string> row in Data){
Console.WriteLine(row["name"]);
}
```

If this example looks a bit too complicated you can also do it like this : 
```cs
var Data = "SELECT * FROM tbl_users".Select();
foreach(var row in Data){
Console.WriteLine(row[nom]);
}
```
<sub> **Be careful ! doing it this way might frighten your teachers..**</sub>
- Insert
<sub>The argument to pass in parameter is your sql request.</sub>

```cs
Database.Write($"INSERT INTO tbl_users(prenom, nom, age) VALUES(\"{firstname}\",\"{name}\",{age}");
```
Or
```cs
$"INSERT INTO tbl_users(prenom, nom, age) VALUES(\"{firstname}\",\"{name}\",{age}".Write();
```
- Update
```cs
Database.Write($"UPDATE tbl_users SET age={age} WHERE nom = \"{name}\"");
```
OU
```cs
$"UPDATE tbl_users SET age={age} WHERE nom = \"{name}\"".Write();
```