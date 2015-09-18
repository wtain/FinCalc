using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Data.SQLite;
using System.Windows;

namespace FCHA
{
	public static class OlapBuilder
	{
		// todo: remove recursion
		private static void BuildTreeRecursive(TempTable olapStage, OlapDimensionsTree root, LinkedList<KeyValuePair<string, string>> filters, string[] columns, int index)
		{
			if (index == columns.Length)
				return;
			string col = columns[index];
			foreach (string value in olapStage.GetColumnValues(col, filters))
			{
				OlapDimensionsTree node = new OlapDimensionsTree(col, value);
				root.children.Add(node);
				filters.AddLast(new LinkedListNode<KeyValuePair<string, string>>(new KeyValuePair<string, string>(col, value)));
				BuildTreeRecursive(olapStage, node, filters, columns, index + 1);
				filters.RemoveLast();
			}
		}

		public static OlapDimensionsTree BuildDimensionsTree(TempTable olapStage, string[] columns)
		{
			LinkedList<KeyValuePair<string, string>> filters = new LinkedList<KeyValuePair<string, string>>();
			OlapDimensionsTree root = new OlapDimensionsTree();
			BuildTreeRecursive(olapStage, root, filters, columns, 0);
			root.CalculateWeight();
			return root;
		}

		private static TextBlock CreateCell(int row, int column, string value, bool isHeader)
		{
			TextBlock cell = new TextBlock();
			cell.Text = value;
			cell.HorizontalAlignment = HorizontalAlignment.Center;
			cell.VerticalAlignment = VerticalAlignment.Center;
			cell.SetValue(Grid.RowProperty, row);
			cell.SetValue(Grid.ColumnProperty, column);
			if (!isHeader)
				cell.Style = App.MoneyAmountTextStyle;
			return cell;
		}

		public static Grid BuildOlapView(SQLiteConnection conn, string tableName, OlapStage stage)
		{
			using (TempTable olapStage = new TempTable(conn, QueryBuilder.BuildOlapStage(tableName, stage)))
			{
				OlapDimensionsTree leftTree = BuildDimensionsTree(olapStage, stage.left);
				OlapDimensionsTree topTree = BuildDimensionsTree(olapStage, stage.top);
				
				Grid grid = new Grid();
				grid.ShowGridLines = true;

				foreach (var d in stage.left)
					grid.ColumnDefinitions.Add(new ColumnDefinition());

				foreach (var d in stage.top)
					grid.RowDefinitions.Add(new RowDefinition());

				leftTree.IterateLeaves((n, f) =>
				{
					grid.RowDefinitions.Add(new RowDefinition());
				});

				topTree.IterateLeaves((n, f) =>
				{
					grid.ColumnDefinitions.Add(new ColumnDefinition());
				});

				// ... ttt
				int row = topTree.Height - 1;
				int column = leftTree.Height - 2;
				leftTree.IterateLeaves((n, f) => grid.Children.Add(CreateCell(row++, column, n.value, true)));

				row = topTree.Height - 2;
				column = leftTree.Height - 1;
				topTree.IterateLeaves((n, f) => grid.Children.Add(CreateCell(row, column++, n.value, true)));

				row = topTree.Height - 1;
				
				leftTree.IterateLeaves((leftValue, leftFilters) =>
				{
					column = leftTree.Height - 1;
					topTree.IterateLeaves((topValue, topFilters) =>
					{
						string data = olapStage.SelectData(leftFilters, topFilters, stage.aggregations[0]).ToString();
						grid.Children.Add(CreateCell(row, column, data, false));
						++column;
					});
					++row;
				});

				return grid;
			}
		}
	}
}
