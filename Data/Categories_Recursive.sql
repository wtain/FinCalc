WITH RECURSIVE
categories_recursive(CategoryId, ParentId) AS (
 SELECT CategoryId, ParentId FROM categories WHERE ParentId=0
 UNION ALL
 SELECT categories.CategoryId, categories.ParentId FROM categories, categories_recursive 
 WHERE categories.ParentId=categories_recursive.CategoryId
)
SELECT * FROM categories_recursive