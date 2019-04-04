# Tasks

Done? | Task
:---:| ---
✅| Setup Identity Management in Program.cs using JWT
✅| Re-test that front end component can connect to API.
⬜️ | Add register, login, ban, and delete operations to users controller.
⬜️ | Find out how to mock UserManager class to be able to test controller methods.
⬜️ | Remove user from donatorConnectionMap if already exists in requestorConnectionMap.
⬜️ | Add re-authenticate token service that allows user to pass valid, but expired token back to server to have another one recreated for them.


# Dependencies
- SendGrid
    * We use SendGrid to handle all of the dirty work involving making sure automatic emails being sent out are safe and secure. Therefore, we must set a secret key that is used to send emails through SendGrid using our organization account. Follow [this guide](https://docs.microsoft.com/en-us/aspnet/core/security/authentication/accconfirm?view=aspnetcore-2.2&tabs=netcore-cli) to get things setup.

# Get Started Locally

    1. Clone the repository.
    2. Create some locally hosted mysql database.
    3. Create environment variables:
        1. APPLICATION_USERS_DB with a value of a db connection string, Ex. <Server=(localdb)\mssqllocaldb;Database=WaterPlsDb;Trusted_Connection=False;ConnectRetryCount=0;>.
        2. JWT_SIGNING_KEY with a string value of at least 32 characters.
	

# How To:

### Update Database Schema
        1. Navigate to: ../water-pls-server-signalr/signalrtest
        2. Enter command(s):
            1. dotnet ef migrations add <Enter-A-Migration-Commit-Message-Here> --context InsertDbContextNameHere
            2. dotnet ef database update --context InsertNameOfDbContextHere

### Boot Up SQL Server 2017 Locally

&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
Found Via Official [MS Docs](https://docs.microsoft.com/en-us/sql/database-engine/configure-windows/scm-services-configure-server-startup-options?view=sql-server-2017)

    1. Start -> SQL Server Configuration Manager
    2. Click SQL  Server Services
    3. In the right pane, right-click SQL Server (<instance_name>), and then click Properties.
    4. On the Startup Parameters tab, in the Specify a startup parameter box, type the parameter, and then click Add.

        For example, to start in single-user mode, type -m in the Specify a startup parameter box and then click Add. (When you restart SQL Server in single-user mode, stop the SQL Server Agent. Otherwise, SQL Server Agent might connect first and prevent you from connecting as a second user.)
    5. Click OK.
    6. Restart the Database Engine.



	

## Notes on Design Decisions

### [Bearer Token over Cookies](https://docs.microsoft.com/en-us/aspnet/core/signalr/authn-and-authz?view=aspnetcore-2.2)

&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Because cookies are specific to browsers, sending them from other kinds of clients adds complexity compared to sending bearer tokens. For this reason, cookie authentication isn't recommended unless the app only needs to authenticate users from the browser client. Bearer token authentication is the recommended approach when using clients other than the browser client.