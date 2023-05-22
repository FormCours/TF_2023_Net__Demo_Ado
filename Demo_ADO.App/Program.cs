using Demo_ADO.App.Models;
using Microsoft.Data.SqlClient;

// ConnectionString 
string connectionString = @"Server=Forma300\TFTIC;Database=Demo_ADO;User Id=Gontran;Password=Test1234=;TrustServerCertificate=true;";

// Nuget package à installer → Microsoft.Data.SqlClient

#region Connnexion vers la DB
// - Instance d'une connexion + ConnectionString
SqlConnection demoConnection = new SqlConnection();
demoConnection.ConnectionString = connectionString;

// - Ouvrir la connexion
demoConnection.Open();

// - Réaliser du traitement...
Console.WriteLine($"Etat de la connexion {demoConnection.State}");

// - Exemple de requete
SqlCommand demoCommand = demoConnection.CreateCommand();
demoCommand.CommandText = "SELECT COUNT(*) FROM [V_Game];";
demoCommand.CommandType = System.Data.CommandType.Text;

int nbGame = (int)demoCommand.ExecuteScalar();
Console.WriteLine($"Le nombre de jeu dans la DB : {nbGame}");


// - Fermer la connexion
demoConnection.Close();
Console.WriteLine($"Etat de la connexion {demoConnection.State}");
#endregion

#region Récuperation des jeux
// Création de la connexion
using(SqlConnection connection = new SqlConnection())
{
    // Définition de la connection string
    connection.ConnectionString = connectionString;

    // Commande à executer (-> SqlCommand)
    using(SqlCommand command = connection.CreateCommand())
    {
        // - Définition de la requete (Query)
        command.CommandText = "SELECT * FROM [V_Game]";
        command.CommandType = System.Data.CommandType.Text;

        // - Ouverture de la connexion
        connection.Open();

        // - Execution de la requete
        using (SqlDataReader reader = command.ExecuteReader())
        {
            while(reader.Read())
            {
                // Récuperation des données -> Via un modele
                Game game = new Game()
                {
                    Id = (int)reader["Id_Game"],
                    Name = (string)reader["Name"],
                    Resume = reader["Resume"] is DBNull ? null : (string)reader["Resume"],
                    Price = reader["Price"] is DBNull ? null : (decimal)reader["Price"],
                    ReleaseDate = reader["Release_Date"] is DBNull ? null : (DateTime)reader["Release_Date"]
                };

                // Exemple de convertion d'un decimal en double
                // double demo = Convert.ToDouble(reader["Price"]);


                // Traitement ?
                Console.WriteLine($" • {game.Id} -> {game.Name}");
                Console.WriteLine($"   Date de sortie : {game.ReleaseDate?.ToString() ?? "N/A"}");
                Console.WriteLine($"   Prix : {game.Price?.ToString() ?? "N/A"}");
                Console.WriteLine($"   Résumé : {game.Resume ?? "N/A"}");
                Console.WriteLine();
            }
        }

        // Fermer la connexion
        connection.Close();
    }
}
#endregion