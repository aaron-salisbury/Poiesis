   M S B A M L   `   `   `  ����  Poiesis.App6    0Poiesis.App.Views.Workflow.SourceDBSelectionView   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5        � DataContext  5      �� *SourceDBSelectionViewModel+   Locator �� Source# [  .��5
      �  � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $��* �6   �� 5      $
��Auto �6   .+�5   
   �� $��0��6    �� Orientation$ Vertical=�6"   .+�5      �� $��Server Name��6   �� 5      $ 
Horizontal=�6   .+�5      �� $	��342��6Z   ��6   �� *#!Manager.SourceDatabase.DataSource+ �� Mode$ TwoWay=�6Z   �� 5      $
��Load��6   $��5,0,0,0q�6Q   ��6(   �� *LoadDatabasesCommand+6Q   5      �� 5      $��Select Local Database��6   $��0,32,0,0q�64   5      �� 5   
   $��1��6   $��Visible=�5      $��1q�6E   ��5      �� *Manager.LocalDatabaseNames+$ TwoWay=� �� SelectedItem 6_   �� *SelectedLocalDatabaseName+$ TwoWay=�5   E   �� 5   
   $��2��6   $ Vertical=�6"   .+�5      �  � Minimum$ 0Q�6    � Maximum$ 1Q�6&   $��16��62    � IsIndeterminate . y�6U   �� *ProgressBarVisibility+�� 5!      $
��Next��5"      $
��Left=�5%      $��0,5,0,0q�5&      {�5#      �� *CanRunProcess+$ TwoWay=���5$      �� *" DisplayDatabaseAttributesCommand+5&      5'      5(      5)      