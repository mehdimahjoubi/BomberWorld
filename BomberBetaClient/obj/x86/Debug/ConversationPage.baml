   M S B A M L   `   `   `  ����N  JBomberContracts_WCF, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullV>clr-namespace:BomberContracts_WCF;assembly=BomberContracts_WCFBomberContracts_WCF  5       BomberBetaClient'   !BomberBetaClient.ConversationPage   5      W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Mmy>clr-namespace:BomberContracts_WCF;assembly=BomberContracts_WCF     5        N� Title$  ConversationPage��6    � 	Resources 5	         conversationDCViewSource��%   &	        �� 5
   
   6   5      .��5      �  � DataContext# [  6    � RowDefinitions 5   
   �� 5      $��92 �6   �� 5      $
��343* �6   �� 5      $	��115 �6   .+�5   
   �� $��Stretch=�6   $��Stretch=�63   $��12,35,12,13q�6Q   $
��Auto=�6f   $��Hidden=�6�   .��5      �� $��1q�6   
�������._�5      �� -   5        $��
textBlock1��5      $��NoWrap=�6V   ��6.   ��  �� Path$ ParticipantsNames�6V   5      5      �� 5   
   -   5        $��_convLogScroller��5      $��Stretch=�6/   $��Stretch=�6L   $��1��6j   $��12,35,12,13q�6w   .��5      �� -   5        $��
textBlock2��5      $
��Wrap=�6P   ��6*   �� $ ConversationLog�6P   5      �� 5   
   -   5        $��_convMsgArea��5       �� TextWrapping$
 Wrap=�6G   $��2��6[   $��Stretch=�6h   $��
12,0,12,12q�6�   $��Stretch=�6�   ��6&   �� $ 
NewMessage�6�   �� 5   
   -   5        $��	_leaveBtn��5   h   $��Leave��6   $��1��6!   $��23��6.   $��Right=�6:   $��0,0,29,0q�6V   $	��Top=�6y   $��97��6�   $y�Hidden=�6�   6�   �� 5   
   -   5        $��_delConvBtn��5      $��Delete party��6$   $��1��6;   $��23��6H   $��Right=�6T   $��0,0,29,0q�6p   $	��Top=�6�   $��97��6�   $y�Visible=�6�   6�   �� 5   
   -   5        $��
_inviteBtn��5   p   $��Invite friends��6   $��1��6*   $��23��67   $
��Left=�6C   $��26,0,0,0q�6^   $	��Top=�6�   $	��102��6�   6�   �� 5   
   -   5        $��_kickBtn��5   o   $��Kick friends��6   $��1��6(   $��23��65   $
��Left=�6A   $��	134,0,0,0q�6\   $	��Top=�6   $��88��6�   6�   �� 5   
   -	   5        $��_mainRoomBtn��5   l   $��	Main room��6   $��1��6%   $��23��62   $
��Left=�6>   $��	279,0,0,0q�6Y   $	��Top=�6�   $��95��6�   6�   �� 5    
   -
   5        $��_playBtn��5       $��Play!��6!   $��1��61   $��23��6>   $
��Left=�6J   $��	409,0,0,0q�6e   $	��Top=�6x   $��55��6�   6�   5!      5"      