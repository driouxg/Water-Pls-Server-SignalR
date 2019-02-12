# Tasks

Done? | Task
:---:| ---
⬜️| Setup Identity Management in Program.cs using JWT
⬜️| Re-test that front end component can connect to API.
⬜️ | Add register, login, ban, and delete operations to users controller.
✅ | <Insert Additional Task Name Here>


# Dependencies
- SendGrid
    * We use SendGrid to handle all of the dirty work involving making sure automatic emails being sent out are safe and secure. Therefore, we must set a secret key that is used to send emails through SendGrid using our organization account. Follow [this guide](https://docs.microsoft.com/en-us/aspnet/core/security/authentication/accconfirm?view=aspnetcore-2.2&tabs=netcore-cli) to get things setup.

# How To:

### Update Database Schema
        1. Navigate to: ../water-pls-server-signalr/signalrtest
        2. Enter command(s):
            1. dotnet ef migrations add <Enter-A-Migration-Commit-Message-Here>
            2. dotnet ef database update 

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


### Create Self Signed Certificate

- [Official MS Docs](https://docs.microsoft.com/en-us/powershell/module/pkiclient/new-selfsignedcertificate?view=win10-ps)
- [Article](https://medium.com/the-new-control-plane/generating-self-signed-certificates-on-windows-7812a600c2d8)

    Enter Command(s):
    1. $cert = New-SelfSignedCertificate -certstorelocation cert:\localmachine\my -dnsname company.co.nz
    2. $pwd = ConvertTo-SecureString -String ‘password1234’ -Force -AsPlainText
    3. $path = ‘cert:\localMachine\my\’ + $cert.thumbprint
    4. Export-PfxCertificate -cert $path -FilePath c:\enteryourdesireddirectoryhere\certificate\powershellcert.pfx -Password $pwd

## Notes on Design Decisions

### [Bearer Token over Cookies](https://docs.microsoft.com/en-us/aspnet/core/signalr/authn-and-authz?view=aspnetcore-2.2)

&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Because cookies are specific to browsers, sending them from other kinds of clients adds complexity compared to sending bearer tokens. For this reason, cookie authentication isn't recommended unless the app only needs to authenticate users from the browser client. Bearer token authentication is the recommended approach when using clients other than the browser client.