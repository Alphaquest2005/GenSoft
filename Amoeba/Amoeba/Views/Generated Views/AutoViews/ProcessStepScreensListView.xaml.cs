﻿using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ViewModels;


namespace Views
{
/// <summary>
	/// Interaction logic for ProcessStepScreens.xaml
	/// </summary>
	public partial class ProcessStepScreensAutoViewList_AutoGen
	{
		public ProcessStepScreensAutoViewList_AutoGen()
		{
			try
			{
				this.InitializeComponent();
				im = this.FindResource("ProcessStepScreensAutoViewModelDataSource") as ProcessStepScreensAutoViewModel_AutoGen;
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message + ex.StackTrace);
			}
		}
		ProcessStepScreensAutoViewModel_AutoGen im;

	}
}
