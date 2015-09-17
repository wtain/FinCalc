DROP TRIGGER expenses_insert_after_trigger;
CREATE TRIGGER expenses_insert_after_trigger
AFTER INSERT 
ON expenses
BEGIN
	UPDATE AccountBalance 
		 SET 
			Value=Value - NEW.Amount, 
			LastUpdatedDate = datetime('now')
		 WHERE AccountId = NEW.AccountId;
END;