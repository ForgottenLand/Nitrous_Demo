Local $currentPort = 25005
Local $endPort = 25005

While $currentPort <= $endPort

   Run("C:\MultiplayerProject\Bin\LithiumDemo.exe")
   Sleep(1000)

   MouseClick("left",683,460,1,20)
   Sleep(1000)

   MouseClick("left",683,468,1,20)
   Sleep(1000)

   Send("{Enter}")
   Sleep(1000)

   MouseClick("left",450,260,1,20)
   Sleep(1000)

   MouseClickDrag("left",370,165,550,165,20)
   Sleep(1000)

   Send("Test Host{TAB}" & $currentPort)

   MouseClick("left",800,165,1,20)
   Sleep(1000)

   MouseClick("left",920,125,1,20)
   Sleep(1000)

   $currentPort = $currentPort + 1
WEnd
