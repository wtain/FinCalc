DROP TRIGGER IF EXISTS accounts_after_delete_trigger;
CREATE TRIGGER accounts_after_delete_trigger
AFTER DELETE
ON accounts
BEGIN
		DELETE FROM AccountBalance WHERE AccountId=OLD.AccountId;
END;