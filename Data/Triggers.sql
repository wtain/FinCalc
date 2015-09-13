DROP VIEW categories_view;
CREATE VIEW categories_view
AS
SELECT CategoryId, ParentId, Name
FROM categories
ORDER BY ParentId, SeqNo;

DROP TRIGGER categories_insert_trigger;
CREATE TRIGGER categories_insert_trigger
BEFORE INSERT 
ON categories
BEGIN
        INSERT INTO debug_log(tableName, opName, fieldName, valueStr) 
        SELECT 'categories', 'INSERT', 'Name', NEW.Name;
		
		INSERT INTO debug_log(tableName, opName, fieldName, valueInt) 
        SELECT 'categories', 'INSERT', 'CategoryId', NEW.CategoryId;
		
		INSERT INTO debug_log(tableName, opName, fieldName, valueInt) 
        SELECT 'categories', 'INSERT', 'ParentId', NEW.ParentId;
		
		INSERT INTO debug_log(tableName, opName, fieldName, valueInt) 
        SELECT 'categories', 'INSERT', 'SeqNo', NEW.SeqNo;
END;

DROP TRIGGER categories_insert_after_trigger;
CREATE TRIGGER categories_insert_after_trigger
AFTER INSERT 
ON categories
BEGIN
	UPDATE categories
	     SET SeqNo=CategoryId
    WHERE SeqNo is NULL;
END;

DROP TRIGGER IF EXISTS categories_view_insert_trigger;
CREATE TRIGGER categories_view_insert_trigger
INSTEAD OF INSERT 
ON categories_view
BEGIN
        INSERT INTO categories(CategoryId, ParentId, Name, SeqNo) 
        SELECT NEW.CategoryId, NEW.ParentId, NEW.Name, NEW.CategoryId; 
END;
