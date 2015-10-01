DROP VIEW IF EXISTS ExpenseByCategoryAndMonth;
CREATE VIEW ExpenseByCategoryAndMonth AS 
SELECT strftime('%m/%Y', date(exp.Date)) as Month, 
          cat.Name AS Category, 
		  exp.Amount AS Amount
 FROM expenses exp
 LEFT JOIN categories cat
 ON exp.CategoryId=cat.CategoryId;
