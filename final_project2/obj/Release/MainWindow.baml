   M S B A M L   `   `   `  ���� O  Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0891&clr-namespace:System;assembly=mscorlibSystem   final_project2   final_project2.MainWindow   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     4c&clr-namespace:System;assembly=mscorlib     X TXceed.Wpf.Toolkit, Version=2.5.0.0, Culture=neutral, PublicKeyToken=3e4669d2f30244f44xctk)http://schemas.xceed.com/wpf/xaml/toolkit  -   $��main_window��  � Title$  Timeline�� � WindowState$ 	Maximized=� � 	Resources    str1��%   &	        ��	Hello��.��� -   $��grid1�� � RowDefinitions �� $��* ��� $��50 � � ContextMenu �� .@�m� -   $��create_item��$��Create��.+��� -    ��Name$ zoomAndPanControl��
�������.���� -   $��front_canvas��$��1��$��0��z��� -   st������  �� EndPoint$ 0.5,1&� �� 
StartPoint$ 0.5,0&�.���  � Color$ Black��	 � Offset$	 0Q�� $ 	#FF8FB8FF��$	 1Q�� $��1��
 � ColumnDefinitions
 �� $��* ��� $��50 ��� $	��450 �.+��� $��0��
����+�� J� Opacity$	 0.5Q��� -   $��AddTodoList��$��1��$��+���� -   $��front_canvas2��$��2��
�������� -	    �Name$ Todo_box��$y�Hidden=�.+�� 
���   �$	 0.5Q��� $	��300�����	#FFE6E6E6
���   �$��1q� �� CornerRadius$ 6,6,6,6��$��Center=�$��Center=�._��� .+��� $��5q�$��Create a Timeline:��$
��Bold"�$��Cambria)��� $��0q�$
��Bold"�$��Cambria)�$��Center=�$��Center=��� $�Name  ���� .F��� -
    ��Name$ InputTextBox2��$	��150��$��Center=�$��Center=�����  �� Path$ activity_name���  �� Orientation$ 
Horizontal=�$��Center=�.+��� -   $��btnOpenFile2��$��80��$��Center=�.��	Open file�� -   $ PathTextBox2��$	��160��$��Center=�$��Center=����� $ activity_name��� -   $��btnOpenWeb2��$��80��$��Center=�.��Open website�� $��0q�$
��Bold"�$��Cambria)�$��Center=�$��Center=��� $5�/Copy the website and Paste in the textbox below���� -   $ WebTextBox2��$	��160��$��Center=�$��Center=����� $ activity_name��� $ 
Horizontal=�$��Center=�.+��� -    ��Name$ 
YesButton2��$��5q�$	��Yes����Z� �� -   $ 	NoButton2��$��5q�$��No����Z� � -   $ InputBox��$y�Hidden=�.+�� 
���   �$	 0.5Q��� $	��300�����	#FFE6E6E6
���   �$��1q�$ 6,6,6,6��$��Center=�$��Center=�._��� .+��� $��5q�$��Create a Timeline:��$
��Bold"�$��Cambria)��� $��0q�$
��Bold"�$��Cambria)�$��Center=�$��Center=��� $�Name  ���� .F��� -   $ InputTextBox��$	��150��$��Center=�$��Center=����� $ activity_name��� $ 
Horizontal=�$��Center=�.+��� -   $��btnOpenFile��$��80��$��Center=�.��	Open file�� -   $ PathTextBox��$	��160��$��Center=�$��Center=����� $ activity_name��� -   $��
btnOpenWeb��$��80��$��Center=�.��Open website�� $��0q�$
��Bold"�$��Cambria)�$��Center=�$��Center=��� $5�/Copy the website and Paste in the textbox below���� -   $ 
WebTextBox��$	��160��$��Center=�$��Center=����� $ activity_name��� $��0q�$
��Bold"�$��Cambria)�$��Center=�$��Center=��� $�Start  ���� .F�&   Xceed.Wpf.Toolkit.DateTimeUpDown  -   $��start_timepicker2��$��20��$	��190��   Format$ Custom=��� $��0q�$
��Bold"�$��Cambria)�$��Center=�$��Center=��� $�End  ���� .F�  -   $��end_timepicker2��$��20��$	��190��$ Custom=��� $��0q�$
��Bold"�$��Cambria)�$��Center=�$��Center=��� $�Color  ���� .F��� -   $��color��$��Center=�.@��� ���	#FFFF6464$��50��$��40��$��Center=�.��Red�� ���	#FF6464FF$��50��$��40��$��Center=�.��Blue�� ���	#FF64FF64$��50��$��40��$��Center=�.��Green�� ���	#FFFFFF64$��50��$��40��$��Center=�.��Yellow�� ���	#FFFF64FF$��50��$��40��$��Center=�.��Purple�� $ 
Horizontal=�$��Center=�.+��� -   $ 	YesButton��$��5q�$	��Yes����Z� �� -   $ NoButton��$��5q�$��No����Z� �� -    ��Name$ ActivityBox��$y�Hidden=�$��Center=�$��80��$��Center=�$	��100��.@��� -   .��Edit�� -   .��Delete�� -   .��Exit� -    $ EditBox��$y�Hidden=�.+�� 
���   �$	 0.5Q��� $	��300�����	#FFE6E6E6
���   �$��1q�$ 6,6,6,6��$��Center=�$��Center=�._��� .+��� $��5q�$��Edit a Timeline:��$
��Bold"�$��Cambria)��� $��0q�$
��Bold"�$��Cambria)�$��Center=�$��Center=��� $�Name  ���� .F��� -!   $ EditTextBox��$	��150��$��Center=�$��Center=����� $ activity_name��� $ 
Horizontal=�$��Center=�.+��� -"   $��btnEditOpenFile��$��80��$��Center=�.��	Open file�� -#   $ PathEditTextBox��$	��160��$��Center=�$��Center=����� $ activity_name��� -$   $��btnEditOpenWeb��$��80��$��Center=�.��Open website�� $��0q�$
��Bold"�$��Cambria)�$��Center=�$��Center=��� $5�/Copy the website and Paste in the textbox below���� -%   $ WebEditTextBox��$	��160��$��Center=�$��Center=����� $ activity_name��� $��5q�$
��Bold"�$��Cambria)�$��Center=�$��Center=��� $�Start  ���� .F�  -&   $��start_timepicker��$��20��$	��190��$ Custom=��� $��0q�$
��Bold"�$��Cambria)�$��Center=�$��Center=��� $�End  ���� .F�  -'   $��end_timepicker��$��20��$	��190��$ Custom=��� $��0q�$
��Bold"�$��Cambria)�$��Center=�$��Center=��� $�Color  ���� .F��� -(   $��	editcolor��$��Center=�.@��� ���	#FFFF6464$��50��$��40��$��Center=�.��Red�� ���	#FF6464FF$��50��$��40��$��Center=�.��Blue�� ���	#FF64FF64$��50��$��40��$��Center=�.��Green�� ���	#FFFFFF64$��50��$��40��$��Center=�.��Yellow�� ���	#FFFF64FF$��50��$��40��$��Center=�.��Purple�� $ 
Horizontal=�$��Center=�.+��� -)   $ Act_YesButton��$��5q�$
��Edit����Z� �� -*   $ Act_NoButton��$��5q�$��No����Z� 