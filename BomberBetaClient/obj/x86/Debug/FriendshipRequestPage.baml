   M S B A M L   `   `   `  ����N  JBomberContracts_WCF, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullV>clr-namespace:BomberContracts_WCF;assembly=BomberContracts_WCFBomberContracts_WCF  5        BomberBetaClient,   &BomberBetaClient.FriendshipRequestPage   5      W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Mmy>clr-namespace:BomberContracts_WCF;assembly=BomberContracts_WCF     5         N� Title$  FriendshipRequestPage��6    � 	Resources 5	         playerDCViewSource��%   &	        �� 5
   
   6   5      .��5      �  � RowDefinitions 5   
   �� 5      $
��374* �6   �� 5      $
��176* �6   .+�5   
   � -   5        $��grid1��5   s    � DataContext# [  6   $��Stretch=�6A   $��
12,55,12,0q�6_   $��Stretch=�6�    � ColumnDefinitions 5      �� 5      $	��125 �6#   �� 5      $��* �6#    5      �� 5      $��35 �6    �� 5      $��35 �6    �� 5      $��35 �6    �� 5      $��* �6    .+�5      �� $��
Pseudonym:��6   $
��Left=�6)   $��0,5,0,0q�6D   $��Center=�6U   $��1��6p   �� 5      -   5        $��pseudonymLabel��5      $��1��67   $��28��6G   $
��Left=�6S   $��6,3,0,0q�6n   $��Center=�6�   $
��auto��6�   $��1��6�   ��6   ��  �� Path$ 	Pseudonym�6�   �� 5      $��Number Of Victories:��6   $��2��63   $
��Left=�6@   $��0,7,0,0q�6[   $��Center=�6l   $��2��6�   �� 5      -   5        $��numberOfVictoriesLabel��5   �   $��1��6?   $��2��6O   $��28��6\   $
��Left=�6h   $��6,5,0,0q�6�   $��Center=�6�   ��6   �� $ NumberOfVictories�6�   �� 5       $��Player Description:��6   $��3��62   $
��Left=�6?   $��	0,12,0,32q�6Z   $	��Top=�6m   �� 5!      -   5        $��playerDescriptionLabel��5!   �   $��1q�6   
�������$��1��6<   $��3��6L   $��Stretch=�6Y   $��6,12,6,6q�6w   $��Stretch=�6�   ._�5"      �� $
��Wrap=�6D   ��6   �� $ PlayerDescription�6D   5#      �� 5$      -   5        $��label1��5$   ^   $��Login :��6   $��28��6&   $
��Left=�62   $��0,6,0,0q�6M   $	��Top=�6l   �� 5%      -   5        $��label2��5%   {   $��1��63   $��28��6C   $
��Left=�6O   $��6,6,0,0q�6j   $	��Top=�6�   ��6   �� $ Login�6�   5&      �� 5'   
   -   5        $��button1��5'   j   $��Refuse��6   $��1��6"   $��23��6/   $��Right=�6;   $��	0,21,70,0q�6W   $	��Top=�6y   $��75��6�   6�   �� 5(   
   -   5        $��button2��5(   k   $��Accept��6   $��1��6"   $��23��6/   $��Right=�6;   $��
0,21,165,0q�6W   $	��Top=�6z   $��75��6�   6�   5)      5*      