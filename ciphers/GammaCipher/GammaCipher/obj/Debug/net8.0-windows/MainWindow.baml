   M S B A M L   `   `   `  ����F  BGammaCipher, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null)clr-namespace:GammaCipherGammaCipher  5   	       GammaCipher.MainWindow   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   +localclr-namespace:GammaCipher     5   	     � Title$  Gamma Cipher��5   	   $	��633��6   $	��855��6+   .��5	      � $��Center=�6   $	��855��6(   ��5
   
   �� 5       �� ImageSource$c ]F:/bachelor/cryptography and information security/Crypto Master Project/img/neutral-back2.jpg��6    j� Stretch$ UniformToFill=�6�   .+�5   
   �� $"��Choose file to import text: ��6   $
��Left=�67   $	��Top=�6R   $��Book Antiqua)�6j   $��14(�6�   
���   �$��	10,10,0,0q�6�   �� 5   
   -   5         ��Name$ FilePathTextBlock1��5      $
��Wrap=�60   $��No file attached.��6D   $
��Left=�6]   $	��Top=�6x   $��Book Antiqua)�6�   $��14(�6�   $��
270,15,0,0q�6�   �� 5   
   -   5        $��Attach file��5      
����ح�
��� d �
����  �$��2q�5      $
��Left=�5      $	��Top=�6,   $��Book Antiqua)�6D   $��14(�6^   $��
198,13,0,0q�6l   6�   �� 5   
   -   5         ��Name$ Encrypt��5      
����ح�
��� d �
����  �$��2q�5      $��Encrypt/Decrypt��5      $
��Left=�6+   $	��Top=�6F   $��Book Antiqua)�6^   $��14(�6x   $��
11,302,0,0q�6�   6�   �� 5   
   -   5        $ SaveFile��5      
����ح�
��� d �
����  �$��2q�5      $
��Save��5       $
��Left=�6    $	��Top=�6;   $��Book Antiqua)�6S   $��14(�6m   $��
11,558,0,0q�6{   6�   �� 5!   
   -   5        $ FilePathTextBlock2��5!      $
��Wrap=�60   $��No file saved.��6D   $
��Left=�6Z   $	��Top=�6u   $��Book Antiqua)�6�   $��14(�6�   $��
55,560,0,0q�6�   �� 5"   
   -   5         ��Name$ 
ResultText��5"       �� TextWrapping$
 Wrap=�6&   $��11,328,9,62q�6:   $��Book Antiqua)�6O   $��14(�6i   
���   �5#      �� 5$   
   -   5        $ ImportedText��5$      $
 Wrap=�6(   $��10,70,10,320q�6<   $��Book Antiqua)�6R   $��14(�6l   
���   �5%      �� 5&   
   -   5        $ KeyText��5&      $
 Wrap=�6#   $��	10,42,0,0q�67   $��Book Antiqua)�6J   $��14(�6d   
���   �$��Center=�6�   $
��Left=�5'   5   $	��Top=�6P   $��23��6h   $	��260��6t   6�   �� 5(   
   -	   5        $ Transfer��5(      
����ح�
��� d �
����  �$��2q�5,      $��Transfer��5-      $
��Left=�6$   $	��Top=�6?   $��Book Antiqua)�6W   $��14(�6q   $��789,302,0,0q�6   6�   �� 5.   
   -
   5        $ PolinomText��5.      $
 Wrap=�6'   $��
275,42,0,0q�6;   $��Book Antiqua)�6O   $��14(�6i   
���   �$��Center=�6�   $
��Left=�5/   1   $	��Top=�6L   $��23��6d   $	��260��6p   6|   51      52      