WITH RECURSIVE
categories_recursive(CategoryId, ParentId, Level) AS 
(
 SELECT CategoryId, ParentId, 0 
 FROM categories WHERE ParentId=0
	UNION ALL
 SELECT categories.CategoryId, categories.ParentId, categories_recursive.Level+1 as Level 
	FROM categories, categories_recursive 
	WHERE categories.ParentId=categories_recursive.CategoryId
)
SELECT * FROM categories_recursive