DROP VIEW IF EXISTS ExpenseByCategory;
CREATE VIEW ExpenseByCategory AS 
SELECT exp.Date as Date, 
          cat.Name AS Category, 
		  exp.Amount AS Amount
 FROM expenses exp
 LEFT JOIN categories cat
 ON exp.CategoryId=cat.CategoryId;