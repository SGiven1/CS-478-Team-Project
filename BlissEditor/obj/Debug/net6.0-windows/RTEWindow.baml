   M S B A M L   `   `   `  ����F  BBlissEditor, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null)clr-namespace:BlissEditorBlissEditor  5   	       BlissEditor.RTEWindow   5   	   W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   +localclr-namespace:BlissEditor     5   	   Lfa=http://schemas.awesome.incremented/wpf/xaml/fontawesome.sharp     5   	   -   5          � Title$  Bliss Editor��5   	    � Icon$ /Images/boplin-icon.png��6   $	��800��6=   $
��1200��6J   
������  � 
ResizeMode$ 	CanResize=�6p    � WindowStartupLocation$ CenterScreen=�6�    J� CommandBindings 5
      )  #System.Windows.Input.CommandBinding  5   
   -   5           Command$ ApplicationCommands.Open��5      6<     5   
   -   5        $ ApplicationCommands.Save��5      6<    � 	Resources 5         	AliceBlue �� Color  AntiqueWhite  Aqua 	 Black  DarkGray  DarkMagenta  	DarkGreen  	Goldenrod  Pink 	 
BlueViolet 	
 Coral  
Chartreuse  Lavender  LavenderBlush  	Firebrick 
 Maroon 
 Orchid  ForestGreen  Moccasin  
LightGreen  MediumSeaGreen  MidnightBlue  
DodgerBlue  LemonChiffon  Honeydew  LightSlateGray  DimGray  	MistyRose  	MintCream 	 Beige  HotPink  	DarkKhaki   	BurlyWood 
! Orange 
" Purple��%  &	        &	 #     &	 I     &	 g     &	 �     &	 �     &	 �     &	 �     &	     &		 1    &	
 U    &	 t    &	 �    &	 �    &	 �    &	     &	 $    &	 D    &	 i    &	 �    &	 �    &	 �    &	 �    &	 !    &	 G    &	 i    &	 �    &	 �    &	 �    &	 �    &	     &	 8    &	  [    &	! ~    &	" �    �� 5   
   $ 	AliceBlue��6,   �� 5   
   $ AntiqueWhite��6/   �� 5   
   $
 Aqua��6'   �� 5   
   $ Black��6(   �� 5   
   $ DarkGray��6+   �� 5   
   $ DarkMagenta��6.   �� 5   
   $ 	DarkGreen��6,   �� 5   
   $ 	Goldenrod��6,   �� 5   
   $
 Pink��6'   �� 5   
   $ 
BlueViolet��6-   �� 5   
   $ Coral��6(   �� 5   
   $ 
Chartreuse��6-   �� 5   
   $ Lavender��6+   �� 5   
   $ LavenderBlush��60   �� 5   
   $ 	Firebrick��6,   �� 5   
   $ Maroon��6)   �� 5   
   $ Orchid��6)   �� 5    
   $ ForestGreen��6.   �� 5!   
   $ Moccasin��6+   �� 5"   
   $ 
LightGreen��6-   �� 5#   
   $ MediumSeaGreen��61   �� 5$   
   $ MidnightBlue��6/   �� 5%   
   $ 
DodgerBlue��6-   �� 5&   
   $ LemonChiffon��6/   �� 5'   
   $ Honeydew��6+   �� 5(   
   $ LightSlateGray��61   �� 5)   
   $ DimGray��6*   �� 5*   
   $ 	MistyRose��6,   �� 5+   
   $ 	MintCream��6,   �� 5,   
   $ Beige��6(   �� 5-   
   $ HotPink��6*   �� 5.   
   $ 	DarkKhaki��6,   �� 5/   
   $ 	BurlyWood��6,   �� 50   
   $ Orange��6)   �� 51   
   $ Purple��6)   52      .��53      ]� ���	#FF531167.+�54   
   c� $	��Top=�6   ���	#FF531167.@�55      �� $��ApplicationCommands.Open��6   .��56      �� $��/Images/Folder.png��6   $��16��64   $��16��6?   57      �� 58      $��ApplicationCommands.Save��6   .��59      �� $��/Images/Save Disk.png��6   $��16��67   $��16��6B   5:      �� 5;      d� 5<      -   5        $��btnBold��5<   @   $ ��EditingCommands.ToggleBold��6   .��5=      �� $��/Images/Bold.png��6   $��16��62   $��16��6=   5>      d� 5?      -   5        $��	btnItalic��5?   B   $"��EditingCommands.ToggleItalic��6   .��5@      �� $��/Images/Italics.png��6   $��16��65   $��16��6@   5A      d� 5B      -   5        $��btnUnderline��5B   E   $%��EditingCommands.ToggleUnderline��6   .��5C      �� $��/Images/Underline.png��6   $��16��67   $��16��6B   5D      �� 5E      �� 5F      -   5        .��5G      �� $��/Images/InsertImage.jpg��6   $��16��69   $��16��6D   5H      �� 5I      �� 5J      -   5        $��cmbFontFamily��5J      $	��150��6,   68   �� 5K      -	   5        $��cmbFontSize��5K      $��50��6*    �� 
IsEditable$
 True��65   $��20��6y   �� 5z      -
   5        $��btnTable��5z      .��5{      �� -   5        	 ��Name$	 	btnTable1��5{      $��/Images/Table.png��6+   $��16��6F   $��16��6Q   5|      �� 5}      -   5        
 ��Name$
 btnExportPDF��5}      .��5~      �� -   5        $	 btnExportPDF1��5~      $��/Images/ExportPDF.png��6/   $��16��6N   $��16��6Y   5      5�      ]� 5�   
   $��False��6   $	��100��6*   ���	#FF531167$	��709��6M   .+�5�      ��  �� CornerRadius$ 10 10 10 10��6   $	��100��60   $��40��6<   $	��Top=�6H   ��5�      �� 5�       �� EndPoint$ 0.5,1&�6*    �� 
StartPoint$ 0.5,0&�6;   .��5�      �  � Color$ 	#FFD061F1��6'   � 5�      $ 	#FF531167��6'    � Offset$ 1Q�69   5�      ._�5�      �� -   5        $
 NewRtfNoteBtn��5�      $	��Top=�60   $��25��6E   $��	7,10,7,10q�6Q   
������ $��SemiBold"�6�   
�������
������ $��Lucida Console)�6�   $��10(�6�   .��6  New RTF Note6  5�      �� 5�      $ 10 10 10 10��6   $	��100��60   $��40��6<   $	��Top=�6H   $��-100 50 0 0q�6`   ��5�      �� 5�      $ 0.5,0&�6*   $ 0.5,1&�6=   .��5�      � $ 	#FFD061F1��6'   $ 0Q�69   � 5�      $ 	#FF531167��6'   $ 1Q�69   5�      ._�5�      �� -   5        $
 NewMarkdownNoteBtn��5�      $	��Top=�65   $��25��6J   $��	7,10,7,10q�6V   
