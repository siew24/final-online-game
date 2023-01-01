Environment Design Notes

Search for locked doors
- specific doors are renamed to "DoorLocked_" + "xxxArea"
- area names: FirstArea, SecondArea, ThirdArea, BossArea

Searh for hint game objects
- hints are renamed as "HintXX_AreaX"
- X are numbers
- Area0: 2 hints (outside)
- Area1: 3 hints (inside)
- Area2: 4 hints (inside)
- Area3: 5 hints (inside)

Turn off signals (in control room)
- 2 signal triggers that need to be pressed at the same time to turn it off (multiplayer aspect coop), named as "SignalTriggerX"
- "SignalAnimation" hologram object has parts named as "PivotXX", should be able to animate it by setActive = false one by one
- the animation depends on you guys want the players to hold the trigger button then need wait until the animation end to indicate successful OR once press the animation just plays

Boss Location
- game object location named as "Engine/BossLocation"

Enemy
- floating robot named "Char_Robot_Ico"