﻿// <autogenerated>
//   This file was generated by T4 code generator Amoeba-Master.tt.
//   Any changes made to this file manually will be lost next time the file is regenerated.
// </autogenerated>


using System.Windows.Input;
using Core.Common.UI;

namespace ViewModels
{
	public class MainWindowViewModel 
	{
		 private static readonly MainWindowViewModel instance;
		 static MainWindowViewModel()
		{
			instance = new MainWindowViewModel();
		}

		 public static MainWindowViewModel Instance
		{
			get { return instance; }
		}

		public MainWindowViewModel()
		{
			this.navMREntitiesQS = new RelayCommand(OnNavMREntitiesQS);
	            
		}

		private RelayCommand navMREntitiesQS;
		private void OnNavMREntitiesQS()
		{
			Core.Common.UI.AppSlider.Slider.MoveTo("MREntitiesQSEXP"); 
		}

		public ICommand NavMREntitiesQS
		{
			get { return this.navMREntitiesQS; }
		}

	 
		
	}
}
