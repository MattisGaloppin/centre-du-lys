namespace Serveur.Model.Data
{
    using System.Data;
    using Microsoft.Data.Sqlite;

    /// <summary>
    /// Interface pour définir les opérations principales d'une base de données.
    /// Permet l'exécution de requêtes SQL et de commandes non requêtées.
    /// </summary>
    public interface IDatabase
    {
        /// <summary>
        /// Exécute une requête SQL et retourne les résultats sous forme de <see cref="DataTable"/>.
        /// </summary>
        /// <param name="query">La requête SQL à exécuter.</param>
        /// <param name="parameters">
        /// Un tableau de paramètres SQL (facultatif) à inclure dans la requête.
        /// Si aucun paramètre n'est requis, utilisez <c>null</c>.
        /// </param>
        /// <returns>Un <see cref="DataTable"/> contenant les résultats de la requête.</returns>
        /// <exception cref="Exception">Lancée si une erreur se produit lors de l'exécution de la requête.</exception>
        public DataTable ExecuteQuery(string query, SqliteParameter[] parameters = null);

        /// <summary>
        /// Exécute une commande SQL qui ne retourne pas de résultats, comme INSERT, UPDATE ou DELETE.
        /// </summary>
        /// <param name="commandText">Le texte de la commande SQL à exécuter.</param>
        /// <param name="parameters">
        /// Un tableau de paramètres SQL (facultatif) à inclure dans la commande.
        /// Si aucun paramètre n'est requis, utilisez <c>null</c>.
        /// </param>
        /// <returns>Le nombre de lignes affectées par la commande.</returns>
        /// <exception cref="Exception">Lancée si une erreur se produit lors de l'exécution de la commande SQL.</exception>
        public int ExecuteNonQuery(string commandText, SqliteParameter[] parameters = null);

        /// <summary>
        /// se connecte a la bd
        /// </summary>
        public void Connect();

        /// <summary>
        /// se deconnecte de la bd
        /// </summary>
        public void Disconnect();
    }

}
