   M S B A M L   `   `   `  ����C  ?Whatsapp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullC&clr-namespace:Whatsapp.Views.ViewPagesWhatsapp.Views.ViewPages  5      .    (Whatsapp.Views.ViewPages.ViewNewPassword   5      W SPresentationCore, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      8local&clr-namespace:Whatsapp.Views.ViewPages     5      _ [MaterialDesignThemes.Wpf, Version=4.9.0.0, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5
      -   5        $
��page��5        N� Title$  ViewNewPassword��5      .��5      ��  �� CornerRadius$	 100��6   ��5   
   �� 5       �� ImageSource$ \Images\background.jpg��6   ._�5   
   � .+�5      �� $��40,40q�6f   $
��Left=�6v   $	��Top=�6�      MaterialDesignIconButton#��[  5       � ToolTip$ MaterialDesignIconButton��5      ��5      �� *GoBackCommand+��67   ��  �� ElementName$
 page��{�5      �� *DataContext.ControlsEnabled+ �� RelativeSource �� *FindAncestor+ �� AncestorType
 Window.��5      '  !MaterialDesignThemes.Wpf.PackIcon  $��50��6*   $��40��66   
��� � �   Kind$ 	ArrowBack=�5      5      �� 5      $��40,40q�6e   $��Right=�6u   $	��Top=�6�   #��[  5      $ MaterialDesignIconButton��5      ��5      �� *CloseCommand+��66   �� $
 page��{�5      �� *DataContext.ControlsEnabled+ �� *FindAncestor+
 Window.��5        $��50��6*   $��40��66   
��� � �$ Close=�5      5      �� 5        J� Opacity$ 50Q�6   $��Center=�6'   $	��500��6B   $	��400��6O   
������ $��350,104,350,0q�6u   .+�5"      G� -   5        	 G�Name$	 CodeOne��5"       # MaterialDesignFilledPasswordBox#��[ 6/   $��30(�5#   %   $��Center=�63   
�������
��� � �$��5q�6}   $��80��6�   $	��300��6�   )  #MaterialDesignThemes.Wpf.HintAssist
   Hint$
 New Pass��5$   %      
Foreground
 � � �5%   %   G� 5&      -   5        $	 CodeTwo��5&      #��[ 6/   $��30(�5'      $��Center=�6$   
�������
��� � �$��5q�6n   $��80��6z   $	��300��6�   $
 Confirm Pass��5(      
 � � �5)      �� 5+      $��0,20,0,0q�5-      $��80��6   $��30(�6#   $	��300��61   $��0q�6=   
���  ��  MaterialDesignRaisedButton#��[ 5.      3  -MaterialDesignThemes.Wpf.ButtonProgressAssist   Value$ -1Q�5/         IsIndicatorVisible$
 True��50         IsIndeterminate$
 True��51      $��Confirm��52      ��5+      �� *ConfirmCommand+��5,      �� $
 page��{�54      �� *DataContext.ControlsEnabled+ �� *FindAncestor+
 Window55      56      57      58      