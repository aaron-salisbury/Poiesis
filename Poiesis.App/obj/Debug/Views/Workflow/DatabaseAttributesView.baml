   M S B A M L   `   `   `  ����  Poiesis.App7    1Poiesis.App.Views.Workflow.DatabaseAttributesView   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5        � DataContext  5      �� *DatabaseAttributesViewModel+   Locator �� Source# [  .��5
      �  � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $��* �6   �� 5      $
��Auto �6   .+�5   
   �� $��0��6    �� Orientation$ Vertical=�6"   .+�5      �� $��New Database Name��6   �� 5      $
��Left=�6i   $	��410��6�   ��6   �� *NewDatabaseName+ �� Mode$ TwoWay=� �� UpdateSourceTrigger$ PropertyChanged=�6�   �� 5      $��Transfer Type��6   $��0,16,0,0q�6,   �� 5       �� SelectedValuePath$ Value��5       �� DisplayMemberPath$
 Text��5      $
��Left=�5      $	��200��5      ��5      �� *TransferTypes+$ TwoWay=� �� SelectedItem 5      �� *SelectedTransferType+$ TwoWay=�$ PropertyChanged=�5      �� 5      $��SQL Server System Files��6   $��0,16,0,0q�66   5      &   System.Windows.Controls.DataGrid  5!   
   $��1��6   	   
IsReadOnly	 . $��1q�6l   
   GridLinesVisibility$	
 All=�6�   ��6    �� *SQLServerSystemFiles+$ OneWay=� � 	Resources 5"      *  $System.Windows.Controls.DataGridCell �� 
TargetType �� Triggers P� Property P� Value �� Property �� Value��%�   '        �� 5#      "  6    5$      P� 5%       �@ 
IsSelected . . �5&      ��  � ��
 ���w�6;   �� 5'       � ��
 ���w�6<   5(      5*      5+      5,      �� 5.   
   $��2��6   $ Vertical=�6"   $��0,5,0,0q�69   .+�5/      �  � Minimum$ 0Q�6    � Maximum$ 1Q�6&   $��16��62    � IsIndeterminate . y�6U   �� *ProgressBarVisibility+�� 51      $	��Run��52      $
��Left=�55      {�53      �� *CanRunProcess+$ TwoWay=���54      �� *RunProcessCommand+55      56      57      58      