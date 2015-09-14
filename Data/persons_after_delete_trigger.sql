DROP TRIGGER IF EXISTS persons_after_delete_trigger;
CREATE TRIGGER persons_after_delete_trigger
AFTER DELETE
ON categories
BEGIN
		DELETE FROM accounts WHERE OwnerPErsonId=OLD.PersonId;
END;