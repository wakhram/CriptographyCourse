   M S B A M L   `   `   `  ����R  NBreakingTheCaeserCipher, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullA%clr-namespace:BreakingTheCaeserCipherBreakingTheCaeserCipher  5   	   (    "BreakingTheCaeserCipher.MainWindow   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   7local%clr-namespace:BreakingTheCaeserCipher     5   	     � Title$  Breaking Caesar Cipher��5   	   $	��522��6(   $
��1072��65   .��5	      � $��0,9,0,0q�6   $��Center=�6   $
��1072��69   ��5
   
   �� 5       �� ImageSource$a [F:/bachelor/cryptography and information security/Crypto Master Project/img/caesar-back.jpg��6    j� Stretch$ UniformToFill=�6�   .+�5   
   �� $.��(Choose file to break the caeser cipher: ��6   $
��Left=�6C   $	��Top=�6^   $��Book Antiqua)�6v   $��14(�6�   
�������$��	10,10,0,0q�6�   �� 5   
   -   5         ��Name$ FilePathTextBlock1��5      $
��Wrap=�60   $��No file attached.��6D   $
��Left=�6]   $	��Top=�6x   $��Book Antiqua)�6�   $��14(�6�   $��
335,15,0,0q�6�   
�������6�   �� 5   
   -   5        $��Attach file��5      
����ح�
��� d �
����  �$��2q�5      $
��Left=�5      $	��Top=�6,   $��Book Antiqua)�6D   $��14(�6^   $��
262,13,0,0q�6l   6�   �� 5   
   -   5        $ EcnryptedInputText��5      $
��Wrap=�60   $$��Here will be encrypted text...��6D   $��10,42,536,123q�6j   $��Book Antiqua)�6�   $��14(�6�   
�������6�   �� 5   
   -   5        $ 
BrokenText��5      $
��Wrap=�6(   $!��Here will be broken text...��6<   $��536,42,10,123q�6_   $��Book Antiqua)�6v   $��14(�6�   $	��526��6�   
�������6�   �� 5   
   -   5         ��Name$ 
NextLetter��5      
����ح�
��� d �
����  �$��2q�5      $��Next letter��5      $
��Left=�6'   $	��Top=�6B   $��Book Antiqua)�6Z   $��14(�6t   $��992,342,0,0q�6�   6�   �� 5   
   -   5        $ PreviousLetter��5      
����ح�
��� d �
����  �$��2q�5!      $��Previous letter��5"      $
��Left=�6+   $	��Top=�6F   $��Book Antiqua)�6^   $��14(�6x   $��
10,342,0,0q�6�   6�   �� 5#   
   -   5        $ SaveFile��5#      
����ح�
��� d �
����  �$��2q�5'      $
��Save��5(      $��Center=�6    $	��Top=�6=   $��Book Antiqua)�6U   $��14(�6o   $��	0,342,0,0q�6}   6�   �� 5)   
   -   5        $ FilePathTextBlock2��5)      $
��Wrap=�60   $��No file saved.��6D   $
��Left=�6Z   $	��Top=�6u   $��Book Antiqua)�6�   $��14(�6�   $��
10,383,0,0q�6�   
�������6�   �� 5*   
   -	   5        $ AmountOfAttemtps��5*      $
��Wrap=�6.   $��Amount of attempts:��6B   $
��Left=�6]   $	��Top=�6x   $��Book Antiqua)�6�   $��14(�6�   $��
10,405,0,0q�6�   
�������6�   �� 5+   
   -
   5        $ KeyLengthTextBlock��5+      $
��Wrap=�60   $��Key's length:��6D   $
��Left=�6Y   $	��Top=�6t   $��Book Antiqua)�6�   $��14(�6�   $��
10,427,0,0q�6�   
�������6�   �� 5,   
   -   5        $ MostFrequencyLetter��5,      $
��Wrap=�61   $��Letter:��6E   $
��Left=�6T   $	��Top=�6o   $��Book Antiqua)�6�   $��14(�6�   $��
10,449,0,0q�6�   
�������6�   5.      5/      