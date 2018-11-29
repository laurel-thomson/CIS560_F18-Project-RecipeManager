﻿CREATE PROCEDURE [dbo].[GetRecipesNotCooked]

AS

SELECT *
FROM 
(
	SELECT r.RecipeID, rcd.LastDateCooked
	FROM [dbo].Recipes r
	   LEFT JOIN [dbo].RecipesCookedDates rcd ON r.RecipeID = rcd.RecipeId
		) tempTable
WHERE tempTable.LastDateCooked IS NULL