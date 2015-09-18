DROP TRIGGER expenses_update_after_trigger;
CREATE TRIGGER expenses_update_after_trigger
AFTER UPDATE 
ON expenses
BEGIN
	UPDATE AccountBalance 
		 SET 
			Value=Value + OLD.Amount, 
			LastUpdatedDate = datetime('now')
		 WHERE AccountId = OLD.AccountId;
	UPDATE AccountBalance 
		 SET 
			Value=Value - NEW.Amount, 
			LastUpdatedDate = datetime('now')
		 WHERE AccountId = NEW.AccountId;
END;