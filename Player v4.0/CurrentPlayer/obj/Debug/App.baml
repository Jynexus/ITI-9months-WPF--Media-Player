   M S B A M L   `   `   `  ����  CurrentPlayer    CurrentPlayer.App   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5        �� 	Resources  5         SliderRepeatButton �� 
TargetType �� Property �� Value �� 
TargetType  SliderRepeatButton1 J� SnapsToDevicePixels  SliderThumb 
 Slider � RowDefinitions m�Name m� Fill	 m� 	Placement
 ��Name ]�Name ]� DecreaseRepeatButton ]� Thumb ]� IncreaseRepeatButton �� Triggers P� Property P� Value �� 
TargetName  Horizontal_Slider �� Triggers��%�
  &	        &	 D    &	 k    &	 �    7  7  7   &	 �    7  �� 5   
   " ��6+   .�5      ��  �@J�SnapsToDevicePixels . 64   �� 5       �@�OverridesDefaultStyle . 66   �� 5	       � �� .  6*   �� 5
       � |� .  6*   �� 5       � �� 5      �� 5      " ��6&   .R�5      �� 
������ 6!   5      5      5      �� 5   
   " ��6,   .�5      ��  �@J�SnapsToDevicePixels . 64   �� 5       �@�OverridesDefaultStyle . 66   �� 5       � �� 5      �� 5      " ��6&   .R�5      ��  . 
�����F�$��1q�6T   
�����F�$��3��6�   5      5      5      �� 5    
   " n�6$   .�5!      ��  �@J�SnapsToDevicePixels . 64   �� 5"       �@�OverridesDefaultStyle . 66   �� 5#       � �� 5$      �� 5%      " n�6&   .R�5'      � .+�5(      A� 
����ح�$��15��67   $��15��6C   A� 5)      
�������$��7��66   $��7��6A   5*      5,      5.      5/      �� 51   
   " ��6*   .R�52      �  53      �� 54      $
��Auto �6$   �� 55      $
��Auto �6$   #��z��62   �� 56      $
��Auto �6$   .+�58      m� $ TopTick��6   
 �����$	��Top=�6>    . $��0��6s   $		 Top=�6�   $��5��6�   $y�Hidden=�6�   �� 59      $
 TrackBackground��6   $��Center=�62   $��	0,-10,0,0q�6M   
�������
�������$��3��6�   $��1��6�   $��1q�6�   ]� 5:      $ 
PART_Track��6%   $��1��6   $��	0,-10,0,0q�69    5;      �� 5<      8��  $��Slider.DecreaseLarge��6U    5>      n� 5?      8�� $��0,0,0,0q�6E    5A      �� 5B      8�� $��Slider.IncreaseLarge��6S   5D      m� 5E      $ 
BottomTick��6   
 ����� . $��2��6\   $	 Bottom=�6k   $��4��6~   $y�	Collapsed=�6�   5F       5G      P� 5H       �@��TickPlacement$ TopLeft=�64   . �5I      �� $ TopTick��6    � y�$ Visible=�6I   5J      P� 5K       �@��TickPlacement$ BottomRight=�63   . �5L      �� $ 
BottomTick��6    � y�$ Visible=�6M   5M      P� 5N       �@��TickPlacement$
 Both=�63   . �5O      �� $ TopTick��6    � y�$ Visible=�6H   �� 5P      $ 
BottomTick��6    � y�$ Visible=�6K   5Q      5S      �� 5U   
   " ��6*   .�5V      ��  � |� .  6*   �� 5W       �@J�SnapsToDevicePixels . 64   �� 5X       �@�OverridesDefaultStyle . 66    5Y      P� 5Z       �@��Orientation$ 
Horizontal=�61   . �5[      ��  � ��$ 21��62   �� 5\       � ��$	 104��61   �� 5]       � ��8   61   5^      5`      5a      5b      