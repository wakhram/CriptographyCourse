   M S B A M L   `   `   `  ����N  JElGamalDigSignature, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null9!clr-namespace:ElGamalDigSignatureElGamalDigSignature  5   	   $    ElGamalDigSignature.MainWindow   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   3local!clr-namespace:ElGamalDigSignature     5   	     � Title$  ElGamal Digital Signature��5   	   $	��547��6+   $	��600��68   .��5      � $��10q�6   .+�5   
   �� .+�5	      �� -   5        $��GenerateKeysButton��5	      
����ح�
��� d �
����  �$��2q�5      $��Generate Keys��5      $��5q�6N   �� 5      $��Public Key (p, g, y):��6   $��5q�65   �� 5      -   5        $��PublicKeyTextBox��5       �� 
IsReadOnly$
 True��6.   $��5q�6@   $��50��6K   �� 5      $��Message:��6   $��5q�6(   �� 5      -   5        $��MessageTextBox��5      $��5q�6,   �� 5      -   5        $��
SignButton��5      
����ح�
��� d �
����  �$��2q�5      $��Sign Message��5      $��5q�6E   �� 5      $��Signature (S1, S2):��6   $��5q�63   �� 5      -   5        $��SignatureTextBox��5      $
 True��6.   $��5q�6@   $��50��6K   �� 5!      -   5        $��VerifyButton��5!      
����ح�
��� d �
����  �$��2q�5%      $��Verify Signature��5&      $��5q�6K   �� 5'      $��Verification (V1, V2):��6   $��5q�66   �� 5(      -   5         ��Name$ VerificationTextBox��5(      $
 True��63   $��5q�6E   $��56��6P   �� 5)      $��Verification Result:��6   $	��570��64   $��21��6@   $��5,5,5,5q�6L   �� 5*      -   5        $ VerificationResultTextBox��5*      $
 True��69   $��30��6K   $	��566��6W   $��5,5,5,5q�6c   5+      5,      5-      