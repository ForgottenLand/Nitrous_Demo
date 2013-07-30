Local $currentPort = 25005
Local $endPort = 25005

While $currentPort <= $endPort

   Run("C:\MultiplayerProject\Bin\LithiumDemo.exe")
   Sleep(1000)

   MouseClick("left",836,595,1,20)
   Sleep(1000)

   MouseClick("left",836,615,1,20)
   Sleep(1000)

   Send("{Enter}")
   Sleep(5000)

   MouseClick("left",575,390,1,20)
   Sleep(1000)

   MouseClickDrag("left",530,310,680,310,20)
   Sleep(1000)

   Send("Test Host{TAB}" & $currentPort)

   MouseClick("left",1000,310,1,20)
   Sleep(1000)

   MouseClick("left",1080,285,1,20)
   Sleep(1000)

   $currentPort = $currentPort + 1

WEnd
