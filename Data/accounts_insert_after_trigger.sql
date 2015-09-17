CREATE TRIGGER accounts_insert_after_trigger
AFTER INSERT 
ON accounts
BEGIN
	INSERT INTO AccountBalance (AccountId, LastUpdatedDate, Value)
	SELECT NEW.AccountId, date('now'), 0;
END;