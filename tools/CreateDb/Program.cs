using Microsoft.Data.SqlClient;

var masterConnectionString = "Server=localhost,1433;Database=master;User Id=sa;Password=YourStrong@Passw0rd;TrustServerCertificate=True";

var databases = new[] {
    "NoDaysOff.Identity",
    "NoDaysOff.Athletes",
    "NoDaysOff.Workouts",
    "NoDaysOff.Exercises",
    "NoDaysOff.Dashboard",
    "NoDaysOff.Media",
    "NoDaysOff.Communication"
};

Console.WriteLine("Connecting to SQL Server...");

try
{
    using var connection = new SqlConnection(masterConnectionString);
    connection.Open();
    Console.WriteLine("Connected to SQL Server.");

    foreach (var dbName in databases)
    {
        Console.WriteLine($"Creating database '{dbName}'...");
        using var cmd = connection.CreateCommand();
        cmd.CommandText = $"IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = '{dbName}') CREATE DATABASE [{dbName}]";
        cmd.ExecuteNonQuery();
        Console.WriteLine($"Database '{dbName}' created or already exists.");
    }

    Console.WriteLine("All databases created successfully!");
}
catch (Exception ex)
{
    Console.WriteLine($"Error: {ex.Message}");
    Console.WriteLine($"Stack trace: {ex.StackTrace}");
    Environment.Exit(1);
}
