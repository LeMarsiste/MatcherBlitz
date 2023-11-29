<br/>
<p align="center">
  <h3 align="center">Memory-Craft Chronicles Vol.1</h3>

  <p align="center">
    A Pixel Art Memory game inspired by World of Warcraft
    <br/>
    <br/>
  </p>
</p>



## About The Project

The project features an editor which lets you switch levels and create levels using its LevelEditor tab, You can find it in the Tools->MatcherBlitz->Editor.
The Main game itself is seperated into 3 different controllers, one for keeping the data (DataCenter.cs) another for the general logic of the game (GameManager.cs) and last but not least the UI Controller (UIController.cs).
The game features different themes which can be set from the editor and you can add new themes by adding them to the LevelThemes.cs and adding their respective ScriptableObject to DataCenter!
The tiles will be pulled from an object pool so dont worry about performance issues from creating too many tiles in your levels, but currently, if you create more than 50 active tiles the tile images (the ones that get shown when you click on them) will have duplicates between them (so there might be 4 of the same picture but they will be 2 different pairs)

Currently the game has 4 tile types:
1- Holes : basically doesnt show on the map
2- Normal : Your day to day tile
3- Explosive: They have more score and add some time to the timer
4- Golden: The better version of the Explosives 
(Visual cues and more effects to be added in the future)



## Roadmap

1- Adding a campaign would be nice, with some voice acting
2- There are some Screen scalling issues that need to be addressed
3- An Options pop-up would be also amazing to have
4- Adding additional effects to the gameboard for GOLDEN and EXPLOSIVE tiles
5- Changing the Name from "MatcherBlitz" to "Memory-Craft Chronicles" (because i changed my game design xD)

## Contributing



### Creating A Pull Request

1. Fork the Project
2. Create your Feature Branch (`git checkout -b feature/AmazingFeature`)
3. Commit your Changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the Branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## Authors

* **Ahmad R Satvati** - *The Mirza* - [Ahmad R Satvati](https://github.com/LeMarsiste) - *Built the MCC Vol.1*


