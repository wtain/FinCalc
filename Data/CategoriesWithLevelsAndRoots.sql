WITH RECURSIVE
categories_recursive(CategoryId, ParentId, Level, RootId) AS 
(
 SELECT CategoryId, ParentId, 0, CategoryId
 FROM categories WHERE ParentId=0
	UNION ALL
 SELECT categories.CategoryId, 
           categories.ParentId, 
           categories_recursive.Level+1 as Level,
		   CASE 
		       WHEN categories_recursive.Level < 1 THEN categories.ParentId
			   ELSE categories_recursive.RootId
		   END as RootId
	FROM categories, categories_recursive 
	WHERE categories.ParentId=categories_recursive.CategoryId
)
SELECT * FROM categories_recursive