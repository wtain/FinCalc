SELECT exp.Date as Date, 
          cat.Name AS Category, 
		  cat.CategoryPath as Path,
		  exp.Amount AS Amount
 FROM expenses exp
 LEFT JOIN 
(
	SELECT Name, CategoryId, ParentId, CategoryPath, PathLen
	FROM 
	(
		WITH RECURSIVE
		categories_recursive(Name, CategoryId, ParentId, CategoryPath, PathLen) AS 
		(
			 SELECT Name, CategoryId, ParentId, Name AS CategoryPath, 1 AS PathLen FROM categories
			 UNION ALL
			 SELECT categories.Name, 
			           categories.CategoryId, 
					   categories.ParentId, 
					   categories_recursive.CategoryPath || '\' || categories.Name as CategoryPath,
					   categories_recursive.PathLen + 1 as PathLen
			 FROM categories, categories_recursive 
			 WHERE categories.ParentId=categories_recursive.CategoryId
		)
		SELECT Name, CategoryId, ParentId, CategoryPath, PathLen
		FROM categories_recursive
	)
	GROUP BY CategoryId
	HAVING PathLen=MAX(PathLen)
	ORDER BY CategoryId
) cat
ON exp.CategoryId=cat.CategoryId;