������ $��SemiBold"�6�   
�������
������ $��10(�6�   $��Lucida Console)�6�   .��6
  New MD Note6  5�      5�      ]� 5�   
   .+�5�      c� $
��Left=�6   $��2��6,   .@�5�      �� $	��100��6   $��6��6,   �� 5�      $��Center=�6   $
��Auto��6<   $	��Top=�6J   $��5��6b    K� ZIndex$	 -15��6l   5�      5�      �� 5�   
   -   5         ��Name$ RTETabControl��5�      $	��719��6,   $
��1082��69    �� ItemStringFormat$  ��6F   $��0,10,9,0q�6Z   .@�5�      �� ��5�      �� 5�      $��RTF Note��6    .��5�      �� -   5         ��Name$ 	rtbEditor��5�      " �� VerticalScrollBarVisibility$
 Auto=�6_    �� 
LineHeight$ 2��6�    �� 	IsEnabled$
 True��6�    J� RenderTransformOrigin$ 0.5,0.5&�6�   $��Right=�6�   $
��Auto��6�   $��0,0,2,0q�6�   $	��690��6  5�      5�      �� 5�      ��5�      �� 5�      $��Markdown Note��6    .��5�      �  � RowDefinitions 5�      �� 5�      $	��23* �6(   �� 5�      $
��668* �6(    � ColumnDefinitions 5�      �� 5�      $	��39* �6+   �� 5�      $
��503* �6+   �� 5�      $
��542* �6+   .+�5�      �� -   5        $ 	mdTextBox��5�   "   $��0��65   $��0,0,6,2q�6F   $ 2��6W   $��2��6�   $��2��6�   (  "System.Windows.Controls.WebBrowser  5�      -   5          Name$ 
webBrowser��5�   !   $��2��66   $��4,5,5,5q�6F   $��2��6W   5�      5�      5�      5�      5�      