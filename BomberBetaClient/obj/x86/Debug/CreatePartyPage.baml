   M S B A M L   `   `   `  ����N  JBomberContracts_WCF, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullV>clr-namespace:BomberContracts_WCF;assembly=BomberContracts_WCFBomberContracts_WCF  5       BomberBetaClient&    BomberBetaClient.CreatePartyPage   5      W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Mmy>clr-namespace:BomberContracts_WCF;assembly=BomberContracts_WCF     5        N� Title$  CreatePartyPage��6    � 	Resources 5	         playerDCViewSource��%   &	        �� 5
   
   6   5      .��5      �  � DataContext# [  6    � RowDefinitions 5   
   �� 5      $
��425* �6   �� 5      $
��125* �6   .+�5   
   �� -   5        $��playerDCListView��5   A   $��93,122,74,45q�6+    �� SelectionMode$ Multiple=�6Y   ��6   �� ��5      �� 5      .�5      ��  �� Property � �� �� Value$ Stretch=�6K   �� 5       � ��$ Stretch=�6I   5       �� View 5      �� 5      .P�5      �� -   5        loginColumn��$��Login��5   :    �� Width$	 125��6I   	 �� CellTemplate	 5      �� 5      .R�5   "   �� $��
6,-1,-6,-1q�6G   $��Center=�6[   ��6(   �� 
 �� Path$
 Login�6[   5      5       �� 5!      -   5        pseudonymColumn��$��	Pseudonym��5!   >   $	 125��6Q   	 5"      �� 5#      .R�5$   "   �� $��-6,-1q�6K   $��Center=�6Z   ��6(   �� $
 	Pseudonym�6Z   5%      5'      �� 5(      -   5        numberOfVictoriesColumn��$��Number Of Victories��5(   F   	 5)      �� 5*      .R�5+   "   �� $��-6,-1q�6S   $��Center=�6b   ��6(   �� $
 NumberOfVictories�6b   5,      5.      �� 5/      -   5        statusColumn��$��Status��5/   ;   $	 110��6K   	 50      �� 51      .R�52   "   �� $��-6,-1q�6N   $��Center=�6]   ��6(   �� $
 PlayerStatus�6]   53      55      56      58      �� 59   
   -   5        $��label1��59   e   $��Choose friends :��6   $��28��6+   $
��Left=�67   $��	93,47,0,0q�6R   $	��Top=�6s   �� 5:   
   -   5        $��button1��5:   r   $��Invite friends��6   $��1��6*   $��23��67   $��Right=�6C   $��	0,0,182,0q�6_   $	��Top=�6�   $	��105��6�   6�   5;      5<      