﻿#pragma checksum "..\..\..\..\Generated Views\ExamDetails\SummaryViewDetails_AutoGen.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "774C795F42BA3B2FC754A2500C545BED"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Core.Common.UI;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;
using ViewModels;
using Views;


namespace Views {
    
    
    /// <summary>
    /// ExamDetailsSummaryDetailsView
    /// </summary>
    public partial class ExamDetailsSummaryDetailsView : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 48 "..\..\..\..\Generated Views\ExamDetails\SummaryViewDetails_AutoGen.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid LayoutRoot;
        
        #line default
        #line hidden
        
        
        #line 51 "..\..\..\..\Generated Views\ExamDetails\SummaryViewDetails_AutoGen.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal Core.Common.UI.SliderPanel Slider;
        
        #line default
        #line hidden
        
        
        #line 61 "..\..\..\..\Generated Views\ExamDetails\SummaryViewDetails_AutoGen.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Expander ComponentsListEXP;
        
        #line default
        #line hidden
        
        
        #line 99 "..\..\..\..\Generated Views\ExamDetails\SummaryViewDetails_AutoGen.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Expander ExamDetailsListEXP;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/Views;component/generated%20views/examdetails/summaryviewdetails_autogen.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\Generated Views\ExamDetails\SummaryViewDetails_AutoGen.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal System.Delegate _CreateDelegate(System.Type delegateType, string handler) {
            return System.Delegate.CreateDelegate(delegateType, this, handler);
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.LayoutRoot = ((System.Windows.Controls.Grid)(target));
            return;
            case 2:
            this.Slider = ((Core.Common.UI.SliderPanel)(target));
            return;
            case 3:
            this.ComponentsListEXP = ((System.Windows.Controls.Expander)(target));
            return;
            case 4:
            
            #line 72 "..\..\..\..\Generated Views\ExamDetails\SummaryViewDetails_AutoGen.xaml"
            ((System.Windows.Controls.Grid)(target)).MouseEnter += new System.Windows.Input.MouseEventHandler(this.BringIntoView);
            
            #line default
            #line hidden
            return;
            case 5:
            this.ExamDetailsListEXP = ((System.Windows.Controls.Expander)(target));
            return;
            case 6:
            
            #line 110 "..\..\..\..\Generated Views\ExamDetails\SummaryViewDetails_AutoGen.xaml"
            ((System.Windows.Controls.Grid)(target)).MouseEnter += new System.Windows.Input.MouseEventHandler(this.BringIntoView);
            
            #line default
            #line hidden
            return;
            case 7:
            
            #line 143 "..\..\..\..\Generated Views\ExamDetails\SummaryViewDetails_AutoGen.xaml"
            ((System.Windows.Controls.TextBlock)(target)).MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(this.GoToComponents);
            
            #line default
            #line hidden
            return;
            case 8:
            
            #line 145 "..\..\..\..\Generated Views\ExamDetails\SummaryViewDetails_AutoGen.xaml"
            ((System.Windows.Controls.TextBlock)(target)).MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(this.GoToExamDetails);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

