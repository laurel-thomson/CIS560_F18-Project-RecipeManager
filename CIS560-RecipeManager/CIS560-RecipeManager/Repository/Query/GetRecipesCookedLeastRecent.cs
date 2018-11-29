﻿using CIS560_RecipeManager.RecipeManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Transactions;


namespace CIS560_RecipeManager.Repository
{
    public partial class Query : IQuery
    {
        IDictionary<Recipe, string> GetRecipesCookedLeastRecent()
        {
            Dictionary<Recipe, string> returnDict = new Dictionary<Recipe, string>();
            Dictionary<int, string> tempRecipeIDs = new Dictionary<int, string>(); // Dict holds recipeID and LastDateCooked
            using (var connection = new SqlConnection(Properties.Settings.Default.RecipeDatabaseConnectionString))
            {

                connection.Open();

                // first, get all recipe ID's and LastDateCooked strings to build recipe objects in second transaction
                using (var transaction = new TransactionScope())
                {
                    using (var command = new SqlCommand("[dbo].GetRecipesCookedLeastRecent", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        var result = command.ExecuteReader();

                        if (result.HasRows)
                        {
                            while (result.Read())
                            {
                                tempRecipeIDs.Add(result.GetFieldValue<int>(0), 
                                                    result.GetFieldValue<string>(1));
                            }
                        }
                        else
                        {
                            throw new Exception("No results returning from query; Have any recipes been cooked?");
                        }
                        // Close reader to prevent DB exceptions
                        result.Close();
                    }
                } // should close transaction here automatically

                foreach (KeyValuePair<int, string> kvp in tempRecipeIDs)
                {

                }
            }
        }
    }
}
