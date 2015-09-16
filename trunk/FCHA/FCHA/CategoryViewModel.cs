using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace FCHA
{
	[DebuggerDisplay("{Name}")]
	public class CategoryViewModel
		: DependencyObject
	{
		private Category m_underlyingData;
		private CategoriesManager m_categoriesManager;
		private CategoryViewModel m_parent;

		public Category UnderlyingData
		{
			get { return m_underlyingData; }
		}

		public static readonly DependencyProperty NameProperty =
			DependencyProperty.Register("Name", typeof(string), typeof(CategoryViewModel));

		public static readonly DependencyProperty ChildrenProperty =
			DependencyProperty.Register("Children", typeof(ObservableCollection<CategoryViewModel>), typeof(CategoryViewModel));

		public string Name
		{
			get { return (string)GetValue(NameProperty); }
			set { SetValue(NameProperty, value); }
		}

		public ObservableCollection<CategoryViewModel> Children
		{
			get { return (ObservableCollection<CategoryViewModel>)GetValue(ChildrenProperty); }
			set { SetValue(ChildrenProperty, value); }
		}

		public CategoryViewModel Parent
		{
			get { return m_parent; }
		}

		public CategoryViewModel(CategoriesManager categoriesManager, CategoryViewModel parent, Category category)
		{
			m_underlyingData = category;
			Name = m_underlyingData.name;
			m_categoriesManager = categoriesManager;
			m_parent = parent;

			Children = new ObservableCollection<CategoryViewModel>(m_categoriesManager.EnumCategoriesByParent(UnderlyingData.categoryId).Select(c => new CategoryViewModel(m_categoriesManager, this, c)));
		}
	}
}
