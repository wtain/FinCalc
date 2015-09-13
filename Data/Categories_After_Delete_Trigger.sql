DROP TRIGGER IF EXISTS categories_view_after_delete_trigger;
CREATE TRIGGER categories_view_after_delete_trigger
AFTER DELETE
ON categories
BEGIN
		DELETE FROM categories WHERE CategoryId IN
		(
			WITH RECURSIVE
				categories_recursive(CategoryId, ParentId) AS 
				(
					SELECT CategoryId, ParentId FROM categories WHERE ParentId=OLD.CategoryId
					UNION ALL
					SELECT categories.CategoryId, categories.ParentId FROM categories, categories_recursive 
					WHERE categories.ParentId=categories_recursive.CategoryId
				)
			SELECT CategoryId FROM categories_recursive
		);
END;