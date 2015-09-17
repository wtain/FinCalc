DROP TRIGGER categories_before_delete_trigger;
CREATE TRIGGER categories_before_delete_trigger
BEFORE DELETE 
ON categories
BEGIN
	SELECT RAISE(ABORT, 'Category is referenced in expenses')
	WHERE EXISTS (SELECT * FROM expenses WHERE CategoryId=OLD.CategoryId);
END;