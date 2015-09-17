INSERT INTO AccountBalance (AccountId, LastUpdatedDate, Value)
SELECT AccountId, date('now'), 0 FROM accounts;