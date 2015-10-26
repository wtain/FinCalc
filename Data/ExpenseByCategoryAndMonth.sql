DROP VIEW IF EXISTS ExpenseByCategoryAndMonth;
CREATE VIEW ExpenseByCategoryAndMonth AS 
SELECT strftime('%m/%Y', date(exp.Date)) as Month, 
          cat.Name AS Category, 
		  CASE
			WHEN cat.IsIncome=1 THEN 1
			ELSE -1
		  END * exp.Amount AS Amount
 FROM expenses exp
 LEFT JOIN categories cat
 ON exp.CategoryId=cat.CategoryId;
