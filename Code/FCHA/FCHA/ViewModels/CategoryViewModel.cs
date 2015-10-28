﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Collections.ObjectModel;
using System.Diagnostics;
using FCHA.Interfaces;

namespace FCHA
{
	[DebuggerDisplay("{Name}")]
	public class CategoryViewModel
		: DependencyObject
	{
		private Category m_underlyingData;
		private ICategoriesManager m_categoriesManager;
		private CategoryViewModel m_parent;

		public Category UnderlyingData
		{
			get { return m_underlyingData; }
		}

		public long CategoryId
		{
			get { return UnderlyingData.categoryId; }
		}

		public static readonly DependencyProperty NameProperty =
			DependencyProperty.Register("Name", typeof(string), typeof(CategoryViewModel));

        public static readonly DependencyProperty IsIncomeProperty =
            DependencyProperty.Register("IsIncome", typeof(bool), typeof(CategoryViewModel));

        public static readonly DependencyProperty ChildrenProperty =
			DependencyProperty.Register("Children", typeof(ObservableCollection<CategoryViewModel>), typeof(CategoryViewModel));

		public string Name
		{
			get { return (string)GetValue(NameProperty); }
			set { SetValue(NameProperty, value); }
		}

        public bool IsIncome
        {
            get { return (bool)GetValue(IsIncomeProperty); }
            set { SetValue(IsIncomeProperty, value); }
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

		private CategoryViewModel(Category category)
		{
			m_underlyingData = category;
			Name = m_underlyingData.name;
            IsIncome = m_underlyingData.isIncome;
		}

		public CategoryViewModel(ICategoriesManager categoriesManager, CategoryViewModel parent, Category category)
			: this(category)
		{
			m_categoriesManager = categoriesManager;
			m_parent = parent;

            WatchChildren();
		}

        public CategoryViewModel(ICategoriesManager categoriesManager, Category category)
            : this(category)
        {
            m_categoriesManager = categoriesManager;
            
            WatchChildren();
        }

        internal void AdjustParent(AccountancyApplication app)
        {
            m_parent = app.GetCategory(m_underlyingData.categoryId);
        }

        private void WatchChildren()
        {
            Children = new ObservableCollection<CategoryViewModel>(m_categoriesManager.EnumCategoriesByParent(CategoryId).Select(c => new CategoryViewModel(m_categoriesManager, this, c)));
        }

		public override string ToString()
		{
			return Name;
		}

        public void UpdateUnderlyingData()
        {
            m_underlyingData.name = Name;
            m_underlyingData.isIncome = IsIncome;
        }

        public bool IsCovers(CategoryViewModel category)
        {
            CategoryViewModel c = category;
            while (null != c)
            {
                if (c.CategoryId == CategoryId)
                    return true;
                c = c.Parent;
            }
            return false;
        }

        public int Level
        {
            get
            {
                int rv = 0;
                CategoryViewModel cvm = Parent;
                while (null != cvm)
                {
                    ++rv;
                    cvm = cvm.Parent;
                }
                return rv;
            }
        }
    }
}
