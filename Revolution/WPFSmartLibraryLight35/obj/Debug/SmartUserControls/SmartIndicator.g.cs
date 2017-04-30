﻿#pragma checksum "..\..\..\SmartUserControls\SmartIndicator.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "4BBE26F65E94D5C64EC2134632B28849"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

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


namespace SoftArcs.WPFSmartLibrary.SmartUserControls {
    
    
    /// <summary>
    /// SmartIndicator
    /// </summary>
    public partial class SmartIndicator : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 1 "..\..\..\SmartUserControls\SmartIndicator.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal SoftArcs.WPFSmartLibrary.SmartUserControls.SmartIndicator VisualRoot;
        
        #line default
        #line hidden
        
        
        #line 32 "..\..\..\SmartUserControls\SmartIndicator.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Border LowIndicator;
        
        #line default
        #line hidden
        
        
        #line 34 "..\..\..\SmartUserControls\SmartIndicator.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Border MiddleIndicator;
        
        #line default
        #line hidden
        
        
        #line 35 "..\..\..\SmartUserControls\SmartIndicator.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Border HighIndicator;
        
        #line default
        #line hidden
        
        /// <summary>
        /// Indicator Name Field
        /// </summary>
        
        #line 42 "..\..\..\SmartUserControls\SmartIndicator.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        public System.Windows.Shapes.Path Indicator;
        
        #line default
        #line hidden
        
        
        #line 45 "..\..\..\SmartUserControls\SmartIndicator.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Media.TranslateTransform IndicatorTranslateTransform;
        
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
            System.Uri resourceLocater = new System.Uri("/WPFSmartLibraryLight35;component/smartusercontrols/smartindicator.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\SmartUserControls\SmartIndicator.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
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
            this.VisualRoot = ((SoftArcs.WPFSmartLibrary.SmartUserControls.SmartIndicator)(target));
            
            #line 7 "..\..\..\SmartUserControls\SmartIndicator.xaml"
            this.VisualRoot.Loaded += new System.Windows.RoutedEventHandler(this.SmartIndicator_Loaded);
            
            #line default
            #line hidden
            
            #line 7 "..\..\..\SmartUserControls\SmartIndicator.xaml"
            this.VisualRoot.SizeChanged += new System.Windows.SizeChangedEventHandler(this.SmartIndicator_SizeChanged);
            
            #line default
            #line hidden
            return;
            case 2:
            this.LowIndicator = ((System.Windows.Controls.Border)(target));
            return;
            case 3:
            this.MiddleIndicator = ((System.Windows.Controls.Border)(target));
            return;
            case 4:
            this.HighIndicator = ((System.Windows.Controls.Border)(target));
            return;
            case 5:
            this.Indicator = ((System.Windows.Shapes.Path)(target));
            return;
            case 6:
            this.IndicatorTranslateTransform = ((System.Windows.Media.TranslateTransform)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

