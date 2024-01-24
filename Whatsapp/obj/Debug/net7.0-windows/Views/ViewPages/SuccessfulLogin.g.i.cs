﻿#pragma checksum "..\..\..\..\..\Views\ViewPages\SuccessfulLogin.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "8A612673D6E98E255FA640023CFAF9A177AF4FC6"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using MaterialDesignThemes.Wpf;
using MaterialDesignThemes.Wpf.Converters;
using MaterialDesignThemes.Wpf.Transitions;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Controls.Ribbon;
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
using Whatsapp.Views.ViewPages;


namespace Whatsapp.Views.ViewPages {
    
    
    /// <summary>
    /// SuccessfulLogin
    /// </summary>
    public partial class SuccessfulLogin : System.Windows.Controls.Page, System.Windows.Markup.IComponentConnector {
        
        
        #line 10 "..\..\..\..\..\Views\ViewPages\SuccessfulLogin.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal Whatsapp.Views.ViewPages.SuccessfulLogin page;
        
        #line default
        #line hidden
        
        
        #line 20 "..\..\..\..\..\Views\ViewPages\SuccessfulLogin.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid AllList;
        
        #line default
        #line hidden
        
        
        #line 77 "..\..\..\..\..\Views\ViewPages\SuccessfulLogin.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button SelectedUserBtn;
        
        #line default
        #line hidden
        
        
        #line 83 "..\..\..\..\..\Views\ViewPages\SuccessfulLogin.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button OnlyChatUsers;
        
        #line default
        #line hidden
        
        
        #line 100 "..\..\..\..\..\Views\ViewPages\SuccessfulLogin.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button AllUsers;
        
        #line default
        #line hidden
        
        
        #line 115 "..\..\..\..\..\Views\ViewPages\SuccessfulLogin.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListView list;
        
        #line default
        #line hidden
        
        
        #line 161 "..\..\..\..\..\Views\ViewPages\SuccessfulLogin.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListView list2;
        
        #line default
        #line hidden
        
        
        #line 206 "..\..\..\..\..\Views\ViewPages\SuccessfulLogin.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox messageText;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "7.0.11.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/Whatsapp;component/views/viewpages/successfullogin.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\..\Views\ViewPages\SuccessfulLogin.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "7.0.11.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.page = ((Whatsapp.Views.ViewPages.SuccessfulLogin)(target));
            return;
            case 2:
            this.AllList = ((System.Windows.Controls.Grid)(target));
            return;
            case 3:
            this.SelectedUserBtn = ((System.Windows.Controls.Button)(target));
            return;
            case 4:
            this.OnlyChatUsers = ((System.Windows.Controls.Button)(target));
            return;
            case 5:
            this.AllUsers = ((System.Windows.Controls.Button)(target));
            return;
            case 6:
            this.list = ((System.Windows.Controls.ListView)(target));
            
            #line 115 "..\..\..\..\..\Views\ViewPages\SuccessfulLogin.xaml"
            this.list.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.selected);
            
            #line default
            #line hidden
            return;
            case 7:
            this.list2 = ((System.Windows.Controls.ListView)(target));
            return;
            case 8:
            this.messageText = ((System.Windows.Controls.TextBox)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

