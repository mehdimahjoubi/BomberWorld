   M S B A M L   `   `   `  ����N  JBomberContracts_WCF, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullV>clr-namespace:BomberContracts_WCF;assembly=BomberContracts_WCFBomberContracts_WCF  5       BomberBetaClient$   BomberBetaClient.AddFriendPage   5      W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Mmy>clr-namespace:BomberContracts_WCF;assembly=BomberContracts_WCF     5        N� Title$  AddFriendPage��6    � 	Resources 5	         playerDCViewSource��%   &	        �� 5
   
   6   5      .��5      �  � DataContext# [  6    � RowDefinitions 5   
   �� 5      $
��0.3* �6   �� 5      $��* �6   .+�5   
   �� -   5        $��_login��5   L   $��23��6   $
��Left=�6   $��	84,69,0,0q�69   $	��Top=�6Z   $	��120��6r   �� 5   
   -   5        $��_pseudo��5      $��23��6!   $
��Left=�6-   $��
333,69,0,0q�6H   $	��Top=�6\   $	��120��6t   �� 5   
   -   5        $��label1��5   \   $��Login :��6   $��28��6"   $
��Left=�6.   $��	33,67,0,0q�6I   $	��Top=�6j   �� 5   
   -   5        $��label2��5   h   $��and/or Pseudonym :��6   $��28��6-   $
��Left=�69   $��
210,67,0,0q�6T   $	��Top=�6v   �� 5   
   -   5        $��button1��5   ]   $��Search��6   $��23��6"   $
��Left=�6.   $��
478,67,0,0q�6I   $	��Top=�6l   $��75��6�   6�   �� 5   
   -   5        $��playerDCListView��5   L   $��1��6   $��
33,0,62,70q�68    �� SelectionMode$ Multiple=�6d   ��6    �� ��5      �� 5      .�5      ��  �� Property � �� �� Value$ Stretch=�6K   �� 5       � ��$ Stretch=�6I   5       �� View 5      �� 5      .P�5      �� -   5        loginColumn��$��Login��5   :    �� Width$ 80��6I   	 �� CellTemplate	 5       �� 5!      .R�5"   "   �� $��
6,-1,-6,-1q�6G   ��6(   �� 
 �� Path$
 Login�6G   5#      5%      �� 5&      -   5        pseudonymColumn��$��	Pseudonym��5&   >   $ 80��6Q   	 5'      �� 5(      .R�5)   "   �� $��-6,-1q�6K   ��6(   �� $
 	Pseudonym�6K   5*      5,      5-      5/      �� 50   
   -	   5        $��button2��50   {   $��Send friendship request��6   $��1��63   $��23��6@   $��Right=�6L   $��	0,0,97,28q�6h   $��Bottom=�6�   $	��145��6�   6�   51      52      