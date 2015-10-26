DROP VIEW IF EXISTS ExpenseByTopLevelCategoryAndMonth;
CREATE VIEW ExpenseByTopLevelCategoryAndMonth AS 
WITH RECURSIVE categories_recursive(CategoryId, Name, ParentId, Level, RootId, RootName, IsIncome) AS 
(
 SELECT CategoryId, Name, ParentId, 0, CategoryId, Name, IsIncome
 FROM categories WHERE ParentId=0
	UNION ALL
 SELECT categories.CategoryId, 
           categories.Name,
		   categories.ParentId, 
		   categories_recursive.Level+1 as Level,
		   CASE 
			   WHEN categories_recursive.Level < 1 THEN categories.ParentId
			   ELSE categories_recursive.RootId
		   END as RootId,
		   categories_recursive.RootName as RootName,
		   categories.IsIncome
	FROM categories, categories_recursive 
	WHERE categories.ParentId=categories_recursive.CategoryId
)
SELECT strftime('%m/%Y', date(exp.Date)) as Month, 
	      cr.RootName AS Category, 
		  CASE 
			WHEN cr.IsIncome=1 THEN 1
			ELSE -1
		  END * exp.Amount AS Amount
FROM expenses exp
LEFT JOIN 
categories_recursive cr
ON exp.CategoryId=cr.CategoryId;
