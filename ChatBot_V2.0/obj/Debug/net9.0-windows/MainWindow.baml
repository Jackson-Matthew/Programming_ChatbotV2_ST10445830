   M S B A M L   `   `   `  ����G  CChatBot_V2.0, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null-clr-namespace:ChatBot_V2._0ChatBot_V2._0  5	   	       ChatBot_V2._0.MainWindow   5   	   W SPresentationCore, Version=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   -localclr-namespace:ChatBot_V2._0     5	   	     � WindowState$  	Maximized=�5   	    � WindowStyle$ SingleBorderWindow=�5   	    � 
ResizeMode$ 	CanResize=�5   	    � Title$ CABBY��5   	   $	��800��6   $	��600��6#   .��5      ]� .+�5   
   �� -   5         ��Name$ MessagePanel��5      .+�5      (�  (� Stretch$ Uniform=�6   .��5      � $	��800��6   $	��600��6#   .+�5      �� $��Center=�6!   $��Center=�6>   �� 5      $��
/CABBY.png��6   $��Uniform=�5       $��262,0,262,437q�5       �� 5      -   5         ��Name$ UserInputTextBox��5      $��30,343,127,224q�68   
���   �6P   �� 5      -   5         ��Name$ 
SendButton��5      $
��Send��61   $��678,343,24,224q�6Y   �� 5      -   5        $ ChatDisplayTextBlock��5       �� TextWrapping$
 Wrap=�6<   	 �� AcceptsReturn$
	 True��6P   "
 �� VerticalScrollBarVisibility$

 Auto=�6e   $ �� HorizontalScrollBarVisibility$ Disabled=�6�    �� 
IsReadOnly$
 True��6�   $��12(�6�   $��30,149,39,262q�6�   �� 5      -   5        $��
Start Quiz��5      $
��Left=�62   $��
635,95,0,0q�6M   $	��Top=�6a   $��29��6y   $��92��6�   6�   �� 5      $F��@Press the Quiz Button to start 
the Quiz, or type "start quiz" ��6   $
��Left=�6y   $��
596,43,0,0q�6�   $	��Top=�6�   $	��170��6�   $��52��6�   5      5      5      5      5      