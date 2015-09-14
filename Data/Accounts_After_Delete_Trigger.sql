DROP TRIGGER IF EXISTS accounts_after_delete_trigger;
CREATE TRIGGER accounts_after_delete_trigger
AFTER DELETE
ON categories
BEGIN
		DELETE FROM AccountBalance WHERE AccountId=OLD.AccountId;
END;