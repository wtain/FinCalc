DROP TRIGGER expenses_delete_after_trigger;
CREATE TRIGGER expenses_delete_after_trigger
AFTER DELETE 
ON expenses
BEGIN
	UPDATE AccountBalance 
		 SET 
			Value=Value + OLD.Amount, 
			LastUpdatedDate = datetime('now')
		 WHERE AccountId = OLD.AccountId;
END;