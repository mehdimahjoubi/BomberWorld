   M S B A M L   `   `   `  ����N  JBomberContracts_WCF, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullV>clr-namespace:BomberContracts_WCF;assembly=BomberContracts_WCFBomberContracts_WCF  5       BomberBetaClient   BomberBetaClient.HomePage   5      W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Mmy>clr-namespace:BomberContracts_WCF;assembly=BomberContracts_WCF     5        N� Title$  HomePage��6    � 	Resources 5	         playerDCViewSource��%   &	        �� 5
   
   6   5      .��5      �  � DataContext# [  6    � RowDefinitions 5   
   �� 5      $
��414* �6   �� 5      $
��136* �6   .+�5   
   � -   5        $��grid1��5   t   # [  6   $��Stretch=�6A   $��64,92,48,19q�6_   $��Stretch=�6�   $	��538��6�    � ColumnDefinitions 5      �� 5      $	��128 �6#   �� 5      $��* �6#    5      �� 5      $��35 �6    �� 5      $��35 �6    �� 5      $��* �6    .+�5      �� $��Number Of Victories:��6   $
��Left=�63   $��0,0,0,10q�6N   $��Center=�6`   �� 5      -   5        $��numberOfVictoriesLabel��5   �   $��1��6?   $��28��6O   $
��Left=�6[   $��3,0,0,6q�6v   $��Center=�6�   ��6   ��  �� Path$ NumberOfVictories�6�   �� 5      $��Player Description:��6   $��2��62   $
��Left=�6?   $��3,17,0,0q�6Z   $	��Top=�6l   �� 5      -   5        $��playerDescriptionTextBox��5   �    �� TextWrapping$
 Wrap=�6    �� AcceptsReturn . $��1��6?   $��2��6O   $��Stretch=�6\   $��3,4,6,6q�6z   $��Stretch=�6  ��6�   �� $ PlayerDescription� �� Mode$ TwoWay=�	 �� ValidatesOnExceptions	 . 
 �� NotifyOnValidationError
 . 6  �� 5      $��
Pseudonym:��6   $��0��6)   $��1��69   $
��Left=�6F   $��3q�6a   $��Center=�6l   �� 5       -   5        $��pseudonymTextBox��5    k   $��1��6   $��1��6&   $��23��63   $
��Left=�6?   $��3,3,0,3q�6Z   $��Center=�6�   $	��245��6  ��6�   �� $ 	Pseudonym�$ TwoWay=�	 . 
 . 6  5!      �� 5"   
   -   5        $��button1��5"   c   $��Save profil��6   $��23��6'   $��Right=�63   $��
0,42,250,0q�6O   $	��Top=�6r   $	��111��6�   $��1��6�   6�   �� 5#   
   -   5        $��button2��5#   e   $��Cancel changes��6   $��23��6*   $��Right=�66   $��	0,42,95,0q�6R   $	��Top=�6t   $	��111��6�   $��1��6�   6�   5$      5%      