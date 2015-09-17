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