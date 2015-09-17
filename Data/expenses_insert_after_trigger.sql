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