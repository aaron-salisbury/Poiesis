   M S B A M L   `   `   `  ����   Poiesis)    #Poiesis.View.DatabaseAttributesView   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008       � DataContext  �� *DatabaseAttributesVM+   Locator �� Source# [  .���  � RowDefinitions �� $
��Auto ��� $��10 ��� $��* ��� $��10 ��� $
��Auto ��� $��15 ��� $
��Auto � � ColumnDefinitions �� $��* �.+�� $��0��$��0�� �� $
��Auto ��� $
��Auto � �� $��* �.+��� $��New Database Name��$��10(�$��0��$��0���� $��0��$��1��$��14(����� *NewDatabaseName+ �� Mode$ TwoWay=� �� UpdateSourceTrigger$ PropertyChanged=�� $��2��$��0�� �� $
��Auto ��� $��* � �� $��* �.+��� $��0��$��0��$��SQL Server System Files��$��10(�&   System.Windows.Controls.DataGrid  $��0��$��1��   
IsReadOnly .   LightAccentAppColorBrush#��[ ���� *SQLServerSystemFiles+$ OneWay=�$ PropertyChanged=� � 	Resources *  $System.Windows.Controls.DataGridCell �� 
TargetType	 �� Triggers
 P� Property P� Value �� Property  PrimaryAppColorBrush �� Value��%f   '        7  7  �� "  	 P� 
 �@ 
IsSelected . . ���  � ��8   ��  � ��8  � $��4��$��0�� �� $
��Auto ��� $
��Auto � �� $
��AUTO ��� $��* �.+��� $��0��$��0��$��Transfer Type��$��10(��� $��1��$��0������ *TransferTypes+ �� SelectedIndex �� *SelectedTransferTypeIndex+�� $��6��$��0��$��Right=�$
��Next��#��[   DarkAccentAppColorBrush#��[ $��16(�$��75��
�������{��� *CanRunProcess+$ OneWay=����� *RunProcessCommand+