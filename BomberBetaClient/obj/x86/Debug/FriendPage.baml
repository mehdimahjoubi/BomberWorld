   M S B A M L   `   `   `  ����N  JBomberContracts_WCF, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullV>clr-namespace:BomberContracts_WCF;assembly=BomberContracts_WCFBomberContracts_WCF  5       BomberBetaClient!   BomberBetaClient.FriendPage   5      W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Mmy>clr-namespace:BomberContracts_WCF;assembly=BomberContracts_WCF     5        N� Title$  
FriendPage��6    � 	Resources 5	         playerDCViewSource��%   &	        �� 5
   
   6   5      .��5      �  � DataContext# [  6    � RowDefinitions 5   
   �� 5      $	��200 �6   �� 5      $��5 �6   �� 5      $��* �6   �� 5      $	��130 �6   .+�5   
   � -   5        $��playerDetailsGrild��5      # [  6)   $��Stretch=�6[   $��
12,22,12,6q�6y   $��Stretch=�6�    � ColumnDefinitions 5      �� 5      $	��125 �6#   �� 5      $��* �6#    5      �� 5      $��32 �6    �� 5      $��32 �6    �� 5      $��32 �6    �� 5      $��* �6    .+�5      �� $��
Pseudonym:��6   $
��Left=�6)   $��0,5,0,0q�6D   $��Center=�6U   $��1��6p   �� 5      -   5        $��pseudonymLabel��5      $��1��67   $��28��6G   $
��Left=�6S   $��6,4,0,0q�6n   $��Center=�6�   $
��auto��6�   $��1��6�   ��6   ��  �� Path$ 	Pseudonym�6�   �� 5       $��Number Of Victories:��6   $��2��63   $
��Left=�6@   $��0,7,0,0q�6[   $��Center=�6l   $��2��6�   �� 5!      -   5        $��numberOfVictoriesLabel��5!   �   $��1��6?   $��2��6O   $��28��6\   $
��Left=�6h   $��6,5,0,0q�6�   $��Center=�6�   ��6   �� $ NumberOfVictories�6�   �� 5"      $��Player Description:��6   $��3��62   $
��Left=�6?   $��0,13,0,0q�6Z   $	��Top=�6l   �� 5#      -   5        $��playerDescriptionLabel��5#   �   $��1q�6   
�������$��1��6<   $��3��6L   $��Stretch=�6Y   $��	6,12,17,6q�6w   $��Stretch=�6�   ._�5$      �� $
��Wrap=�6D   ��6   �� $ PlayerDescription�6D   5%      �� 5&      -   5        $��label1��5&   ^   $��Login :��6   $��28��6&   $
��Left=�62   $��0,6,0,0q�6M   $	��Top=�6l   �� 5'      -   5        $��label2��5'   {   $��1��63   $��28��6C   $
��Left=�6O   $��6,4,0,0q�6j   $	��Top=�6�   ��6   �� $ Login�6�   �� 5(      -   5        $��button1��5(   v   $��Remove friend��6   $��1��6-   $��23��6=   $��Right=�6I   $��0,4,0,0q�6e   $	��Top=�6�   $��98��6�   6�   5)      �� 5*   
   -   5        $��_messageToFriend��5*   �   $��Stretch=�6]   $��15,14,12,12q�6{   $��Stretch=�6�    �� TextWrapping$
 Wrap=�6�   $��3��6�   {�6   �� $ IsConnected���6:   �� $ NextMessage�6�   �� 5+   
   -	   5        $ ��_friendChatHistoryScroller��5+      $��Stretch=�69   $��2��6W   $��	12,6,12,0q�6d   $��Stretch=�6w   .��5,      �� -
   5        $��_friendChatHistory��5,      ��62   �� $ MessagesLog�5-      �� 5.   
   -   5        $��gridSplitter1��5.   d   $��1��6   $��1��6$   $��Stretch=�65   $��0,0,0,0q�6S   $��Stretch=�6y   5/      50      