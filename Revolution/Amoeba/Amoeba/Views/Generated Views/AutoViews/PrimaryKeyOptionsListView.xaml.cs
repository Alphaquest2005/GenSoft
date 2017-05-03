﻿using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ViewModels;


namespace Views
{
/// <summary>
	/// Interaction logic for PrimaryKeyOptions.xaml
	/// </summary>
	public partial class PrimaryKeyOptionsAutoViewList_AutoGen
	{
		public PrimaryKeyOptionsAutoViewList_AutoGen()
		{
			try
			{
				this.InitializeComponent();
				im = this.FindResource("PrimaryKeyOptionsAutoViewModelDataSource") as PrimaryKeyOptionsAutoViewModel_AutoGen;
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message + ex.StackTrace);
			}
		}
		PrimaryKeyOptionsAutoViewModel_AutoGen im;

	}
}
