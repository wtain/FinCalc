DROP TRIGGER accounts_insert_after_trigger;
CREATE TRIGGER accounts_insert_after_trigger
AFTER INSERT 
ON accounts
BEGIN
	INSERT INTO AccountBalance (AccountId, LastUpdatedDate, Value)
	SELECT NEW.AccountId, datetime('now'), 0;
END;