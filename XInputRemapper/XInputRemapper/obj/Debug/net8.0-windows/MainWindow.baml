   M S B A M L   `   `   `  ����  XInputRemapper    XInputRemapper.MainWindow   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   \ XHardcodet.Wpf.TaskbarNotification, Version=1.0.5.0, Culture=neutral, PublicKeyToken=null)tb http://www.hardcodet.net/taskbar  5   	     � Title$  XInput Remapper��5   	   $	��746��6!   $	��926��6.    � 	Resources 5         TrayMenu m� Command��%�   &	        �� 5   
   .@�5	      m� $
��Show��6    6%   �� *ShowWindowCommand+m� 5
      $
��Exit��6    6%   �� *ExitApplicationCommand+5      5      .��5      ]� .+�5   
   3  -Hardcodet.Wpf.TaskbarNotification.TaskbarIcon  -   5          Name$ MyNotifyIcon��5         
IconSource$8 2pack://application:,,,/Icon/bracke_forest_logo.ico��5         ToolTipText$ XInput Remapper��5       � ContextMenu# [  5      �� 5   
   $
��Auto=�6   $
��Auto=�6:   .��5      � $��Bottom=�6   $��10q�6+    � RowDefinitions 5      �� 5      $
��Auto �6$   �� 5      $��* �6$    � ColumnDefinitions 5      �� 5      $��* �6'   �� 5      $��* �6'   .+�5"      �� $��0��6   $��0��6*   	 �� Orientation$	 Vertical=�6:   $��10q�6Q   .+�5$      �� $	 
Horizontal=�6!   $��0,0,0,10q�6:   .+�5%      �� $��Center=�6$   $��16(�6?   $��0,0,10,0q�6M   $��Select Controller:��6_   �� 5&      -   5        
 ��Name$
 ControllerComboBox��5&   #   $	��200��6?   $��45,0,0,0q�6�   .@�5'      �� $��Xbox Controller��6+   �� 5(      $	��MD4��6+   �� 5)      $
��MD10��6+   5*      5+      �� 5.      $	 
Horizontal=�6!   $��0,0,0,10q�6:   .+�5/      �� $��Center=�6$   $��16(�6?   $��0,0,10,0q�6M   $��Select Controller Index:��6_   �� 50      -   5        $
 ControllerIndexComboBox��50   #   $	��200��6D   $��2,0,0,0q�6�   .@�51      �� $��Controller 1��6+   �� 52      $��Controller 2��6+   �� 53      $��Controller 3��6+   �� 54      $��Controller 4��6+   55      56      �� 57      -   5        $��SaveProfileButton��57      $��Save Profile��66   $
��Left=�6M   $	��Top=�6h   $	��100��6�   $��10q�6�   6�   �� 58      -   5        $��LoadProfileButton��58      $��Load Profile��66   $
��Left=�6M   $	��Top=�6h   $	��100��6�   $��	10,10,0,0q�6�   6�   �� 5;      $	 
Horizontal=�6!   $��0,0,0,10q�6:   .+�5<      �� $��Center=�6$   $��16(�6?   $��0,0,10,0q�6M   $��Remap From:��6_   �� 5=      -   5        $
 RemapFromComboBox��5=   #   $	��200��6>   .@�5>      �� $��A��6+   
 � Tag$ 1��67   �� 5?      $��B��6+   $ 2��67   �� 5@      $��X��6+   $ 3��67   �� 5A      $��Y��6+   $ 4��67   �� 5B      $��DPad_Up��6+   $ 5��6=   �� 5C      $��	DPad_Down��6+   $ 6��6?   �� 5D      $��	DPad_Left��6+   $ 7��6?   �� 5E      $��
DPad_Right��6+   $ 8��6@   �� 5F      $��Start��6+   $ 9��6;   �� 5G      $��Select��6+   $ 10��6<   �� 5H      $
��Back��6+   $ 11��6:   �� 5I      $��
Left_Thumb��6+   $ 12��6@   �� 5J      $��Right_Thumb��6+   $ 13��6A   �� 5K      $��Left_Shoulder��6+   $ 14��6C   �� 5L      $��Right_Shoulder��6+   $ 15��6D   �� 5M      $
��None��6+   $ 16��6:   5N      �� 5O      -   5         ��Name$ RemapFromTextBlock��5O   $   $��16(�6@   $	��200��6N   $��Center=�6Z   $��10,0,0,0q�6w   5P      �� 5S      $	 
Horizontal=�6!   $��0,0,0,10q�6:   .+�5T      �� $��Center=�6$   $��16(�6?   $��0,0,10,0q�6M   $��	Remap To:��6_   �� 5U      -   5        $
 RemapToComboBox��5U   #   $	��200��6<   $��20,0,0,0q�6|   .@�5V      �� $��A��6+   �� 5W      $��B��6+   �� 5X      $��X��6+   �� 5Y      $��Y��6+   �� 5Z      $��DPad_Up��6+   �� 5[      $��	DPad_Down��6+   �� 5\      $��	DPad_Left��6+   �� 5]      $��
DPad_Right��6+   �� 5^      $��Start��6+   �� 5_      $��Select��6+   �� 5`      $
��Back��6+   �� 5a      $��
Left_Thumb��6+   �� 5b      $��Right_Thumb��6+   �� 5c      $��Left_Shoulder��6+   �� 5d      $��Right_Shoulder��6+   �� 5e      $
��None��6+   5f      �� 5g      -	   5        $ RemapToTextBlock��5g   $   $��Center=�6>   $��16(�6Y   $��10,0,0,0q�6g   $	��200��6y   5h      �� 5j      -
   5         ��Name$ ApplyRemapButton��5j      $��Apply Remap��67   $	��150��6M   $
��Left=�6x   �� 5n      -   5         ��Name$ BindingsTextBox��5n      $
��Left=�5o      $��0,10,0,0q�69   $	��400��5p      $	��300��6*    �� 
IsReadOnly$
 True��67    �� TextWrapping$
 Wrap=�6I   �� 5t      -   5        $ ControllerStatusTextBlock��5t       $��16(�6C   $
��Bold"�6Q   $��0,10,0,0q�6c   $��Controller Status: ��6u   5u      �� 5x      -   5         ��Name$ ControllerImage��5x      $��0��61   $	��349��6>   $	��402��6K   $��Uniform=�6W   $
��Left=�6i   $��493,115,0,115q�6�   $��2��6�   5y      5z      5{      5|      