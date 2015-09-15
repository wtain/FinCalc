DROP TRIGGER IF EXISTS persons_after_delete_trigger;
CREATE TRIGGER persons_after_delete_trigger
AFTER DELETE
ON persons
BEGIN
		DELETE FROM accounts WHERE OwnerPersonId=OLD.PersonId;
END;