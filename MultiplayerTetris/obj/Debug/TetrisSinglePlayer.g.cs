﻿

#pragma checksum "C:\Users\dnshe_000\documents\visual studio 2012\Projects\MultiplayerTetris\MultiplayerTetris\TetrisSinglePlayer.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "201951E7531401A63E30348D1E81930E"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MultiplayerTetris
{
    partial class TetrisSinglePlayer : global::MultiplayerTetris.Common.LayoutAwarePage, global::Windows.UI.Xaml.Markup.IComponentConnector
    {
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
 
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 1:
                #line 11 "..\..\TetrisSinglePlayer.xaml"
                ((global::Windows.UI.Xaml.FrameworkElement)(target)).Loaded += this.pageRoot_Loaded;
                 #line default
                 #line hidden
                break;
            case 2:
                #line 24 "..\..\TetrisSinglePlayer.xaml"
                ((global::Windows.UI.Xaml.UIElement)(target)).PointerPressed += this.Grid_PointerPressed_1;
                 #line default
                 #line hidden
                break;
            case 3:
                #line 57 "..\..\TetrisSinglePlayer.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.Button_Click_1;
                 #line default
                 #line hidden
                break;
            case 4:
                #line 58 "..\..\TetrisSinglePlayer.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.Button_Click_2;
                 #line default
                 #line hidden
                break;
            case 5:
                #line 60 "..\..\TetrisSinglePlayer.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.RangeBase)(target)).ValueChanged += this.levelSlider_ValueChanged;
                 #line default
                 #line hidden
                break;
            case 6:
                #line 50 "..\..\TetrisSinglePlayer.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.drop_Click;
                 #line default
                 #line hidden
                break;
            case 7:
                #line 38 "..\..\TetrisSinglePlayer.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.GoBack;
                 #line default
                 #line hidden
                break;
            }
            this._contentLoaded = true;
        }
    }
}


