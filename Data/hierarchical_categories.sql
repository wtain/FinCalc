SELECT CategoryId, ParentId, CategoryPath, PathLen
FROM 
(
	WITH RECURSIVE
	categories_recursive(CategoryId, ParentId, CategoryPath, PathLen) AS 
	(
		 SELECT CategoryId, ParentId, Name AS CategoryPath, 1 AS PathLen FROM categories
		 UNION ALL
		 SELECT categories.CategoryId, 
				   categories.ParentId, 
				   categories_recursive.CategoryPath || '\' || categories.Name as CategoryPath,
				   categories_recursive.PathLen + 1 as PathLen
		 FROM categories, categories_recursive 
		 WHERE categories.ParentId=categories_recursive.CategoryId
	)
	SELECT CategoryId, ParentId, CategoryPath, PathLen
	FROM categories_recursive
)
GROUP BY CategoryId
HAVING PathLen=MAX(PathLen)
ORDER BY CategoryId;