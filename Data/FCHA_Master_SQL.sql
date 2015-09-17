BEGIN TRANSACTION;
CREATE TABLE "expenses" (
	`ExpenseId`	INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,
	`AccountId`	INTEGER NOT NULL,
	`Amount`	INTEGER NOT NULL,
	`CategoryId`	INTEGER NOT NULL,
	`Date`	TEXT NOT NULL,
	`Description`	TEXT
);
CREATE TRIGGER persons_after_delete_trigger
AFTER DELETE
ON persons
BEGIN
		DELETE FROM accounts WHERE OwnerPersonId=OLD.PersonId;
END;
CREATE TRIGGER expenses_update_after_trigger
AFTER UPDATE 
ON expenses
BEGIN
	UPDATE AccountBalance 
		 SET 
			Value=Value - OLD.Amount + NEW.Amount, 
			LastUpdatedDate = NEW.date
		 WHERE AccountId = OLD.AccountId;
END;
CREATE TRIGGER expenses_insert_after_trigger
AFTER INSERT 
ON expenses
BEGIN
	UPDATE AccountBalance 
		 SET 
			Value=Value + NEW.Amount, 
			LastUpdatedDate = NEW.Date
		 WHERE AccountId = NEW.AccountId;
END;
CREATE TRIGGER expenses_delete_after_trigger
AFTER DELETE 
ON expenses
BEGIN
	UPDATE AccountBalance 
		 SET 
			Value=Value - OLD.Amount, 
			LastUpdatedDate = date('now')
		 WHERE AccountId = OLD.AccountId;
END;
CREATE TRIGGER categories_view_insert_trigger
INSTEAD OF INSERT 
ON categories_view
BEGIN
        INSERT INTO categories(CategoryId, ParentId, Name, SeqNo) 
        SELECT NEW.CategoryId, NEW.ParentId, NEW.Name, NEW.CategoryId; 
END;
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
CREATE TRIGGER categories_insert_after_trigger
AFTER INSERT 
ON categories
BEGIN
	UPDATE categories
	     SET SeqNo=CategoryId
    WHERE SeqNo is NULL;
END;
CREATE TRIGGER categories_before_delete_trigger
BEFORE DELETE 
ON categories
BEGIN
	SELECT RAISE(ABORT, 'Category is referenced in  expenses')
	WHERE EXISTS (SELECT * FROM expenses WHERE CategoryId=OLD.CategoryId);
END;
CREATE TRIGGER accounts_insert_after_trigger
AFTER INSERT 
ON accounts
BEGIN
	INSERT INTO AccountBalance (AccountId, LastUpdatedDate, Value)
	SELECT NEW.AccountId, date('now'), 0;
END;
CREATE VIEW categories_view
AS
SELECT CategoryId, ParentId, Name
FROM categories
ORDER BY ParentId, SeqNo;
COMMIT;
