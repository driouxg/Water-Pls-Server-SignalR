# Tasks

- [] Setup Identity Management in Program.cs using JWT
- [] Re-test that front end component can connect to API.
- [] Add register, login, ban, and delete operations to users controller.

# How To:

    ## Update Database Schema
        1. Navigate to: ../water-pls-server-signalr/signalrtest
        2. Enter command(s):
            1. dotnet ef migrations add <Enter-A-Migration-Commit-Message-Here>
            2. dotnet ef database update 