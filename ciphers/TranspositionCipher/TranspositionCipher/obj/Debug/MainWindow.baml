   M S B A M L   `   `   `  ����N  JTranspositionCipher, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null9!clr-namespace:TranspositionCipherTranspositionCipher  5   	   $    TranspositionCipher.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   3local!clr-namespace:TranspositionCipher     5   	     � Title$  Transposition Cipher��5   	   $	��450��6&   $	��800��63   .��5	      � ��5
   
   �� 5       �� ImageSource$c ]F:/bachelor/cryptography and information security/Crypto Master Project/img/neutral-back1.jpg��6    j� Stretch$ UniformToFill=�6�   .+�5   
   �� -   5        $��myTxtbx��5       �� TextWrapping$
 Wrap=�6!   $��Enter text...��65   $��10,131,104,231q�6J   $��Times New Roman)�6b   $��28(�6   $��Center=�6�   ���	#FFBBDEFB
���   ����	#FF0D47A1$ �� HorizontalScrollBarVisibility$ Disabled=�6�   $��5,0,5,0q�6  �� 5   
   $G��AWrite your plaintext or ciphertext and click corresponding button��6   $��0,57,0,0q�6\   
���   �$��Center=�6�   $��Center=�6�   $��Times New Roman)�6�   $��28(�6�   $��Center=�6�   $	��Top=�6  �� 5   
   $��Transposition Cipher��6   $��0,10,0,0q�6/   
���   �$��Center=�6T   $��Center=�6v   $��Times New Roman)�6�   $��28(�6�   $��Center=�6�   $	��Top=�6�   �� 5   
   -   5        $��encrypt��5      
����ح�
��� d �
����  �$��2q�5      $��Encrypt��5   &   $��10,208,589,166q�68   $��Times New Roman)�6P   $��28(�6m   �� 5   
   -   5        $��decrypt��5      
����ح�
��� d �
����  �$��2q�5      $��Decrypt��5   &   $��495,208,104,166q�68   $��Times New Roman)�6Q   $��28(�6n   �� 5   
   -   5        $	��lbl��5      $��Result:��6   $��10,273,10,0q�6-   
���   �$��Center=�6U   $��Center=�6w   $��Times New Roman)�6�   $��28(�6�   $	��Top=�6�   �� 5   
   -   5        $��ResultTextLabel��5      $�� ��6'   $��10,320,10,14q�62   
���   �$��Center=�6[   $��Center=�6}   $��Times New Roman)�6�   $��28(�6�   �� 5   
   -   5        $��	KeyLength��5      $��700,131,10,231q�6$   $��Times New Roman)�6<   $��28(�6Y   $��Center=�6g   $��Center=�6�   6�   �� 5   
   -   5         ��Name$ 	transport��5      
����ح�
��� d �
����  �$��2q�5#      $��	Transport��5$   (   $��252,208,346,166q�6<   $��Times New Roman)�6U   $��28(�6r   5%      5&      