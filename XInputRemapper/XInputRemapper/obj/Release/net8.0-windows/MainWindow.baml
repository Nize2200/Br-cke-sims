   M S B A M L   `   `   `  ����   XInputRemapper    XInputRemapper.MainWindow   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     \ XHardcodet.Wpf.TaskbarNotification, Version=1.0.5.0, Culture=neutral, PublicKeyToken=null)tb http://www.hardcodet.net/taskbar    � Title$  XInput Remapper��$	��746��$	��926�� � 	Resources    TrayMenu m� Command��%u   &	        �� .@�m� $
��Show�� �� *ShowWindowCommand+m� $
��Exit�� �� *ExitApplicationCommand+.��]� .+�3  -Hardcodet.Wpf.TaskbarNotification.TaskbarIcon  -     Name$ MyNotifyIcon��   
IconSource$8 2pack://application:,,,/Icon/bracke_forest_logo.ico��   ToolTipText$ XInput Remapper�� � ContextMenu# [  �� $
��Auto=�$
��Auto=�.��� $��Bottom=�$��10q� � RowDefinitions �� $
��Auto ��� $��* � � ColumnDefinitions �� $��* ��� $��* �.+��� $��0��$��0��	 �� Orientation$	 Vertical=�$��10q�.+��� $	 
Horizontal=�$��0,0,0,10q�.+��� $��Center=�$��16(�$��0,0,10,0q�$��Select Controller:���� -   
 ��Name$
 ControllerComboBox��$	��200��$��45,0,0,0q�.@��� $��Xbox Controller���� $	��MD4���� $
��MD10���� $	 
Horizontal=�$��0,0,0,10q�.+��� $��Center=�$��16(�$��0,0,10,0q�$��Select Controller Index:���� -   $
 ControllerIndexComboBox��$	��200��$��2,0,0,0q�.@��� $��Controller 1���� $��Controller 2���� $��Controller 3���� $��Controller 4���� -   $��SaveProfileButton��$��Save Profile��$
��Left=�$	��Top=�$	��100��$��10q��� -   $��LoadProfileButton��$��Load Profile��$
��Left=�$	��Top=�$	��100��$��	10,10,0,0q��� $	 
Horizontal=�$��0,0,0,10q�.+��� $��Center=�$��16(�$��0,0,10,0q�$��Remap From:���� -   $
 RemapFromComboBox��$	��200��.@��� $��A��
 � Tag$ 1���� $��B��$ 2���� $��X��$ 3���� $��Y��$ 4���� $��DPad_Up��$ 5���� $��	DPad_Down��$ 6���� $��	DPad_Left��$ 7���� $��
DPad_Right��$ 8���� $��Start��$ 9���� $��Select��$ 10���� $
��Back��$ 11���� $��
Left_Thumb��$ 12���� $��Right_Thumb��$ 13���� $��Left_Shoulder��$ 14���� $��Right_Shoulder��$ 15���� $
��None��$ 16���� -    ��Name$ RemapFromTextBlock��$��16(�$	��200��$��Center=�$��10,0,0,0q��� $	 
Horizontal=�$��0,0,0,10q�.+��� $��Center=�$��16(�$��0,0,10,0q�$��	Remap To:���� -   $
 RemapToComboBox��$	��200��$��20,0,0,0q�.@��� $��A���� $��B���� $��X���� $��Y���� $��DPad_Up���� $��	DPad_Down���� $��	DPad_Left���� $��
DPad_Right���� $��Start���� $��Select���� $
��Back���� $��
Left_Thumb���� $��Right_Thumb���� $��Left_Shoulder���� $��Right_Shoulder���� $
��None���� -	   $ RemapToTextBlock��$��Center=�$��16(�$��10,0,0,0q�$	��200���� -
    ��Name$ ApplyRemapButton��$��Apply Remap��$	��150��$
��Left=��� -    ��Name$ BindingsTextBox��$
��Left=�$��0,10,0,0q�$	��400��$	��300�� �� 
IsReadOnly$
 True�� �� TextWrapping$
 Wrap=��� -   $ ControllerStatusTextBlock��$��16(�$
��Bold"�$��0,10,0,0q�$��Controller Status: ���� -    ��Name$ ControllerImage��$��0��$	��349��$	��402��$��Uniform=�$
��Left=�$��493,115,0,115q�$��2��