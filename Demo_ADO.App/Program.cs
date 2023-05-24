using Demo_ADO.App.Models;
using Microsoft.Data.SqlClient;
using System.Data;

#region ConnectionString 
/* ConnectionString Centre
 * string connectionString = @"Server=Forma300\TFTIC;Database=Demo_ADO;User Id=Gontran;Password=Test1234=;TrustServerCertificate=true;"; */

/* ConnectionString LocalDB*/
string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Demo_ADO_DB;Integrated Security=True";
#endregion

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

#region Manipulation des Genres avec Procédures stockées
/* Insertion d'un genre avec Param OUTPUT
Genre new_genre = new Genre() { Name = "RTS", Description = "Real Time Strategy"};

using (SqlConnection connection = new SqlConnection(connectionString))
{
    //connection.ConnectionString = connectionString;
    connection.Open();
    using(SqlCommand command = connection.CreateCommand()){
        command.CommandText = "SP_Genre_Insert";
        command.CommandType = CommandType.StoredProcedure;
        SqlParameter param_name = new SqlParameter() { 
            ParameterName =  "name", 
            Value="RTS" 
        };
        command.Parameters.Add(param_name);
        command.Parameters.AddWithValue("description", "Real Time Strategy");
        command.Parameters.Add(
            new SqlParameter() { 
                ParameterName= "id",
                Value = 0,
                Direction= ParameterDirection.Output
            }
            );
        command.ExecuteNonQuery();
        new_genre.Id_Genre = (int)command.Parameters["id"].Value;
        Console.WriteLine($"Le nouveau genre '{new_genre.Name}' a l'identifiant {new_genre.Id_Genre}.");
    }
    connection.Close();
}*/

/* Insertion de plusieurs genre avec un Param type TABLE*/

List<Genre> genres_to_add = new List<Genre>() { 
    new Genre(){ Name = "RTS", Description = "Real Time Strategy"},
    new Genre(){ Name = "Horror", Description = null},
    new Genre(){ Name = "Reflection", Description = null}
};


using (SqlConnection connection = new SqlConnection(connectionString))
{
    using (SqlCommand cmd = connection.CreateCommand())
    {
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandText = "SP_Genre_InsertTable";

        // On crée une table
        DataTable data = new DataTable();
        // On défini la structure de la table
        data.Columns.Add("Name", typeof(string));
        data.Columns.Add("Description", typeof(string));
        // On insert les données dans la table
        foreach (Genre genre in genres_to_add)
        {
            data.Rows.Add(genre.Name, genre.Description);
        }

        cmd.Parameters.Add(
            new SqlParameter() { 
                ParameterName = "genres",
                Value = data,
                TypeName = "T_Genre"
            });

        connection.Open();

        int nb_inserted_row = cmd.ExecuteNonQuery();

        Console.WriteLine($"On a ajouté {nb_inserted_row} genre{((nb_inserted_row>1)?"s":"")}");

        connection.Close();
    }
}

#endregion