﻿using System;
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
				OlapDimensionsTree node = new OlapDimensionsTree(col, value, root);
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

		private static TextBlock CreateCell(int row, int column, string value, bool isHeader, OlapCellInfo tag)
		{
			TextBlock cell = new TextBlock();
			cell.Text = value;
			cell.HorizontalAlignment = HorizontalAlignment.Center;
			cell.VerticalAlignment = VerticalAlignment.Center;
			cell.SetValue(Grid.RowProperty, row);
			cell.SetValue(Grid.ColumnProperty, column);
            cell.Tag = tag;
			cell.Height = 24;
            cell.MouseLeftButtonDown += (s, e) => 
            {
                if (null != tag)
                    MessageBox.Show(tag.ToString());
            };
            if (!isHeader)
                cell.Style = App.CellStyle;
            return cell;
		}

		private static RowDefinition CreateRow()
		{
			RowDefinition rd = new RowDefinition();
			rd.Height = new System.Windows.GridLength(0.0, GridUnitType.Auto);
			return rd;
		}

		private static ColumnDefinition CreateColumn()
		{
			return new ColumnDefinition();
		}

		public static Grid BuildOlapView(SQLiteConnection conn, string tableName, OlapStage stage)
		{
			using (TempTable olapStage = new TempTable(conn, QueryBuilder.BuildOlapStage(tableName, stage)))
			{
				OlapDimensionsTree leftTree = BuildDimensionsTree(olapStage, stage.left);
				OlapDimensionsTree topTree = BuildDimensionsTree(olapStage, stage.top);

				bool bLeftTotals = true;
				bool bTopTotals = true;
				bool bGrandTotals = true;
				
				Grid grid = new Grid();
				grid.ShowGridLines = true;

				foreach (var d in stage.left)
					grid.ColumnDefinitions.Add(CreateColumn());
				if (bLeftTotals || bGrandTotals)
					grid.ColumnDefinitions.Add(CreateColumn());

				foreach (var d in stage.top)
					grid.RowDefinitions.Add(CreateRow());
				if (bTopTotals || bGrandTotals)
					grid.RowDefinitions.Add(CreateRow());

				leftTree.IterateLeaves((n, f) => grid.RowDefinitions.Add(CreateRow()));

				topTree.IterateLeaves((n, f) => grid.ColumnDefinitions.Add(CreateColumn()));

				int nStartRow = topTree.Height - 1;
				int nStartColumn = leftTree.Height - 1;

				// ... ttt
				int row = nStartRow;
				int column = nStartColumn - 1;
				int nLeftTotals = leftTree.IterateLeaves((n, f) => grid.Children.Add(CreateCell(row++, column, n.value, true, new OlapCellInfo(n, null))));

				row = nStartRow - 1;
				column = nStartColumn;
				int nTopTotals = topTree.IterateLeaves((n, f) => grid.Children.Add(CreateCell(row, column++, n.value, true, new OlapCellInfo(null, n))));

				long[] leftTotals = new long[nLeftTotals];
				long[] topTotals = new long[nTopTotals];
				long grandTotals = 0;

				row = nStartRow;
				leftTree.IterateLeaves((leftValue, leftFilters) =>
				{
					column = nStartColumn;
					topTree.IterateLeaves((topValue, topFilters) =>
					{
						long val = olapStage.SelectData(leftFilters, topFilters, stage.aggregations[0]);
						if (0 != val)
						{
							if (bLeftTotals)
								leftTotals[row - nStartRow] = leftTotals[row - nStartRow] + val;
							if (bTopTotals)
								topTotals[column - nStartColumn] = topTotals[column - nStartColumn] + val;
							if (bGrandTotals)
								grandTotals += val;
							grid.Children.Add(CreateCell(row, column, val.ToString(), false, new OlapCellInfo(leftValue, topValue)));
						}
						++column;
					});
					++row;
				});

				if (bLeftTotals)
					for (row = 0; row < nLeftTotals; ++row)
						grid.Children.Add(CreateCell(nStartRow + row, nStartColumn + nTopTotals, leftTotals[row].ToString(), false, null));

				if (bTopTotals)
					for (column = 0; column < nTopTotals; ++column)
						grid.Children.Add(CreateCell(nStartRow + nLeftTotals, nStartColumn + column, topTotals[column].ToString(), false, null));

				if (bGrandTotals)
					grid.Children.Add(CreateCell(nStartRow + nLeftTotals, nStartColumn + nTopTotals, grandTotals.ToString(), false, null));

				return grid;
			}
		}
	}
}
