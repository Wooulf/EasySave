﻿#pragma checksum "..\..\..\..\View\ProgressionWindow.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "A0D4C1B5F26566D4FF151611AFBF4D191F75DF9E"
//------------------------------------------------------------------------------
// <auto-generated>
//     Ce code a été généré par un outil.
//     Version du runtime :4.0.30319.42000
//
//     Les modifications apportées à ce fichier peuvent provoquer un comportement incorrect et seront perdues si
//     le code est régénéré.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Controls.Ribbon;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms.Integration;
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
using WpfApp3;


namespace WpfApp3.View {
    
    
    /// <summary>
    /// ProgressionWindow
    /// </summary>
    public partial class ProgressionWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 12 "..\..\..\..\View\ProgressionWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label Progression;
        
        #line default
        #line hidden
        
        
        #line 13 "..\..\..\..\View\ProgressionWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ProgressBar progressDownloadFiles;
        
        #line default
        #line hidden
        
        
        #line 14 "..\..\..\..\View\ProgressionWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnConfirmProgression;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "6.0.1.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/WpfApp3;V1.0.0.0;component/view/progressionwindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\View\ProgressionWindow.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "6.0.1.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            
            #line 7 "..\..\..\..\View\ProgressionWindow.xaml"
            ((WpfApp3.View.ProgressionWindow)(target)).Closing += new System.ComponentModel.CancelEventHandler(this.ProgressionWindow_Closing);
            
            #line default
            #line hidden
            return;
            case 2:
            this.Progression = ((System.Windows.Controls.Label)(target));
            return;
            case 3:
            this.progressDownloadFiles = ((System.Windows.Controls.ProgressBar)(target));
            
            #line 13 "..\..\..\..\View\ProgressionWindow.xaml"
            this.progressDownloadFiles.ValueChanged += new System.Windows.RoutedPropertyChangedEventHandler<double>(this.progressDownloadFiles_ValueChanged);
            
            #line default
            #line hidden
            return;
            case 4:
            this.btnConfirmProgression = ((System.Windows.Controls.Button)(target));
            
            #line 14 "..\..\..\..\View\ProgressionWindow.xaml"
            this.btnConfirmProgression.Click += new System.Windows.RoutedEventHandler(this.OnConfirmProgression);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

